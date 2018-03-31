﻿using JWT;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace PERI.Agenda.BLL
{
    public class VerifyUserAttribute : ActionFilterAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Get authorization header
            var request = filterContext.HttpContext.Request;
            var authHeader = request.Headers["Authorization"];

            // Validate token
            var secret = Core.Setting.Configuration.GetValue<string>("JWT:Secret");
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            try
            {
                var payload = decoder.DecodeToObject<Dictionary<string, object>>(authHeader, secret, verify: true);
                var userId = Convert.ToInt32(payload["id"]);

                // Verifiy user
                var context = new EF.AARSContext();
                var bll_u = new BLL.EndUser(context);
                var user = bll_u.Get(new EF.EndUser { UserId = userId }).Result;

                if (user.DateInactive != null)
                {
                    logger.Error("Unauthorized.");
                    filterContext.Result = new UnauthorizedResult();
                }
                else
                {
                    filterContext.HttpContext.Items.Add("EndUser", user);
                }
            }
            catch (TokenExpiredException ex)
            {
                logger.Error(ex);
                filterContext.Result = new StatusCodeResult(412);
            }
            catch (SignatureVerificationException ex)
            {
                logger.Error(ex);
                filterContext.Result = new StatusCodeResult(417);
            }
            catch (Exception ex)
            {
                // BadRequest
                logger.Error(ex);
                filterContext.Result = new BadRequestResult();
            }
        }
    }
}

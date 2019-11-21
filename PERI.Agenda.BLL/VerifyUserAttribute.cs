using JWT;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PERI.Agenda.BLL
{
    /// <summary>
    /// Verifiies the token of the request
    /// </summary>
    public class VerifyUserAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Specify allowed Roles. Enter names separated by comma
        /// </summary>
        public string AllowedRoles { get; set; }

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
                string token;
                if (((ClaimsIdentity)filterContext.HttpContext.User.Identity).Claims.Count() > 0)
                    token = ((ClaimsIdentity)filterContext.HttpContext.User.Identity).FindFirst(ClaimTypes.UserData).Value;
                else
                    token = authHeader;    

                var payload = decoder.DecodeToObject<Dictionary<string, object>>(token, secret, verify: true);
                var userId = Convert.ToInt32(payload["id"]);

                // Verifiy user
                var unitOfWork = new UnitOfWork(new EF.AARSContext());
                var bll_u = new BLL.EndUser(unitOfWork);
                var user = bll_u.GetById(userId).Result;

                // Allowed Roles
                if (AllowedRoles != null)
                {
                    var roles = AllowedRoles.Split(',');
                    if (roles.Count() > 0 && !roles.Contains(user.Role.Name))
                    {
                        logger.Error("Unauthorized.");
                        filterContext.Result = new UnauthorizedResult();
                    }
                }

                if (user.DateInactive != null)
                {
                    logger.Error("Unauthorized.");
                    filterContext.Result = new UnauthorizedResult();
                }
                else
                {
                    if (filterContext.HttpContext.Items["EndUser"] == null)
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

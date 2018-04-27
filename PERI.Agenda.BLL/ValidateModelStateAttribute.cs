using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PERI.Agenda.BLL
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                var msgs = "";
                foreach (var modelState in context.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        msgs += error.ErrorMessage + "\n";
                        logger.Error(error);
                    }
                }

                logger.Error(msgs);

                var res = new ObjectResult(msgs)
                {
                    StatusCode = 403,
                    Value = msgs
                };

                context.Result = res;
            }
        }
    }
}

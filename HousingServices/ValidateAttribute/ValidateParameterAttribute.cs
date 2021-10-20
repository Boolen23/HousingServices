using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HousingServices.ValidateAttribute
{
    public class ValidateParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionArguments.ContainsKey("Account_ID"))
            {
                string Account_ID = filterContext.ActionArguments["Account_ID"]?.ToString();
                if (!int.TryParse(Account_ID, out int _Account_ID))
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    filterContext.Result = new JsonResult($"{(string.IsNullOrEmpty(Account_ID) ? "Empty string" : Account_ID)} is not a number!");
                }
            }
            if (filterContext.ActionArguments.ContainsKey("Period"))
            {
                int? Period = filterContext.ActionArguments["Period"] as int?;
                if (!Period.HasValue)
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    filterContext.Result = new JsonResult($"Period was null!");
                }
                if(Period != 1 && Period != 2 && Period != 3)
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    filterContext.Result = new JsonResult($"Unexpected period!");
                }
            }
        }
    }
}

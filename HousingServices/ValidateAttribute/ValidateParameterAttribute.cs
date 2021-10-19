using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HousingServices.ValidateAttribute
{
    public class ValidateAccountIDParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionArguments.ContainsKey("Account_ID"))
            {
                string Account_ID = filterContext.ActionArguments["Account_ID"].ToString();
                if (!int.TryParse(Account_ID, out int _Account_ID))
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    filterContext.Result = new JsonResult($"{Account_ID} is not a number!");
                }
            }
        }
    }
}

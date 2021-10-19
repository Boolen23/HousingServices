using HousingServices.Model;
using HousingServices.ValidateAttribute;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HousingServices.Controllers
{
    public class ApiController : Controller
    {
        public ApiController()
        {

        }
        public IActionResult Index() => View();

        [HttpGet("/Getdata")]
        [Produces("text/xml", "text/csv")]
        [ValidateAccountIDParameter]
        public IActionResult Getdata(string Account_ID)
        {
            try
            {
                var t = BalanceCalculator.LoadByAccountId(int.Parse(Account_ID));
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(ex.Message);
            }
            return Ok(Data());
        }

        [HttpGet("/GetBalances")]
        public IActionResult GetBalances()
        {
            return Ok(Data());
        }


        private static IEnumerable<Balance> Data()
        {
            var model = new List<Balance>
            {
                new Balance()
                {
                    account_id = 23
                },
                new Balance()
                {
                    account_id = 25
                },
            };

            return model;
        }


    }
}

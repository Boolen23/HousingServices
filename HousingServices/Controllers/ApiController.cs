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
        public IActionResult Index() => View();

        [ValidateParameter]
        [HttpGet("/GetBalances")]
        [Produces("text/xml", "text/csv")]
        public IActionResult GetBalances(string Account_ID, int Period)
        {
            try
            {
                var bc = BalanceCalculator.LoadByAccountId(int.Parse(Account_ID));
                return Ok(bc.GetReport(Period));
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(ex.Message);
            }
        }
        [ValidateParameter]
        [HttpGet("/GetArrearsInPayment")]
        public IActionResult GetArrearsInPayment(string Account_ID)
        {
            try
            {
                return Ok(BalanceCalculator.GetArrearsInPayment(int.Parse(Account_ID)));
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(ex.Message);
            }
        }


    }
}

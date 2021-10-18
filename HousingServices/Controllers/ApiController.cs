using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousingServices.Controllers
{
    public class ApiController : Controller
    {
        public ApiController()
        {

        }
        public IActionResult Index() => View();

    }
}

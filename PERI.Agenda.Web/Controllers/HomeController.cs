using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PERI.Agenda.Web.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        [Route("privacy")]
        public IActionResult Privacy()
        {
            ViewData["Title"] = "Privacy Policy";
            return View("Privacy");
        }

        [Route("terms")]
        public IActionResult Terms()
        {
            ViewData["Title"] = "Terms";
            return View("Terms");
        }

        [Route("about")]
        public IActionResult About()
        {
            ViewData["Title"] = "About";
            return View("About");
        }

        [Route("version")]
        public IActionResult Version()
        {
            return Json(Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion);
        }
    }
}

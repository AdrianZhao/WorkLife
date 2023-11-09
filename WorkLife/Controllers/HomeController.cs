using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkLife.Areas.Identity.Data;
using WorkLife.Data;
using WorkLife.Models;
using WorkLife.Models.WorkLifeLogicLayer;

namespace WorkLife.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Employer"))
            {
                return RedirectToAction("Index", "Employers");
            }
            else if (User.IsInRole("Applicant"))
            {
                return RedirectToAction("Index", "Applicants");
            }
            var choseRoleClaim = User.FindFirst("ChoseRole");
            if (choseRoleClaim != null)
            {
                string choseRole = choseRoleClaim.Value;
                if (choseRole == "Employer")
                {
                    return RedirectToAction("NewApplicantPage", "Applicants");
                }
                else if (choseRole == "Applicant")
                {
                    return RedirectToAction("NewEmployerPage", "Employers");
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
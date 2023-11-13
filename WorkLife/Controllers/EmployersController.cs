using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkLife.Areas.Identity.Data;
using WorkLife.Data;
using WorkLife.Models;
using WorkLife.Models.ViewModel;
using WorkLife.Models.WorkLifeLogicLayer;

namespace WorkLife.Controllers
{
    public class EmployersController : Controller
    {
        private readonly WorkLifeLogicLayer _workLifeLogicLayer;
        public EmployersController(IRepository<Applicant> applicantRepository, IRepository<Employer> employerRepository, IRepository<WorkLifeUser> workLifeUserRepository, IRepository<Country> countryRepository, IRepository<Job> jobRepository, IRepository<IndustryArea> industryAreaRepository, IRepository<Application> applicationRepository, IRepository<ApplicantIndustryArea> applicantIndustryAreaRepository, IRepository<EmployerIndustryArea> employerIndustryAreaRepository, IRepository<JobIndustryArea> jobIndustryAreaRepository, RoleManager<IdentityRole> roleManager, UserManager<WorkLifeUser> userManager, SignInManager<WorkLifeUser> signInManager)
        {
            _workLifeLogicLayer = new WorkLifeLogicLayer(applicantRepository, employerRepository, workLifeUserRepository, countryRepository, jobRepository, industryAreaRepository, applicationRepository, applicantIndustryAreaRepository, employerIndustryAreaRepository, jobIndustryAreaRepository, roleManager, userManager, signInManager);
        }

        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Index()
        {
            WorkLifeUser workLifeUser = await _workLifeLogicLayer.GetWorkLifeUserByEmail(User.Identity.Name);
            workLifeUser = _workLifeLogicLayer.GetUpdateWorkLifeUser(workLifeUser);
            List<Job> jobs = _workLifeLogicLayer.GetJobsByEmployerId(User.Identity.Name).ToList();
            EmployerJobsViewModel employerJobsViewModel = new EmployerJobsViewModel
            {
                WorkLifeUser = workLifeUser,
                Jobs = jobs
            };
            return View(employerJobsViewModel);
        }

        [Authorize(Roles = "Employer")]
        public IActionResult NewEmployerPage()
        {
            WorkLifeUser workLifeUser = _workLifeLogicLayer.GetUsers().First();
            ViewBag.UserId = workLifeUser.Id;
            ViewBag.Countries = new SelectList(_workLifeLogicLayer.GetCountries(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult NewEmployerPage(string industryAreasInput, [Bind("CompanyName,Description,Region")] Employer employer)
        {
            ViewBag.Countries = new SelectList(_workLifeLogicLayer.GetCountries(), "Id", "Name");
            if (string.IsNullOrWhiteSpace(industryAreasInput))
            {
                ModelState.AddModelError("industryAreasInput", "Industry Areas is required.");
                return View(employer);
            }
            if (ModelState.IsValid)
            {
                _workLifeLogicLayer.CreateNewEmployer(employer);
                int employerId = employer.Id;
                List<IndustryArea> employerIndustryAreas = _workLifeLogicLayer.SeprateIndustryAreas(industryAreasInput);
                _workLifeLogicLayer.UpdateEmployerIndustryAreas(employerId);
                _workLifeLogicLayer.CreateNewEmployerIndustryAreas(employerId, employerIndustryAreas);
                _workLifeLogicLayer.UpdateNewEmployerIndustryAreas();
                WorkLifeUser workLifeUser = _workLifeLogicLayer.GetWorkLifeUser();
                _workLifeLogicLayer.UpdateNewEmployer(employer, workLifeUser);
                return RedirectToAction("Index", "Home");
            }
            return View(employer);
        }

        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Profile()
        {
            WorkLifeUser workLifeUser = await _workLifeLogicLayer.GetWorkLifeUserByEmail(User.Identity.Name);
            workLifeUser = _workLifeLogicLayer.GetUpdateWorkLifeUser(workLifeUser);
            Employer employer = workLifeUser.Employer;
            if (employer == null)
            {
                return RedirectToAction("NewEmployerPage");
            }
            ViewBag.Countries = new SelectList(_workLifeLogicLayer.GetCountries(), "Id", "Name");
            return View(employer);
        }

        [HttpPost]
        public IActionResult Profile(string industryAreasInput, Employer employer)
        {
            ViewBag.Countries = new SelectList(_workLifeLogicLayer.GetCountries(), "Id", "Name");
            if (string.IsNullOrWhiteSpace(industryAreasInput))
            {
                ModelState.AddModelError("industryAreasInput", "Industry Areas is required.");
                return View(employer);
            }
            if (ModelState.IsValid)
            {
                _workLifeLogicLayer.UpdateCurrentEmployer(employer);
                int employerId = employer.Id;
                List<IndustryArea> employerIndustryAreas = _workLifeLogicLayer.SeprateIndustryAreas(industryAreasInput);
                _workLifeLogicLayer.UpdateEmployerIndustryAreas(employerId);
                _workLifeLogicLayer.CreateNewEmployerIndustryAreas(employerId, employerIndustryAreas);
                _workLifeLogicLayer.UpdateNewEmployerIndustryAreas();
                return RedirectToAction("Index");
            }
            return View(employer);
        }
    }
}

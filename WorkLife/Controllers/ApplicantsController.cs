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
using WorkLife.Models.WorkLifeLogicLayer;

namespace WorkLife.Controllers
{
    public class ApplicantsController : Controller
    {
        private readonly WorkLifeLogicLayer _workLifeLogicLayer;
        public ApplicantsController(IRepository<Applicant> applicantRepository, IRepository<Employer> employerRepository, IRepository<WorkLifeUser> workLifeUserRepository, IRepository<Country> countryRepository, IRepository<Job> jobRepository, IRepository<IndustryArea> industryAreaRepository, IRepository<ApplicantIndustryArea> applicantIndustryAreaRepository, IRepository<EmployerIndustryArea> employerIndustryAreaRepository, IRepository<JobIndustryArea> jobIndustryAreaRepository, RoleManager<IdentityRole> roleManager, UserManager<WorkLifeUser> userManager, SignInManager<WorkLifeUser> signInManager)
        {
            _workLifeLogicLayer = new WorkLifeLogicLayer(applicantRepository, employerRepository, workLifeUserRepository, countryRepository, jobRepository, industryAreaRepository, applicantIndustryAreaRepository, employerIndustryAreaRepository, jobIndustryAreaRepository, roleManager, userManager, signInManager);
        }

        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> Index()
        {
            WorkLifeUser workLifeUser = await _workLifeLogicLayer.GetWorkLifeUserByEmail(User.Identity.Name);
            workLifeUser = _workLifeLogicLayer.GetUpdateWorkLifeUser(workLifeUser);
            return View(workLifeUser);
        }

        [Authorize(Roles = "Applicant")]
        public IActionResult NewApplicantPage()
        {
            WorkLifeUser workLifeUser = _workLifeLogicLayer.GetUsers().First();
            ViewBag.UserId = workLifeUser.Id;
            ViewBag.Countries = new SelectList(_workLifeLogicLayer.GetCountries(), "Id", "Name");
            return View();
        }
        
        [HttpPost]
        public IActionResult NewApplicantPage(string industryAreasInput, [Bind("PersonName,FamilyName,DateOfBirth,Region")] Applicant applicant)
        {
            ViewBag.Countries = new SelectList(_workLifeLogicLayer.GetCountries(), "Id", "Name");
            if (string.IsNullOrWhiteSpace(industryAreasInput))
            {
                ModelState.AddModelError("industryAreasInput", "Industry Areas is required.");
                return View(applicant);
            }
            if (ModelState.IsValid)
            {
                _workLifeLogicLayer.CreateNewApplicant(applicant);
                int applicantId = applicant.Id;
                List<IndustryArea> applicantIndustryAreas = _workLifeLogicLayer.SeprateIndustryAreas(industryAreasInput);
                _workLifeLogicLayer.UpdateApplicantIndustryAreas(applicantId);
                _workLifeLogicLayer.CreateNewApplicantIndustryAreas(applicantId, applicantIndustryAreas);
                _workLifeLogicLayer.UpdateNewApplicantIndustryAreas();
                WorkLifeUser workLifeUser = _workLifeLogicLayer.GetWorkLifeUser();
                _workLifeLogicLayer.UpdateNewApplicant(applicant, workLifeUser);
                return RedirectToAction("Index", "Home");
            }
            return View(applicant);
        }

        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> EditApplicantProfile()
        {
            WorkLifeUser workLifeUser = await _workLifeLogicLayer.GetWorkLifeUserByEmail(User.Identity.Name);
            workLifeUser = _workLifeLogicLayer.GetUpdateWorkLifeUser(workLifeUser);
            Applicant applicant = workLifeUser.Applicant;
            if (applicant == null)
            {
                return RedirectToAction("NewApplicantPage");
            }
            ViewBag.Countries = new SelectList(_workLifeLogicLayer.GetCountries(), "Id", "Name");
            return View(applicant);
        }

        [HttpPost]
        public IActionResult EditApplicantProfile(string industryAreasInput, Applicant applicant)
        {
            ViewBag.Countries = new SelectList(_workLifeLogicLayer.GetCountries(), "Id", "Name");
            if (string.IsNullOrWhiteSpace(industryAreasInput))
            {
                ModelState.AddModelError("industryAreasInput", "Industry Areas is required.");
                return View(applicant);
            }
            if (ModelState.IsValid)
            {
                _workLifeLogicLayer.UpdateCurrentApplicant(applicant);
                int applicantId = applicant.Id;
                List<IndustryArea> applicantIndustryAreas = _workLifeLogicLayer.SeprateIndustryAreas(industryAreasInput);
                _workLifeLogicLayer.UpdateApplicantIndustryAreas(applicantId);
                _workLifeLogicLayer.CreateNewApplicantIndustryAreas(applicantId, applicantIndustryAreas);
                _workLifeLogicLayer.UpdateNewApplicantIndustryAreas();
                return RedirectToAction("Index");
            }
            return View(applicant);
        }        
    }
}

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
    public class JobsController : Controller
    {
        private readonly WorkLifeLogicLayer _workLifeLogicLayer;
        public JobsController(IRepository<Applicant> applicantRepository, IRepository<Employer> employerRepository, IRepository<WorkLifeUser> workLifeUserRepository, IRepository<Country> countryRepository, IRepository<Job> jobRepository, IRepository<IndustryArea> industryAreaRepository, IRepository<ApplicantIndustryArea> applicantIndustryAreaRepository, IRepository<EmployerIndustryArea> employerIndustryAreaRepository, IRepository<JobIndustryArea> jobIndustryAreaRepository, RoleManager<IdentityRole> roleManager, UserManager<WorkLifeUser> userManager, SignInManager<WorkLifeUser> signInManager)
        {
            _workLifeLogicLayer = new WorkLifeLogicLayer(applicantRepository, employerRepository, workLifeUserRepository, countryRepository, jobRepository, industryAreaRepository, applicantIndustryAreaRepository, employerIndustryAreaRepository, jobIndustryAreaRepository, roleManager, userManager, signInManager);
        }

        public IActionResult Index()
        {
            List<Job> jobs = _workLifeLogicLayer.GetJobs().ToList();
            return View(jobs);
        }

        [Authorize(Roles = "Employer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string industryAreasInput, [Bind("Title,Duration,Competition,Location,Description,StartDate,SubmissionDeadline")] Job job)
        {
            if (string.IsNullOrWhiteSpace(industryAreasInput))
            {
                ModelState.AddModelError("industryAreasInput", "Industry Areas is required.");
                return View(job);
            }
            WorkLifeUser workLifeUser = await _workLifeLogicLayer.GetWorkLifeUserByEmail(User.Identity.Name);
            workLifeUser = _workLifeLogicLayer.GetUpdateWorkLifeUser(workLifeUser);
            if (workLifeUser.EmployerId != null && workLifeUser.Employer != null)
            {
                job.EmployerEmail = workLifeUser.Email;
            }
            if (ModelState.IsValid)
            {
                _workLifeLogicLayer.CreateNewJob(job);
                int jobId = job.Id;
                List<IndustryArea> jobIndustryAreas = _workLifeLogicLayer.SeprateIndustryAreas(industryAreasInput);
                _workLifeLogicLayer.UpdateJobIndustryAreas(jobId);
                _workLifeLogicLayer.CreateNewJobIndustryAreas(jobId, jobIndustryAreas);
                _workLifeLogicLayer.UpdateNewJobIndustryAreas();
                return RedirectToAction("Index", "Home");
            }
            return View(job);
        }

        [Authorize(Roles = "Employer")]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Job job = _workLifeLogicLayer.GetJobById(id);
            return View(job);
        }

        [HttpPost]
        public IActionResult Edit(string industryAreasInput, Job job)
        {
            if (string.IsNullOrWhiteSpace(industryAreasInput))
            {
                ModelState.AddModelError("industryAreasInput", "Industry Areas is required.");
                return View(job);
            }
            if (ModelState.IsValid)
            {
                _workLifeLogicLayer.UpdateCurrentJob(job);
                int jobId = job.Id;
                List<IndustryArea> jobIndustryAreas = _workLifeLogicLayer.SeprateIndustryAreas(industryAreasInput);
                _workLifeLogicLayer.UpdateJobIndustryAreas(jobId);
                _workLifeLogicLayer.CreateNewJobIndustryAreas(jobId, jobIndustryAreas);
                _workLifeLogicLayer.UpdateNewJobIndustryAreas();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        [Authorize(Roles = "Employer")]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Job job = _workLifeLogicLayer.GetJobById(id);
            return View(job);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            Job job = _workLifeLogicLayer.GetJobById(id);
            _workLifeLogicLayer.DeleteJobById(id);
            return RedirectToAction("Index");
        }
    }
}

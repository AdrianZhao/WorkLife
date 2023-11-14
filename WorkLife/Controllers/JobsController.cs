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
    public class JobsController : Controller
    {
        private readonly WorkLifeLogicLayer _workLifeLogicLayer;
        public JobsController(IRepository<Applicant> applicantRepository, IRepository<Employer> employerRepository, IRepository<WorkLifeUser> workLifeUserRepository, IRepository<Country> countryRepository, IRepository<Job> jobRepository, IRepository<IndustryArea> industryAreaRepository, IRepository<Application> applicationRepository, IRepository<ApplicantIndustryArea> applicantIndustryAreaRepository, IRepository<EmployerIndustryArea> employerIndustryAreaRepository, IRepository<JobIndustryArea> jobIndustryAreaRepository, RoleManager<IdentityRole> roleManager, UserManager<WorkLifeUser> userManager, SignInManager<WorkLifeUser> signInManager)
        {
            _workLifeLogicLayer = new WorkLifeLogicLayer(applicantRepository, employerRepository, workLifeUserRepository, countryRepository, jobRepository, industryAreaRepository, applicationRepository, applicantIndustryAreaRepository, employerIndustryAreaRepository, jobIndustryAreaRepository, roleManager, userManager, signInManager);
        }

        public async Task<IActionResult> Index()
        {
            WorkLifeUser workLifeUser = await _workLifeLogicLayer.GetWorkLifeUserByEmail(User.Identity.Name);
            workLifeUser = _workLifeLogicLayer.GetUpdateWorkLifeUser(workLifeUser);
            List<ApplicantJobsViewModel> jobs = _workLifeLogicLayer.GetJobs(workLifeUser.ApplicantId).ToList();
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
        public async Task<IActionResult> Edit(string industryAreasInput, Job job)
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
            _workLifeLogicLayer.DeleteJobById(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Applicant")]
        public IActionResult Apply(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            ViewBag.JobId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Apply([Bind("JobId,Resume,ReferenceOneName,ReferenceOneEmail,ReferenceOnePhoneNumber,ReferenceTwoName,ReferenceTwoEmail,ReferenceTwoPhoneNumber,ReferenceThreeName,ReferenceThreeEmail,ReferenceThreePhoneNumber,ReferenceFourName,ReferenceFourEmail,ReferenceFourPhoneNumber,ReferenceFiveName,ReferenceFiveEmail,ReferenceFivePhoneNumber")] Application application)
        {
            WorkLifeUser workLifeUser = await _workLifeLogicLayer.GetWorkLifeUserByEmail(User.Identity.Name);
            workLifeUser = _workLifeLogicLayer.GetUpdateWorkLifeUser(workLifeUser);
            application.Job = _workLifeLogicLayer.GetJobById(application.JobId);
            if (workLifeUser.ApplicantId != null && workLifeUser.Applicant != null)
            {
                application.ApplicantEmail = workLifeUser.Email;
                application.Applicant = workLifeUser.Applicant;
            }
            if (ModelState.IsValid)
            {
                _workLifeLogicLayer.CreateNewApplcation(application);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.JobId = application.JobId;
            return View(application);
        }

        [Authorize(Roles = "Employer")]
        public IActionResult Applications(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            List<Application> applications = _workLifeLogicLayer.GetApplicationsByJobId(id);
            return View(applications);
        }

        public IActionResult ApplicationDetail(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Application application = _workLifeLogicLayer.GetApplicationByApplicationId(id);
            return View(application);
        }

        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> MyApplications()
        {
            WorkLifeUser workLifeUser = await _workLifeLogicLayer.GetWorkLifeUserByEmail(User.Identity.Name);
            workLifeUser = _workLifeLogicLayer.GetUpdateWorkLifeUser(workLifeUser);
            List<Application> applications = _workLifeLogicLayer.GetApplicationByApplicantId(workLifeUser.Applicant.Id);
            return View(applications);
        }

        [Authorize(Roles = "Applicant")]
        public IActionResult Withdraw(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Application application = _workLifeLogicLayer.GetApplicationByApplicationId(id);
            return View(application);
        }

        [HttpPost]
        public IActionResult WithdrawConfirmed(int id)
        {
            _workLifeLogicLayer.DeleteApplicationById(id);
            return RedirectToAction("MyApplications");
        }

        [Authorize(Roles = "Applicant")]
        public IActionResult EditApplication(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Application application = _workLifeLogicLayer.GetApplicationByApplicationId(id);
            ViewBag.JobId = application.JobId;
            return View(application);
        }

        [HttpPost]
        public IActionResult EditApplication(Application application)
        {            
            if (ModelState.IsValid)
            {
                Application result = _workLifeLogicLayer.GetApplicationByApplicationId(application.Id);
                _workLifeLogicLayer.UpdateCurrentApplication(result ,application);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.JobId = application.JobId;
            return View(application);
        }
    }
}

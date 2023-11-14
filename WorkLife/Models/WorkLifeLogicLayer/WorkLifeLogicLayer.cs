using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Collections.Generic;
using WorkLife.Areas.Identity.Data;
using WorkLife.Data;
using WorkLife.Models.ViewModel;

namespace WorkLife.Models.WorkLifeLogicLayer
{
    public class WorkLifeLogicLayer
    {
        private readonly IRepository<Applicant> _applicantRepository;
        private readonly IRepository<Employer> _employerRepository;
        private readonly IRepository<WorkLifeUser> _workLifeUserRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<Job> _jobRepository;
        private readonly IRepository<IndustryArea> _industryAreaRepository;
        private readonly IRepository<Application> _applicationRepository;
        private readonly IRepository<ApplicantIndustryArea> _applicantIndustryAreaRepository;
        private readonly IRepository<EmployerIndustryArea> _employerIndustryAreaRepository;
        private readonly IRepository<JobIndustryArea> _jobIndustryAreaRepository;
        private readonly UserManager<WorkLifeUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<WorkLifeUser> _signInManager;
        public WorkLifeLogicLayer(IRepository<Applicant> applicantRepository, IRepository<Employer> employerRepository, IRepository<WorkLifeUser> workLifeUserRepository, IRepository<Country> countryRepository, IRepository<Job> jobRepository, IRepository<IndustryArea> industryAreaRepository, IRepository<Application> applicationRepository, IRepository<ApplicantIndustryArea> applicantIndustryAreaRepository, IRepository<EmployerIndustryArea> employerIndustryAreaRepository, IRepository<JobIndustryArea> jobIndustryAreaRepository, RoleManager<IdentityRole> roleManager, UserManager<WorkLifeUser> userManager, SignInManager<WorkLifeUser> signInManager)
        {
            _applicantRepository = applicantRepository;
            _employerRepository = employerRepository;
            _workLifeUserRepository = workLifeUserRepository;
            _countryRepository = countryRepository;
            _jobRepository = jobRepository;
            _industryAreaRepository = industryAreaRepository;
            _applicationRepository = applicationRepository;     
            _applicantIndustryAreaRepository = applicantIndustryAreaRepository;
            _employerIndustryAreaRepository = employerIndustryAreaRepository;
            _jobIndustryAreaRepository = jobIndustryAreaRepository;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public List<Country> GetCountries()
        {
            return _countryRepository.GetAll().ToList();
        }

        public List<IndustryArea> SeprateIndustryAreas(string industryAreas)
        {
            IEnumerable<string> industryAreaStrings = industryAreas.Split(',').Select(area => area.Trim());
            List<IndustryArea> currentIndustryAreas = _industryAreaRepository.GetAll().ToList();
            List<IndustryArea> thisIndustryAreas = new List<IndustryArea>();
            foreach (string areaTitle in industryAreaStrings)
            {
                IndustryArea area = currentIndustryAreas.FirstOrDefault(a => a.Title == areaTitle);
                if (area == null)
                {
                    area = new IndustryArea
                    {
                        Title = areaTitle,
                        ApplicantsIndustryAreas = new List<ApplicantIndustryArea>(),
                        EmployersIndustryAreas = new List<EmployerIndustryArea>()
                    };
                    _industryAreaRepository.Create(area);
                }
                thisIndustryAreas.Add(area);
            }
            return thisIndustryAreas;
        }

        public void UpdateEmployerIndustryAreas(int employerId)
        {
            if (employerId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            List<EmployerIndustryArea> employerIndustryAreasToDelete = _employerIndustryAreaRepository.GetAll().Where(eia => eia.EmployerId == employerId).ToList();
            foreach (var employerIndustryAreaToDelete in employerIndustryAreasToDelete)
            {
                _employerIndustryAreaRepository.Delete(employerIndustryAreaToDelete);
            }
        }

        public void UpdateApplicantIndustryAreas(int applicantId)
        {
            if (applicantId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            List<ApplicantIndustryArea> applicantIndustryAreasToDelete = _applicantIndustryAreaRepository.GetAll().Where(aia => aia.ApplicantId == applicantId).ToList();
            foreach (var applicantIndustryAreaToDelete in applicantIndustryAreasToDelete)
            {
                _applicantIndustryAreaRepository.Delete(applicantIndustryAreaToDelete);
            }
        }

        public void CreateNewEmployerIndustryAreas(int employerId, List<IndustryArea> industryAreas)
        {
            if (employerId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            foreach (IndustryArea industryArea in industryAreas)
            {
                EmployerIndustryArea employerIndustryArea = new EmployerIndustryArea
                {
                    EmployerId = employerId,
                    IndustryArea = industryArea,
                    IndustryAreaId = industryArea.Id
                };
                _employerIndustryAreaRepository.Create(employerIndustryArea);
            }
        }

        public void CreateNewApplicantIndustryAreas(int applicantId, List<IndustryArea> industryAreas)
        {
            if (applicantId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            foreach (IndustryArea industryArea in industryAreas)
            {
                ApplicantIndustryArea applicantIndustryArea = new ApplicantIndustryArea
                {
                    ApplicantId = applicantId,
                    IndustryArea = industryArea,
                    IndustryAreaId = industryArea.Id
                };
                _applicantIndustryAreaRepository.Create(applicantIndustryArea);
            }
        }

        public void UpdateNewEmployerIndustryAreas()
        {
            List<EmployerIndustryArea> employerIndustryAreas = _employerIndustryAreaRepository.GetAll().ToList();
            List<Employer> employers = _employerRepository.GetAll().ToList();
            foreach (Employer employer in employers)
            {
                foreach (EmployerIndustryArea industryArea in employerIndustryAreas)
                {
                    if (industryArea.EmployerId == employer.Id)
                    {
                        industryArea.Employer = employer;
                    }
                    _employerIndustryAreaRepository.Update(industryArea);
                }
            }
        }

        public void UpdateNewApplicantIndustryAreas()
        {
            List<ApplicantIndustryArea> applicantIndustryAreas = _applicantIndustryAreaRepository.GetAll().ToList();
            List<Applicant> applicants = _applicantRepository.GetAll().ToList();
            foreach (Applicant applicant in applicants)
            {
                foreach (ApplicantIndustryArea industryArea in applicantIndustryAreas)
                {
                    if (industryArea.ApplicantId == applicant.Id)
                    {
                        industryArea.Applicant = applicant;
                    }
                    _applicantIndustryAreaRepository.Update(industryArea);
                }
            }
        }

        public List<WorkLifeUser> GetUsers()
        {
            return _workLifeUserRepository.GetAll().ToList();
        }

        public WorkLifeUser GetWorkLifeUser()
        {
            WorkLifeUser loggedIn = _workLifeUserRepository.GetAll().Where(u => u.ApplicantId == null && u.EmployerId == null).FirstOrDefault();
            return loggedIn;
        }

        public void CreateNewApplicant(Applicant applicant)
        {
            _applicantRepository.Create(applicant);
        }

        public void UpdateNewApplicant(Applicant applicant, WorkLifeUser workLifeUser)
        {
            _applicantRepository.Update(applicant);
            workLifeUser.ApplicantId = applicant.Id;
            workLifeUser.Applicant = applicant;
            _workLifeUserRepository.Update(workLifeUser);
        }

        public void CreateNewEmployer(Employer employer)
        {
            _employerRepository.Create(employer);
        }

        public void UpdateNewEmployer(Employer employer, WorkLifeUser workLifeUser)
        {
            _employerRepository.Update(employer);
            workLifeUser.EmployerId = employer.Id;
            workLifeUser.Employer = employer;
            _workLifeUserRepository.Update(workLifeUser);
        }

        public WorkLifeUser GetCurrentLoggedEmployerUser(int employerId)
        {
            if (employerId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            WorkLifeUser workLifeUser = _workLifeUserRepository.GetAll().FirstOrDefault(u => u.EmployerId == employerId);
            return workLifeUser;
        }

        public WorkLifeUser GetCurrentLoggedApplicantUser(int applicantId)
        {
            if (applicantId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            WorkLifeUser workLifeUser = _workLifeUserRepository.GetAll().FirstOrDefault(u => u.ApplicantId == applicantId);
            return workLifeUser;
        }

        public void UpdateCurrentEmployer(Employer employer)
        {         
            _employerRepository.Update(employer);
            WorkLifeUser workLifeUser = _workLifeUserRepository.GetAll().FirstOrDefault(u => u.EmployerId == employer.Id);
            workLifeUser.Employer = employer;
            _workLifeUserRepository.Update(workLifeUser);
        }

        public void UpdateCurrentApplicant(Applicant applicant)
        {
            _applicantRepository.Update(applicant);
            WorkLifeUser workLifeUser = _workLifeUserRepository.GetAll().FirstOrDefault(u => u.ApplicantId == applicant.Id);
            workLifeUser.Applicant = applicant;
            _workLifeUserRepository.Update(workLifeUser);
        }

        public void UpdateCurrentJob(Job job)
        {
            _jobRepository.Update(job);
        }

        public async Task<WorkLifeUser> GetWorkLifeUserByEmail(string email)
        {
            WorkLifeUser workLifeUser = await _userManager.FindByEmailAsync(email);
            return workLifeUser;
        }

        public WorkLifeUser GetUpdateWorkLifeUser(WorkLifeUser workLifeUser)
        {
            workLifeUser = _workLifeUserRepository.GetAll().FirstOrDefault(u => u == workLifeUser);
            return workLifeUser;
        }

        public List<ApplicantJobsViewModel> GetJobs(int? applicantId)
        {
            List<Job> jobs = _jobRepository.GetAll().ToList();
            List<ApplicantJobsViewModel> viewModelList = new List<ApplicantJobsViewModel>();
            if (applicantId != null)
            {
                foreach (Job job in jobs)
                {
                    ApplicantJobsViewModel applicantJobsViewModel = new ApplicantJobsViewModel { Job = job };
                    foreach (Application application in job.Applications)
                    {
                        if (application.Applicant.Id == applicantId)
                        {
                            applicantJobsViewModel.IsApplied = true;
                            break;
                        }
                    }
                    viewModelList.Add(applicantJobsViewModel);
                }
            }
            else
            {
                foreach (Job job in jobs)
                {
                    ApplicantJobsViewModel applicantJobsViewModel = new ApplicantJobsViewModel { Job = job };
                    viewModelList.Add(applicantJobsViewModel);
                }
            }
            return viewModelList;
        }

        public List<Job> GetJobsByEmployerId(string employerEmail)
        {
            if (string.IsNullOrEmpty(employerEmail))
            {
                throw new ArgumentOutOfRangeException();
            }
            List<Job> jobs = _jobRepository.GetAll().Where(j => j.EmployerEmail == employerEmail).ToList();
            return jobs;
        }

        public void CreateNewJob(Job job)
        {
            _jobRepository.Create(job);
        }

        public void UpdateJobIndustryAreas(int jobId)
        {
            if (jobId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            List<JobIndustryArea> jobIndustryAreasToDelete = _jobIndustryAreaRepository.GetAll().Where(jia => jia.JobId == jobId).ToList();
            foreach (var jobIndustryAreaToDelete in jobIndustryAreasToDelete)
            {
                _jobIndustryAreaRepository.Delete(jobIndustryAreaToDelete);
            }
        }

        public void CreateNewJobIndustryAreas(int jobId, List<IndustryArea> industryAreas)
        {
            if (jobId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            foreach (IndustryArea industryArea in industryAreas)
            {
                JobIndustryArea jobIndustryArea = new JobIndustryArea
                {
                    JobId = jobId,
                    IndustryArea = industryArea,
                    IndustryAreaId = industryArea.Id
                };
                _jobIndustryAreaRepository.Create(jobIndustryArea);
            }
        }

        public void UpdateNewJobIndustryAreas()
        {
            List<JobIndustryArea> jobIndustryAreas = _jobIndustryAreaRepository.GetAll().ToList();
            List<Job> jobs = _jobRepository.GetAll().ToList();
            foreach (Job job in jobs)
            {
                foreach (JobIndustryArea industryArea in jobIndustryAreas)
                {
                    if (industryArea.JobId == job.Id)
                    {
                        industryArea.Job = job;
                    }
                    _jobIndustryAreaRepository.Update(industryArea);
                }
            }
        }

        public Job GetJobById(int jobId)
        {
            if (jobId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            return _jobRepository.GetAll().FirstOrDefault(j => j.Id == jobId);
        }

        public void DeleteJobById(int jobId)
        {
            if (jobId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            Job job = _jobRepository.GetAll().FirstOrDefault(i => i.Id == jobId);
            if (job.Applications.Count() > 0)
            {
                List<Application> applications = _applicationRepository.GetAll().Where(a => a.JobId == job.Id).ToList();
                foreach (Application application in applications)
                {
                    _applicationRepository.Delete(application);
                }
            }
            _jobRepository.Delete(job);
        }

        public void CreateNewApplcation(Application application)
        {
            _applicationRepository.Create(application);
        }

        public List<Application> GetApplicationsByJobId(int jobId) 
        {
            if (jobId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            List<Application> applications = _applicationRepository.GetAll().Where(a => a.JobId == jobId).ToList();
            return applications;
        }

        public Application GetApplicationByApplicationId(int applicationId)
        {
            if (applicationId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            return _applicationRepository.GetAll().Where(a => a.Id == applicationId).FirstOrDefault();
        }

        public List<Application> GetApplicationByApplicantId(int applicantId)
        {
            if (applicantId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            List<Application> applications = _applicationRepository.GetAll().Where(a => a.Applicant.Id == applicantId).ToList();
            return applications;
        }

        public void DeleteApplicationById(int applicationId)
        {
            if (applicationId == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            Application application = _applicationRepository.GetAll().Where(a => a.Id == applicationId).FirstOrDefault();
            _applicationRepository.Delete(application);
        }

        public void UpdateCurrentApplication(Application result, Application application)
        {
            result.Resume = application.Resume;
            result.ReferenceOneName = application.ReferenceOneName;
            result.ReferenceOneEmail = application.ReferenceOneEmail;
            result.ReferenceOnePhoneNumber = application.ReferenceOnePhoneNumber;
            result.ReferenceTwoName = application.ReferenceTwoName;
            result.ReferenceTwoEmail = application.ReferenceTwoEmail;
            result.ReferenceTwoPhoneNumber = application.ReferenceTwoPhoneNumber;
            result.ReferenceThreeName = application.ReferenceThreeName;
            result.ReferenceThreeEmail = application.ReferenceThreeEmail;
            result.ReferenceThreePhoneNumber = application.ReferenceThreePhoneNumber;
            result.ReferenceFourName = application.ReferenceFourName;
            result.ReferenceFourEmail = application.ReferenceFourEmail;
            result.ReferenceFourPhoneNumber = application.ReferenceFourPhoneNumber;
            result.ReferenceFiveName = application.ReferenceFiveName;
            result.ReferenceFiveEmail = application.ReferenceFiveEmail;
            result.ReferenceFivePhoneNumber = application.ReferenceFivePhoneNumber;
            _applicationRepository.Update(result);
        }
    }
}

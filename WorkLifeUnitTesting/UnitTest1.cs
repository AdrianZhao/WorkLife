using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Moq;
using WorkLife.Areas.Identity.Data;
using WorkLife.Data;
using WorkLife.Models;
using WorkLife.Models.WorkLifeLogicLayer;

namespace WorkLifeUnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        private Mock<IRepository<Applicant>> _applicantRepo = new Mock<IRepository<Applicant>>();
        private Mock<IRepository<Employer>> _employerRepo = new Mock<IRepository<Employer>>();
        private Mock<IRepository<WorkLifeUser>> _workLifeUserRepo = new Mock<IRepository<WorkLifeUser>>();
        private Mock<IRepository<Country>> _countryRepo = new Mock<IRepository<Country>>();
        private Mock<IRepository<Job>> _jobRepo = new Mock<IRepository<Job>>();
        private Mock<IRepository<IndustryArea>> _industryAreaRepo = new Mock<IRepository<IndustryArea>>();
        private Mock<IRepository<ApplicantIndustryArea>> _applicantIndustryAreaRepo = new Mock<IRepository<ApplicantIndustryArea>>();
        private Mock<IRepository<EmployerIndustryArea>> _employerIndustryAreaRepo = new Mock<IRepository<EmployerIndustryArea>>();
        private Mock<IRepository<JobIndustryArea>> _jobIndustryAreaRepo = new Mock<IRepository<JobIndustryArea>>();
        private UserManager<WorkLifeUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<WorkLifeUser> _signInManager;
        public WorkLifeLogicLayer InitializeBLL()
        {
            return new WorkLifeLogicLayer(_applicantRepo.Object, _employerRepo.Object, _workLifeUserRepo.Object, _countryRepo.Object, _jobRepo.Object, _industryAreaRepo.Object, _applicantIndustryAreaRepo.Object, _employerIndustryAreaRepo.Object, _jobIndustryAreaRepo.Object, _roleManager, _userManager, _signInManager);
        }

        [TestMethod]
        public void GetCountries_ReturnsCountries()
        {
            // Arrange
            List<Country> countries = new List<Country>
            {
                new Country { Id = 1, Name = "Country1" },
                new Country { Id = 2, Name = "Country2" }
            };
            _countryRepo.Setup(repo => repo.GetAll()).Returns(countries);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act
            List<Country> result = workLifeLogicLayer.GetCountries().ToList();
            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void SeprateIndustryAreas_ReturnsIndustryAreas()
        {
            // Arrange
            string industryAreas = "Area1, Area2, Area3";
            IEnumerable<string> industryAreaStrings = industryAreas.Split(',').Select(area => area.Trim());
            List<IndustryArea> existingIndustryAreas = new List<IndustryArea>
            {
                new IndustryArea { Id = 1, Title = "Area1" },
                new IndustryArea { Id = 2, Title = "Area2" }
            };
            _industryAreaRepo.Setup(repo => repo.GetAll()).Returns(existingIndustryAreas);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act
            List<IndustryArea> result = workLifeLogicLayer.SeprateIndustryAreas(industryAreas);
            // Assert
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void SeprateIndustryAreas_CheckIfExistingIndustryAreaIsInDatabase()
        {
            // Arrange
            string industryAreas = "Area1, Area2, Area3";
            IEnumerable<string> industryAreaStrings = industryAreas.Split(',').Select(area => area.Trim());
            List<IndustryArea> existingIndustryAreas = new List<IndustryArea>
            {
                new IndustryArea { Id = 1, Title = "Area1" },
                new IndustryArea { Id = 2, Title = "Area2" }
            };
            _industryAreaRepo.Setup(repo => repo.GetAll()).Returns(existingIndustryAreas);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act
            List<IndustryArea> result = workLifeLogicLayer.SeprateIndustryAreas(industryAreas);
            // Assert
            Assert.IsTrue(result.Any(area => area.Title == "Area1"));
            Assert.IsTrue(result.Any(area => area.Title == "Area2"));
            Assert.IsTrue(result.Any(area => area.Title == "Area3"));
        }

        [TestMethod]
        public void UpdateEmployerIndustryAreas_DeletesAssociatedAreas()
        {
            // Arrange
            int employerId = 1;
            List<EmployerIndustryArea> employerIndustryAreasToDelete = new List<EmployerIndustryArea>
            {
                new EmployerIndustryArea { Id = 1, EmployerId = employerId },
                new EmployerIndustryArea { Id = 2, EmployerId = employerId }
            };
            _employerIndustryAreaRepo.Setup(repo => repo.GetAll()).Returns(employerIndustryAreasToDelete);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act
            workLifeLogicLayer.UpdateEmployerIndustryAreas(employerId);
            // Assert
            foreach (EmployerIndustryArea employerIndustryArea in employerIndustryAreasToDelete)
            {
                _employerIndustryAreaRepo.Verify(repo => repo.Delete(employerIndustryArea));
            }
        }

        [TestMethod]
        public void UpdateEmployerIndustryAreas_InvalidIdThrowArgumentOutOfRangeException()
        {
            // Arrange
            int employerId = 0;
            List<EmployerIndustryArea> employerIndustryAreasToDelete = new List<EmployerIndustryArea>
            {
                new EmployerIndustryArea { Id = 1, EmployerId = employerId },
                new EmployerIndustryArea { Id = 2, EmployerId = employerId }
            };
            _employerIndustryAreaRepo.Setup(repo => repo.GetAll()).Returns(employerIndustryAreasToDelete);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workLifeLogicLayer.UpdateEmployerIndustryAreas(employerId));     
        }

        [TestMethod]
        public void UpdateApplicantIndustryAreas_DeletesAssociatedAreas()
        {
            // Arrange
            int applicantId = 1;
            List<ApplicantIndustryArea> applicantIndustryAreasToDelete = new List<ApplicantIndustryArea>
            {
                new ApplicantIndustryArea { Id = 1, ApplicantId = applicantId },
                new ApplicantIndustryArea { Id = 2, ApplicantId = applicantId }
            };
            _applicantIndustryAreaRepo.Setup(repo => repo.GetAll()).Returns(applicantIndustryAreasToDelete);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act
            workLifeLogicLayer.UpdateApplicantIndustryAreas(applicantId);
            // Assert
            foreach (ApplicantIndustryArea applicantIndustryArea in applicantIndustryAreasToDelete)
            {
                _applicantIndustryAreaRepo.Verify(repo => repo.Delete(applicantIndustryArea));
            }
        }

        [TestMethod]
        public void UpdateApplicantIndustryAreas_InvalidIdThrowArgumentOutOfRangeException()
        {
            // Arrange
            int applicantId = 0;
            List<ApplicantIndustryArea> applicantIndustryAreasToDelete = new List<ApplicantIndustryArea>
            {
                new ApplicantIndustryArea { Id = 1, ApplicantId = applicantId },
                new ApplicantIndustryArea { Id = 2, ApplicantId = applicantId }
            };
            _applicantIndustryAreaRepo.Setup(repo => repo.GetAll()).Returns(applicantIndustryAreasToDelete);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workLifeLogicLayer.UpdateApplicantIndustryAreas(applicantId));
        }

        [TestMethod]
        public void CreateNewEmployerIndustryAreas_CreatesEmployerIndustryAreas()
        {
            // Arrange
            int employerId = 1;
            List<IndustryArea> industryAreas = new List<IndustryArea>
            {
                new IndustryArea { Id = 1, Title = "Area1" },
                new IndustryArea { Id = 2, Title = "Area2" }
            };
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act
            workLifeLogicLayer.CreateNewEmployerIndustryAreas(employerId, industryAreas);
            // Assert
            foreach (var industryArea in industryAreas)
            {
                _employerIndustryAreaRepo.Verify(repo => repo.Create(It.Is<EmployerIndustryArea>(eia => eia.EmployerId == employerId && eia.IndustryAreaId == industryArea.Id)));
            }
        }

        [TestMethod]
        public void CreateNewEmployerIndustryAreas_InvalidIdThrowArgumentOutOfRangeException()
        {
            // Arrange
            int employerId = 0;
            List<IndustryArea> industryAreas = new List<IndustryArea>
            {
                new IndustryArea { Id = 1, Title = "Area1" },
                new IndustryArea { Id = 2, Title = "Area2" }
            };
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workLifeLogicLayer.CreateNewEmployerIndustryAreas(employerId, industryAreas));
        }

        [TestMethod]
        public void CreateNewApplicantIndustryAreas_CreatesApplicantIndustryAreas()
        {
            // Arrange
            int applicantId = 1;
            List<IndustryArea> industryAreas = new List<IndustryArea>
            {
                new IndustryArea { Id = 1, Title = "Area1" },
                new IndustryArea { Id = 2, Title = "Area2" }
            };
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act
            workLifeLogicLayer.CreateNewApplicantIndustryAreas(applicantId, industryAreas);
            // Assert
            foreach (IndustryArea industryArea in industryAreas)
            {
                _applicantIndustryAreaRepo.Verify(repo => repo.Create(It.Is<ApplicantIndustryArea>(aia => aia.ApplicantId == applicantId && aia.IndustryAreaId == industryArea.Id)), Times.Once);
            }
        }

        [TestMethod]
        public void CreateNewApplicantIndustryAreas_InvalidIdThrowArgumentOutOfRangeException()
        {
            // Arrange
            int applicantId = 0;
            List<IndustryArea> industryAreas = new List<IndustryArea>
            {
                new IndustryArea { Id = 1, Title = "Area1" },
                new IndustryArea { Id = 2, Title = "Area2" }
            };
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workLifeLogicLayer.UpdateApplicantIndustryAreas(applicantId));
        }

        [TestMethod]
        public void UpdateNewEmployerIndustryAreas_UpdatesEmployerProperty()
        {
            // Arrange
            List<EmployerIndustryArea> employerIndustryAreas = new List<EmployerIndustryArea>
            {
                new EmployerIndustryArea { Id = 1, EmployerId = 1 },
                new EmployerIndustryArea { Id = 2, EmployerId = 2 }
            };
            List<Employer> employers = new List<Employer>
            {
                new Employer { Id = 1 },
                new Employer { Id = 2 }
            };
            _employerIndustryAreaRepo.Setup(repo => repo.GetAll()).Returns(employerIndustryAreas);
            _employerRepo.Setup(repo => repo.GetAll()).Returns(employers);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act
            workLifeLogicLayer.UpdateNewEmployerIndustryAreas();
            // Assert
            foreach (EmployerIndustryArea employerIndustryArea in employerIndustryAreas)
            {
                _employerIndustryAreaRepo.Verify(repo => repo.Update(It.Is<EmployerIndustryArea>(eia => eia.EmployerId == employerIndustryArea.EmployerId && eia.Employer == employers.First(e => e.Id == employerIndustryArea.EmployerId))));
            }
        }

        [TestMethod]
        public void UpdateNewApplicantIndustryAreas_UpdatesApplicantProperty()
        {
            // Arrange
            List<ApplicantIndustryArea> applicantIndustryAreas = new List<ApplicantIndustryArea>
            {
                new ApplicantIndustryArea { Id = 1, ApplicantId = 1 },
                new ApplicantIndustryArea { Id = 2, ApplicantId = 2 }
            };
            List<Applicant> applicants = new List<Applicant>
            {
                new Applicant { Id = 1 },
                new Applicant { Id = 2 }
            };
            _applicantIndustryAreaRepo.Setup(repo => repo.GetAll()).Returns(applicantIndustryAreas);
            _applicantRepo.Setup(repo => repo.GetAll()).Returns(applicants);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act
            workLifeLogicLayer.UpdateNewApplicantIndustryAreas();
            // Assert
            foreach (ApplicantIndustryArea applicantIndustryArea in applicantIndustryAreas)
            {
                _applicantIndustryAreaRepo.Verify(repo => repo.Update(It.Is<ApplicantIndustryArea>(aia => aia.ApplicantId == applicantIndustryArea.ApplicantId && aia.Applicant == applicants.First(a => a.Id == applicantIndustryArea.ApplicantId))));
            }
        }

        [TestMethod]
        public void GetUsers_ReturnsUsers()
        {
            // Arrange
            List<WorkLifeUser> users = new List<WorkLifeUser>
            {
                new WorkLifeUser { Id = "1", UserName = "User1" },
                new WorkLifeUser { Id = "2", UserName = "User2" }
            };
            _workLifeUserRepo.Setup(repo => repo.GetAll()).Returns(users);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act
            List<WorkLifeUser> result = workLifeLogicLayer.GetUsers().ToList();
            // Assert
            Assert.AreEqual(2, result.Count); 
        }

        [TestMethod]
        public void GetWorkLifeUser_ReturnsUser()
        {
            // Arrange
            List<WorkLifeUser> users = new List<WorkLifeUser>
            {
                new WorkLifeUser { Id = "1", UserName = "User1", ApplicantId = null, EmployerId = null },
                new WorkLifeUser { Id = "2", UserName = "User2", ApplicantId = 1, EmployerId = null },
            };
            _workLifeUserRepo.Setup(repo => repo.GetAll()).Returns(users);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act
            WorkLifeUser result = workLifeLogicLayer.GetWorkLifeUser();
            // Assert
            Assert.AreEqual(result.Id, 1.ToString());
        }

        [TestMethod]
        public void CreateNewApplicant_CreatesApplicant()
        {
            // Arrange
            Applicant applicant = new Applicant
            {
                PersonName = "John",
                FamilyName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Region = "Sample Region"
            };
            _applicantRepo.Setup(repo => repo.Create(It.IsAny<Applicant>())).Verifiable();            
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.CreateNewApplicant(applicant);
            // Assert
            _applicantRepo.Verify(repo => repo.Create(applicant));
        }

        [TestMethod]
        public void CreateNewEmployer_CreatesEmployer()
        {
            // Arrange
            Employer employer = new Employer
            {
                CompanyName = "ABC Inc.",
                Description = "Sample description",
                Region = "Sample Region"
            };
            _employerRepo.Setup(repo => repo.Create(It.IsAny<Employer>())).Verifiable();
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.CreateNewEmployer(employer);
            // Assert
            _employerRepo.Verify(repo => repo.Create(employer));
        }

        [TestMethod]
        public void UpdateNewApplicant_UpdatesApplicantAndUser()
        {
            // Arrange
            Applicant applicant = new Applicant
            {
                Id = 1,
                PersonName = "John",
                FamilyName = "Doe",
            };
            WorkLifeUser workLifeUser = new WorkLifeUser
            {
                Id = 1.ToString()
            };
            _applicantRepo.Setup(repo => repo.Update(It.IsAny<Applicant>())).Verifiable();
            _workLifeUserRepo.Setup(repo => repo.Update(It.IsAny<WorkLifeUser>())).Verifiable();
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.UpdateNewApplicant(applicant, workLifeUser);
            // Assert
            _applicantRepo.Verify(repo => repo.Update(applicant));
            _workLifeUserRepo.Verify(repo => repo.Update(workLifeUser));
        }

        [TestMethod]
        public void UpdateNewEmployer_UpdatesEmployerAndUser()
        {
            // Arrange
            Employer employer = new Employer
            {
                Id = 1,
                CompanyName = "ABC Company",
            };
            WorkLifeUser workLifeUser = new WorkLifeUser
            {
                Id = 1.ToString()
            };
            _employerRepo.Setup(repo => repo.Update(It.IsAny<Employer>())).Verifiable();
            _workLifeUserRepo.Setup(repo => repo.Update(It.IsAny<WorkLifeUser>())).Verifiable();
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.UpdateNewEmployer(employer, workLifeUser);
            // Assert
            _employerRepo.Verify(repo => repo.Update(employer));
            _workLifeUserRepo.Verify(repo => repo.Update(workLifeUser));
        }

        [TestMethod]
        public void GetCurrentLoggedEmployerUser_ReturnsUser()
        {
            // Arrange
            int employerId = 1;
            List<WorkLifeUser> users = new List<WorkLifeUser>
            {
                new WorkLifeUser { Id = 1.ToString(), EmployerId = 1 },
                new WorkLifeUser { Id = 2.ToString(), EmployerId = 2 }
            };
            _workLifeUserRepo.Setup(repo => repo.GetAll()).Returns(users.AsQueryable().ToList());
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            WorkLifeUser result = workLifeLogicLayer.GetCurrentLoggedEmployerUser(employerId);
            // Assert
            Assert.AreEqual(1.ToString(), result.Id);
        }

        [TestMethod]
        public void GetCurrentLoggedEmployerUser_InvalidIdThrowArgumentOutOfRangeException()
        {
            // Arrange
            int employerId = 0;
            List<WorkLifeUser> users = new List<WorkLifeUser>
            {
                new WorkLifeUser { Id = 1.ToString(), EmployerId = 1 },
                new WorkLifeUser { Id = 2.ToString(), EmployerId = 2 }
            };
            _workLifeUserRepo.Setup(repo => repo.GetAll()).Returns(users.AsQueryable().ToList());
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workLifeLogicLayer.GetCurrentLoggedEmployerUser(employerId));
        }

        [TestMethod]
        public void GetCurrentLoggedApplicantUser_ReturnsUser()
        {
            // Arrange
            int applicantId = 1;
            List<WorkLifeUser> users = new List<WorkLifeUser>
            {
                new WorkLifeUser { Id = 1.ToString(), ApplicantId = 1 },
                new WorkLifeUser { Id = 2.ToString(), ApplicantId = 2 }
            };
            _workLifeUserRepo.Setup(repo => repo.GetAll()).Returns(users.AsQueryable().ToList());
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            WorkLifeUser result = workLifeLogicLayer.GetCurrentLoggedApplicantUser(applicantId);
            // Assert
            Assert.AreEqual(1.ToString(), result.Id);
        }

        [TestMethod]
        public void GetCurrentLoggedApplicantUser_InvalidIdThrowArgumentOutOfRangeException()
        {
            // Arrange
            int applicantId = 0;
            List<WorkLifeUser> users = new List<WorkLifeUser>
            {
                new WorkLifeUser { Id = 1.ToString(), ApplicantId = 1 },
                new WorkLifeUser { Id = 2.ToString(), ApplicantId = 2 }
            };
            _workLifeUserRepo.Setup(repo => repo.GetAll()).Returns(users.AsQueryable().ToList());
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workLifeLogicLayer.GetCurrentLoggedApplicantUser(applicantId));
        }

        [TestMethod]
        public void UpdateCurrentEmployer_UpdatesEmployerAndUser()
        {
            Employer employer = new Employer { Id = 1 }; 
            List<WorkLifeUser> users = new List<WorkLifeUser>
            {
                new WorkLifeUser { Id = 1.ToString(), EmployerId = 1 },
                new WorkLifeUser { Id = 2.ToString(), EmployerId = 2 }
            };
            _employerRepo.Setup(repo => repo.Update(employer)).Verifiable();
            _workLifeUserRepo.Setup(repo => repo.GetAll()).Returns(users.AsQueryable().ToList());
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.UpdateCurrentEmployer(employer);
            // Assert
            _employerRepo.Verify(repo => repo.Update(employer));
            _workLifeUserRepo.Verify(repo => repo.Update(It.IsAny<WorkLifeUser>()));
        }

        [TestMethod]
        public void UpdateCurrentApplicant_UpdatesApplicantAndUser()
        {
            // Arrange
            Applicant applicant = new Applicant { Id = 1 };
            List<WorkLifeUser> users = new List<WorkLifeUser>
            {
                new WorkLifeUser { Id = 1.ToString(), ApplicantId = 1 },
                new WorkLifeUser { Id = 2.ToString(), ApplicantId = 2 }
            };
            _applicantRepo.Setup(repo => repo.Update(applicant)).Verifiable();
            _workLifeUserRepo.Setup(repo => repo.GetAll()).Returns(users.AsQueryable().ToList());
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.UpdateCurrentApplicant(applicant);
            // Assert
            _applicantRepo.Verify(repo => repo.Update(applicant));
            _workLifeUserRepo.Verify(repo => repo.Update(It.IsAny<WorkLifeUser>()));
        }

        [TestMethod]
        public void UpdateCurrentJob_UpdatesJob()
        {
            // Arrange
            Job job = new Job { Id = 1 };
            _jobRepo.Setup(repo => repo.Update(job)).Verifiable();
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.UpdateCurrentJob(job);
            // Assert
            _jobRepo.Verify(repo => repo.Update(job));
        }
        /*
        [TestMethod]
        public async Task GetWorkLifeUserByEmail_ReturnsUser()
        {
            Mock<UserManager<WorkLifeUser>> _userManager = new Mock<UserManager<WorkLifeUser>>(new Mock<IUserStore<WorkLifeUser>>().Object, null, null, null, null, null, null, null, null);
            // Arrange
            string email = "test@example.com";
            WorkLifeUser workLifeUser = new WorkLifeUser { Email = email };
            _userManager.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(workLifeUser);
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            WorkLifeUser result = await workLifeLogicLayer.GetWorkLifeUserByEmail(email);
            // Assert
            Assert.AreEqual(email, result.Email);
        }
        */

        [TestMethod]
        public void GetUpdateWorkLifeUser_ReturnsUser()
        {
            WorkLifeUser workLifeUser = new WorkLifeUser { Id = 1.ToString() };
            _workLifeUserRepo.Setup(repo => repo.GetAll()).Returns(new[] { workLifeUser, new WorkLifeUser { Id = 2.ToString() } }.AsQueryable().ToList());
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            WorkLifeUser result = workLifeLogicLayer.GetUpdateWorkLifeUser(workLifeUser);
            // Assert
            Assert.AreEqual(workLifeUser.Id, result.Id);
        }

        [TestMethod]
        public void GetJobs_ReturnsJobs()
        {
            // Arrange
            List<Job> expectedJobs = new List<Job>
            {
                new Job { Id = 1, Title = "Job1" },
                new Job { Id = 2, Title = "Job2" }
            };
            _jobRepo.Setup(repo => repo.GetAll()).Returns(expectedJobs);
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            var result = workLifeLogicLayer.GetJobs();
            // Assert
            CollectionAssert.AreEqual(expectedJobs, result.ToList());
        }

        [TestMethod]
        public void GetJobsByEmployerId_ReturnsJobs()
        {
            // Arrange
            string employerEmail = "employer@example.com";
            List<Job> expectedJobs = new List<Job>
            {
                new Job { Id = 1, Title = "Job1", EmployerEmail = employerEmail },
                new Job { Id = 2, Title = "Job2", EmployerEmail = employerEmail }
            };
            _jobRepo.Setup(repo => repo.GetAll()).Returns(expectedJobs);
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            List<Job> result = workLifeLogicLayer.GetJobsByEmployerId(employerEmail).ToList();
            // Assert
            CollectionAssert.AreEqual(expectedJobs, result.ToList());
        }

        [TestMethod]
        public void CreateNewJob_CreatesJob()
        {
            // Arrange
            Job jobToCreate = new Job
            {
                Id = 1,
                Title = "Job Title"
            };
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.CreateNewJob(jobToCreate);
            // Assert
            _jobRepo.Verify(repo => repo.Create(jobToCreate));
        }

        [TestMethod]
        public void UpdateJobIndustryAreas_DeletesJobIndustryAreas()
        {
            // Arrange
            int jobId = 1;
            List<JobIndustryArea> jobIndustryAreasToDelete = new List<JobIndustryArea>
            {
                new JobIndustryArea { Id = 1, JobId = jobId },
                new JobIndustryArea { Id = 2, JobId = jobId }
            };
            _jobIndustryAreaRepo.Setup(repo => repo.GetAll()).Returns(jobIndustryAreasToDelete);
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.UpdateJobIndustryAreas(jobId);
            // Assert
            foreach (var jobIndustryAreaToDelete in jobIndustryAreasToDelete)
            {
                _jobIndustryAreaRepo.Verify(repo => repo.Delete(jobIndustryAreaToDelete));
            }
        }

        [TestMethod]
        public void UpdateJobIndustryAreas_InvalidIdThrowArgumentOutOfRangeException()
        {
            // Arrange
            int jobId = 0;
            List<JobIndustryArea> jobIndustryAreasToDelete = new List<JobIndustryArea>
            {
                new JobIndustryArea { Id = 1, JobId = jobId },
                new JobIndustryArea { Id = 2, JobId = jobId }
            };
            _jobIndustryAreaRepo.Setup(repo => repo.GetAll()).Returns(jobIndustryAreasToDelete);
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workLifeLogicLayer.UpdateJobIndustryAreas(jobId));
        }

        [TestMethod]
        public void CreateNewJobIndustryAreas_CreatesJobIndustryAreas()
        {
            // Arrange
            int jobId = 1;
            List<IndustryArea> industryAreas = new List<IndustryArea>
            {
                new IndustryArea { Id = 1, Title = "Area1" },
                new IndustryArea { Id = 2, Title = "Area2" }
            };
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.CreateNewJobIndustryAreas(jobId, industryAreas);
            // Assert
            foreach (var industryArea in industryAreas)
            {
                _jobIndustryAreaRepo.Verify(repo => repo.Create(It.Is<JobIndustryArea>(jia => jia.JobId == jobId && jia.IndustryArea == industryArea && jia.IndustryAreaId == industryArea.Id)));
            }
        }

        [TestMethod]
        public void CreateNewJobIndustryAreas_InvalidIdThrowArgumentOutOfRangeException()
        {
            // Arrange
            int jobId = 0;
            List<IndustryArea> industryAreas = new List<IndustryArea>
            {
                new IndustryArea { Id = 1, Title = "Area1" },
                new IndustryArea { Id = 2, Title = "Area2" }
            };
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workLifeLogicLayer.CreateNewJobIndustryAreas(jobId, industryAreas));
        }

        [TestMethod]
        public void UpdateNewJobIndustryAreas_UpdatesJobIndustryAreas()
        {
            // Arrange
            List<JobIndustryArea> jobIndustryAreas = new List<JobIndustryArea>
            {
                new JobIndustryArea { Id = 1, JobId = 1 },
                new JobIndustryArea { Id = 2, JobId = 2 }
            };
            List<Job> jobs = new List<Job>
            {
                new Job { Id = 1 },
                new Job { Id = 2 }
            };
            _jobIndustryAreaRepo.Setup(repo => repo.GetAll()).Returns(jobIndustryAreas);
            _jobRepo.Setup(repo => repo.GetAll()).Returns(jobs);
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.UpdateNewJobIndustryAreas();
            // Assert
            foreach (var job in jobs)
            {
                foreach (var industryArea in jobIndustryAreas)
                {
                    if (industryArea.JobId == job.Id)
                    {
                        _jobIndustryAreaRepo.Verify(repo => repo.Update(It.Is<JobIndustryArea>(jia => jia == industryArea && jia.Job == job)));
                    }
                }
            }
        }

        [TestMethod]
        public void GetJobById_ReturnsJob()
        {
            // Arrange
            int jobId = 1;
            Job expectedJob = new Job { Id = jobId, Title = "Sample Job" };
            _jobRepo.Setup(repo => repo.GetAll()).Returns(new List<Job>
            {
                expectedJob,
                new Job { Id = 2, Title = "Another Job" }
            });
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            var result = workLifeLogicLayer.GetJobById(jobId);
            // Assert
            Assert.AreEqual(jobId, result.Id);
        }

        [TestMethod]
        public void GetJobById_InvalidIdThrowArgumentOutOfRangeException()
        {
            // Arrange
            int jobId = 0;
            Job expectedJob = new Job { Id = jobId, Title = "Sample Job" };
            _jobRepo.Setup(repo => repo.GetAll()).Returns(new List<Job>
            {
                expectedJob,
                new Job { Id = 2, Title = "Another Job" }
            });
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workLifeLogicLayer.GetJobById(jobId));
        }

        [TestMethod]
        public void DeleteJobById_DeletesJob()
        {
            // Arrange
            int jobId = 1;
            Job jobToDelete = new Job { Id = jobId, Title = "Sample Job" };
            List<Job> jobs = new List<Job>
            {
                jobToDelete,
                new Job { Id = 2, Title = "Another Job" }
            };
            _jobRepo.Setup(repo => repo.GetAll()).Returns(jobs);
            _jobRepo.Setup(repo => repo.Delete(It.IsAny<Job>())).Callback<Job>((job) =>
            {
                jobs.Remove(job);
            });
            // Act
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            workLifeLogicLayer.DeleteJobById(jobId);
            List<Job> remainingJobs = _jobRepo.Object.GetAll().ToList();
            // Assert
            Assert.AreEqual(1, remainingJobs.Count);
            Assert.IsFalse(remainingJobs.Contains(jobToDelete));
        }

        [TestMethod]
        public void DeleteJobById_InvalidIdThrowArgumentOutOfRangeException()
        {
            // Arrange
            int jobId = 0;
            Job jobToDelete = new Job { Id = jobId, Title = "Sample Job" };
            List<Job> jobs = new List<Job>
            {
                jobToDelete,
                new Job { Id = 2, Title = "Another Job" }
            };
            _jobRepo.Setup(repo => repo.GetAll()).Returns(jobs);
            _jobRepo.Setup(repo => repo.Delete(It.IsAny<Job>())).Callback<Job>((job) =>
            {
                jobs.Remove(job);
            });
            WorkLifeLogicLayer workLifeLogicLayer = InitializeBLL();
            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workLifeLogicLayer.DeleteJobById(jobId));
        }
    }
}
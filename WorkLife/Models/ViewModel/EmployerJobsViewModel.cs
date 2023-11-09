using WorkLife.Areas.Identity.Data;

namespace WorkLife.Models.ViewModel
{
    public class EmployerJobsViewModel
    {
        public WorkLifeUser WorkLifeUser { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }
}

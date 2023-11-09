namespace WorkLife.Models
{
    public class IndustryArea
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<ApplicantIndustryArea> ApplicantsIndustryAreas { get; set; }
        public ICollection<EmployerIndustryArea> EmployersIndustryAreas { get; set; }
        public ICollection<JobIndustryArea> JobIndustryAreas { get; set; }
    }
}

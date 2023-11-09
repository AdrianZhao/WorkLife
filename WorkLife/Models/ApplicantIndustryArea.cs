using System.ComponentModel.DataAnnotations;

namespace WorkLife.Models
{
    public class ApplicantIndustryArea
    {
        public int Id { get; set; }
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
        public int IndustryAreaId { get; set; }
        public IndustryArea IndustryArea { get; set; }
    }
}

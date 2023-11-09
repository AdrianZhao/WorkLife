using System.ComponentModel.DataAnnotations;

namespace WorkLife.Models
{
    public class EmployerIndustryArea
    {
        public int Id { get; set; }
        public int EmployerId { get; set; }
        public Employer Employer { get; set; }
        public int IndustryAreaId { get; set; }
        public IndustryArea IndustryArea { get; set; }
    }
}

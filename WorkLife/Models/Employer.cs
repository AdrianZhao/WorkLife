using System.ComponentModel.DataAnnotations;

namespace WorkLife.Models
{
    public class Employer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Company name must be between 3 and 200 characters.")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 500 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Industry Areas")]
        public ICollection<EmployerIndustryArea>? IndustryAreas { get; set; }

        public string Region { get; set; }
    }
}

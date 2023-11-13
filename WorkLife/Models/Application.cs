using System.ComponentModel.DataAnnotations;

namespace WorkLife.Models
{
    public class Application
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Resume is required.")]
        [StringLength(5000, MinimumLength = 0, ErrorMessage = "Resume must be less than 5000 characters.")]
        public string Resume {  get; set; }
        public string? ApplicantEmail { get; set; }
        public Applicant? Applicant { get; set; }
        public int JobId { get; set; }
        public Job? Job { get; set; }
        [Display(Name = "Reference One Name")]
        public string? ReferenceOneName { get; set;}
        [Display(Name = "Reference One Email")]
        public string? ReferenceOneEmail { get; set; }
        [Display(Name = "Reference One Phone Number")]
        public string? ReferenceOnePhoneNumber { get; set; }
        [Display(Name = "Reference Two Name")]
        public string? ReferenceTwoName { get; set;}
        [Display(Name = "Reference Two Email")]
        public string? ReferenceTwoEmail { get; set; }
        [Display(Name = "Reference Two Phone Number")]
        public string? ReferenceTwoPhoneNumber { get; set; }
        [Display(Name = "Reference Three Name")]
        public string? ReferenceThreeName { get; set;}
        [Display(Name = "Reference Three Email")]
        public string? ReferenceThreeEmail { get; set; }
        [Display(Name = "Reference Three Phone Number")]
        public string? ReferenceThreePhoneNumber { get; set; }
        [Display(Name = "Reference Four Name")]
        public string? ReferenceFourName { get; set; }
        [Display(Name = "Reference Four Email")]
        public string? ReferenceFourEmail { get; set; }
        [Display(Name = "Reference Four Phone Number")]
        public string? ReferenceFourPhoneNumber { get; set; }
        [Display(Name = "Reference Five Name")]
        public string? ReferenceFiveName { get; set; }
        [Display(Name = "Reference Five Email")]
        public string? ReferenceFiveEmail { get; set; }
        [Display(Name = "Reference Five Phone Number")]
        public string? ReferenceFivePhoneNumber { get; set; }
    }
}

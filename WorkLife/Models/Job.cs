using System.ComponentModel.DataAnnotations;

namespace WorkLife.Models
{
    public class Job
    {
        public int Id { get; set; }

        public string? EmployerEmail { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Duration must be between 3 and 200 characters.")]
        public string Duration { get; set; }

        [RegularExpression("^[A-Za-z0-9]*$", ErrorMessage = "Competition should be alphanumeric.")]
        public string? Competition { get; set; }

        public string? Location { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 2000 characters.")]
        public string Description { get; set; }

        [Display(Name = "Industry Areas")]
        public ICollection<JobIndustryArea>? IndustryAreas { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Submission Deadline")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateMustBeAfter(ErrorMessage = "Please enter a valid date after today.")]
        public DateTime SubmissionDeadline { get; set; }

        public ICollection<Application>? Applications { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateMustBeAfter : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                DateTime today = DateTime.Today;
                return date > today;
            }
            return false;
        }
    }
}
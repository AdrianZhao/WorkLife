using System.ComponentModel.DataAnnotations;

namespace WorkLife.Models
{
    public class Applicant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Person name is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Person name must be between 3 and 200 characters.")]
        [Display(Name = "Person name")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Family name is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Family name must be between 3 and 200 characters.")]
        [Display(Name = "Family name")]
        public string FamilyName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateOfBirthRange(1915-01-01, ErrorMessage = "Please enter a valid date of birth between 1915-01-01 to present day.")]
        public DateTime DateOfBirth { get; set; }

        public string Region { get; set; }

        [Display(Name = "Industry Areas")]
        public ICollection<ApplicantIndustryArea>? IndustryAreas { get; set; }

        public ICollection<Application>? Applications { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateOfBirthRangeAttribute : ValidationAttribute
    {
        public DateOfBirthRangeAttribute(int minimumDate)
        {
            MinimumDate = minimumDate;
        }
        public int MinimumDate { get; }
        public override bool IsValid(object? value)
        {
            bool result = false;
            if (value is DateTime date)
            {
                DateTime maximumDate = DateTime.Now.Date;
                DateTime minimumDate = new DateTime(MinimumDate);
                result = date >= minimumDate && date <= maximumDate;
            }
            return result;
        }
    }
}

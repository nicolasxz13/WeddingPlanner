using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Wedder one is required")]
        [Display(Name = "Wedder One")]
        public string? Wedder_One { get; set; }

        [Required(ErrorMessage = "Wedder two is required")]
        [Display(Name = "Wedder Two")]
        public string? Wedder_Two { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [FutureDate]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "address is required")]
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Rsv>? Rsvs { get; set; }
        public User? Creator { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext
        )
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            DateTime inputDate = (DateTime)value;
            DateTime dateTimeNow = DateTime.Now;

            if (inputDate < dateTimeNow)
            {
                return new ValidationResult("The date must be a future date.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeddingPlanner.Data;

namespace WeddingPlanner.Models
{
    public class UserDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public String Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(8, ErrorMessage = "Password requires 8 values as minimum")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}

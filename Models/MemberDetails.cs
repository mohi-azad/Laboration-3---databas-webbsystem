using System.ComponentModel.DataAnnotations;

namespace Laboration_3.Models
{
    public class MemberDetails
    {
        // konstruktor
        public int MemberId { get; set; }

        [Required(ErrorMessage = "First name is required!")]
        [StringLength(30)]
        public String? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        [StringLength (30)]
        public String? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format!")]
        public String? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number!")]
        public String? Phone { get; set; }


        [Range(5, 110, ErrorMessage = "Age must be betweend 5 and 110!")]
        public int Age { get; set; }

        [Range(0,50000, ErrorMessage = "Score must be between 0-50 000")]
        public int Score { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ETickets.ModelView
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "Username or email")]
        public string UserNameOrEmail { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberMe{ get; set; }

        public bool EmailConfirmed { get; set; } = true;


    }
}

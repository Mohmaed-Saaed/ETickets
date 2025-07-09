using System.ComponentModel.DataAnnotations;

namespace ETickets.ModelView
{
    public class ForgetPasswordVM
    {

        [Display(Name = "Username or Email")]
        public string UserNameOrEmail { get; set; } = null!;
    }
}

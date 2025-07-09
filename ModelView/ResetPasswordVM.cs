using System.ComponentModel.DataAnnotations;

namespace ETickets.ModelView
{
    public class ResetPasswordVM
    {
        [Required]
        [Display(Name = "OTP")]


        public string UserId { get; set; } = null!;
        public int OTPNumber { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}

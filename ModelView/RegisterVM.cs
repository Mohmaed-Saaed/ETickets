using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ETickets.ModelView
{
    public class RegisterVM
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        public bool IsPasswordHiden { get; set; }
        public IEnumerable<SelectListItem>? UserRoles { get; set; }
        public string? Id { get; set; }
        public IEnumerable<string> Roles { get; set; } = null!; // This will add validation for this feild that it is required. null!;
    }
}

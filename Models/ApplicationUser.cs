using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ETickets.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

    }
}

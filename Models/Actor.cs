using Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETickets.Models;

public partial class Actor
{
    [Display(Name = "Actors")]
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Bio { get; set; }

    public string? ProfilePicture { get; set; }

    public string? News { get; set; }
    public virtual ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();
}

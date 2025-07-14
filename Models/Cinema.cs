using Microsoft.AspNetCore.Antiforgery;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETickets.Models;

public partial class Cinema
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CinemaLogo { get; set; }
    [Required]
    public int NumberOfChairs { get; set; }

    [Required]
    public string Address { get; set; } = null!;
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    public ICollection<Chair> Chairs { get; set; } = new List<Chair>();


}

using System;
using System.Collections.Generic;

namespace ETickets.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string ImgUrl { get; set; } = null!;

    public string TrailerUrl { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? MovieStatus { get; set; }

    public int? CinemaId { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Cinema? Cinema { get; set; }
}

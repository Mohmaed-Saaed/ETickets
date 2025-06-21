using Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETickets.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }  

    public string ImgUrl { get; set; } = null!;

    public string TrailerUrl { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int MovieDisplayStateId { get; set; }

    public bool MovieStatus { get; set; }

    public int? CinemaId { get; set; }

    public int? CategoryId { get; set; }

    public virtual ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();



    public virtual Category? Category { get; set; }
    public virtual MovieDisplayState? MovieDisplayState { get; set; }
    public virtual Cinema? Cinema { get; set; }
}

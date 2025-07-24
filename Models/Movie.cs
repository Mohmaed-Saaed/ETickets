using System.ComponentModel.DataAnnotations;

namespace ETickets.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal? Price { get; set; }
    [Display(Name = "Image")]
    public string? ImgUrl { get; set; } 
    [Display(Name = "Trailer Url")]
    public string? TrailerUrl { get; set; }

    [Display(Name = "Start date")]
    public DateOnly StartDate { get; set; }
    [Display(Name = "End date")]
    public DateOnly EndDate { get; set; }

    [Display(Name = "Movie state")]
    public int MovieDisplayStateId { get; set; }
    [Display(Name = "Movie status")]
    public bool MovieStatus { get; set; }
    [Display(Name = "Cinema")]
    public int? CinemaId { get; set; }
    [Display(Name = "Category")]
    public int? CategoryId { get; set; }
    public TimeSpan? Duration { get; set; }

    public virtual ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();
    public virtual ICollection<MovieImage>  MovieImages { get; set; } = new List<MovieImage>();
    public virtual Category? Category { get; set; }
    public virtual MovieDisplayState MovieDisplayState { get; set; } = null!;
    public virtual Cinema? Cinema { get; set; }
}

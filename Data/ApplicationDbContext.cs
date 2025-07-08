using ETickets.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ETickets.ModelView;

namespace ETickets.Data;

    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; }

        public virtual DbSet<ActorMovie> ActorMovies { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Cinema> Cinemas { get; set; }

        public virtual DbSet<Movie>  Movies { get; set; }

        public virtual DbSet<MovieImage> MovieImages { get; set; }

        public virtual DbSet<MovieDisplayState> MovieDisplayStates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Actors__3214EC07D759252C");

                entity.Property(e => e.Bio)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.News)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
                entity.Property(e => e.ProfilePicture)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
            
        
            modelBuilder.Entity<ActorMovie>(entity =>
            {
                entity.HasKey(e => new { e.ActorId, e.MovieId }); // Define composite primary key

                entity.HasOne(d => d.Actor)
                    .WithMany(d => d.ActorMovies) // This is correct, assuming Actor.ActorMovies exists
                    .HasForeignKey(d => d.ActorId)
                    .HasConstraintName("FK__ActorMovi__Actor__4BAC3F29");

                entity.HasOne(d => d.Movie)
                    .WithMany(d => d.ActorMovies) // This is correct, assuming Movie.ActorMovies exists
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK__ActorMovi__Movie__4CA06362");



            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07B8534E97");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cinema>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Cinemas__3214EC07DC1EFB74");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.CinemaLogo)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Description)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Movies__3214EC07DB4D565B");

                entity.Property(e => e.Description)
                    .HasMaxLength(5000)
                    .IsUnicode(false);
                entity.Property(e => e.EndDate).HasColumnType("datetime");
                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.StartDate).HasColumnType("datetime");
                entity.Property(e => e.TrailerUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category).WithMany(p => p.Movies)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Movies__Category__45F365D3");

                entity.HasOne(d => d.Cinema).WithMany(p => p.Movies)
                    .HasForeignKey(d => d.CinemaId)
                    .HasConstraintName("FK__Movies__CinemaId__44FF419A");

                entity.HasOne(d => d.MovieDisplayState).WithMany(p => p.Movies)
                      .HasForeignKey(d => d.MovieDisplayStateId);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }


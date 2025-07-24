using ETickets.Models;
using ETickets.ModelView.Admin;
using ETickets.Repositry.IRepositry;
using ETickets.Servies.IServies;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETickets.Servies
{
    public class MovieAdminSaveService : IMovieAdminSaveService
    {
        private readonly IMovieRepository _MovieRepository;
        private readonly IActorRepository _ActorRepository;
        private readonly IActorMovieRepository _ActorMovieRepository;
        private readonly ICategoryRepository _CategoryRepository;
        private readonly ICinemaRepository _CinemaRepository;
        private readonly IMovieDisplayStateRepository _MovieDisplayStateRepository;

        public MovieAdminSaveService(IMovieRepository movieRepository,
                               IActorRepository actorRepository,
                               IActorMovieRepository actorMovieRepository,
                               ICategoryRepository categoryRepository,
                               ICinemaRepository cinemaRepository,
                               IMovieDisplayStateRepository movieDisplayStateRepository
                               )
        {
            _MovieRepository = movieRepository;
            _ActorRepository = actorRepository;
            _ActorMovieRepository = actorMovieRepository;
            _CategoryRepository = categoryRepository;
            _CinemaRepository = cinemaRepository;
            _MovieDisplayStateRepository = movieDisplayStateRepository;
        }

        public AdminMovieSaveVM AdminMovieSaveVM(int Id)
        {

            Movie movie = _MovieRepository.GetOne(x => x.Id == Id) ?? new Movie { EndDate = DateOnly.FromDateTime(DateTime.Now), StartDate = DateOnly.FromDateTime(DateTime.Now) };

            var selectedActorIds = _ActorMovieRepository.Get(x => x.MovieId == Id).Select(am => am.ActorId).ToList();

            AdminMovieSaveVM adminMovieIndexVM = new AdminMovieSaveVM()
            {
                MovieStatus = _MovieDisplayStateRepository.Get().Select(x => new SelectListItem { Text = x.Status, Value = x.Id.ToString() }).ToList(),

                Categories = _CategoryRepository.Get().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList(),

                Cinemas = _CinemaRepository.Get().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList(),

                Actors = _ActorRepository.Get().Select(x => new SelectListItem { Text = $"{x.FirstName} {x.LastName}", Value = x.Id.ToString() }).ToList(),

                ActorsSelectList = _ActorMovieRepository.Get(x => x.MovieId == Id)
                .Select(x => new SelectListItem
                {
                    Text = $"{x.Actor.FirstName} {x.Actor.LastName}",
                    Value = x.Actor.Id
                   .ToString(),
                    Selected = selectedActorIds.Contains(x.ActorId)
                }).ToList(),

                Movie = movie,

                ActorIds = selectedActorIds,

                SubImages = _MovieRepository.GetMovieImages(x => x.MovieId == Id)
            };

            return adminMovieIndexVM;
        }
    }
}

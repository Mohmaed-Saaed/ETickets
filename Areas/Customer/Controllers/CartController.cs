using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry;
using ETickets.Repositry.IRepositry;
using ETickets.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ETickets.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = $"{SD.Employee}")]
    public class CartController : Controller
    {

        readonly public ICartRepository _CartRepository;
        readonly public IMovieRepository _MovieRepository;
        readonly public UserManager<ApplicationUser> _UserManager;


        public CartController(UserManager<ApplicationUser>  userManager,
                              ICartRepository cartRepository,
                              IMovieRepository movieRepository)
        {
            _CartRepository = cartRepository;
            _UserManager = userManager;
            _MovieRepository = movieRepository;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _UserManager.GetUserAsync(User);

            if( user is not null)
            {
                var userCart = _CartRepository.Get(c => c.ApplicationUserId == user.Id, [ x => x.Movie ]);

                ViewBag.TotalAmount = userCart.Sum(x => x.Count * x.Movie.Price);

                return View(userCart);

            }

            return new EmptyResult();
        }
        public async Task<IActionResult> AddToCart(int movieId , int quantity)
        {
            var user = await _UserManager.GetUserAsync(User);

            if (user is not null)
            {
                
                var movie = _MovieRepository.GetOne(x => x.Id == movieId);

                if (movie is not null) {

                    if(quantity == 0)
                    {
                        TempData["error"] = "Add quantity.";
                        return RedirectToAction("Details", "Home", new { area = "Customer", Id = movieId });
                    } else 
                    {
                        var cart = _CartRepository.GetOne(x => x.ApplicationUserId == user.Id && x.MovieId == movie.Id);
                        if (cart is not null)
                        {
                            cart.Count += quantity;
                            _CartRepository.Commit();
                        }
                        else
                        {
                            _CartRepository.Create(new Cart()
                            {
                                MovieId = movieId,
                                ApplicationUserId = user.Id,
                                Count = quantity
                            });
                        }
                
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Details", "Home", new { area = "Customer", Id = movieId });    
        }


        public async Task<JsonResult> Increment(int movieId)
        {

            var cartMovie = _CartRepository.GetOne(x => x.MovieId == movieId);

            if(cartMovie is not null)
            {

               var user = await _UserManager.GetUserAsync(User);

                if(user is not null)
                {
                    cartMovie.Count++;
                    _CartRepository.Commit();

                    var TotalAmount =  _CartRepository.Get(x => x.ApplicationUserId == user.Id , [x => x.Movie]).Sum(x => x.Count * x.Movie.Price);

                    return Json(new { status = true, movieCount = cartMovie.Count , TotalAmount });
                }
            }
            return Json(new {status = false});
        }

    }
}

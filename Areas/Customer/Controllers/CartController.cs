using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry;
using ETickets.Repositry.IRepositry;
using ETickets.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe.Climate;
using System.Threading.Tasks;

namespace ETickets.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = $"{SD.Customer}")]
    public class CartController : Controller
    {
        readonly public ICartRepository _CartRepository;
        readonly public IMovieRepository _MovieRepository;
        readonly public IReservationRepository _ReservationRepository;
        readonly public UserManager<ApplicationUser> _UserManager;


        public CartController(UserManager<ApplicationUser> userManager,
                              ICartRepository cartRepository,
                              IMovieRepository movieRepository,
                              IReservationRepository reservation)
        {
            _CartRepository = cartRepository;
            _UserManager = userManager;
            _MovieRepository = movieRepository;
            _ReservationRepository = reservation;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _UserManager.GetUserAsync(User);

            if (user is not null)
            {
                var userCart = _CartRepository.Get(c => c.ApplicationUserId == user.Id, [x => x.Movie , x => x.Movie.Cinema]);

                ViewBag.TotalAmount = userCart.Sum(x => x.Count * x.Movie.Price);

                return View(userCart);

            }

            return new EmptyResult();
        }
        public async Task<IActionResult> AddToCart(int movieId, int quantity)
        {
            var user = await _UserManager.GetUserAsync(User);

            if (user is not null)
            {

                var movie = _MovieRepository.GetOne(x => x.Id == movieId);

                if (movie is not null)
                {

                    if (quantity == 0)
                    {
                        TempData["error"] = "Add quantity.";
                        return RedirectToAction("Details", "Home", new { area = "Customer", Id = movieId });
                    }
                    else
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
                                CinemaId = movie.CinemaId,
                                Count = quantity
                            });
                        }

                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Details", "Home", new { area = "Customer", Id = movieId });
        }


        public async Task<IActionResult> Increment(int movieId)
        {

            var user = await _UserManager.GetUserAsync(User);

            if (user is not null)
            {
                var cartMovie = _CartRepository.GetOne(x => x.MovieId == movieId && user.Id == x.ApplicationUserId);

                if (cartMovie is not null)


                {
                    cartMovie.Count++;
                    _CartRepository.Commit();

                    return RedirectToAction(nameof(Index));
                }
            }
            return Json(new { status = false });
        }

        public async Task<IActionResult> Decrement(int movieId)
        {

            var user = await _UserManager.GetUserAsync(User);

            if (user is not null)
            {
                var cartMovie = _CartRepository.GetOne(x => x.MovieId == movieId && user.Id == x.ApplicationUserId);

                if (cartMovie is not null)
                {


                    cartMovie.Count--;

                    if (cartMovie.Count == 0)
                    {
                        return RedirectToAction(nameof(Delete), "Cart", new { area = "Customer", movieId });
                    }

                    _CartRepository.Commit();
                    return RedirectToAction(nameof(Index));
                }
            }
            return Json(new { status = false });
        }


        public async Task<IActionResult> Delete(int movieId)
        {

            var cartMovie = _CartRepository.GetOne(x => x.MovieId == movieId);

            if (cartMovie is not null)
            {
                var user = await _UserManager.GetUserAsync(User);

                if (user is not null)
                {
                    var userCart = _CartRepository.GetOne(x => x.MovieId == movieId && user.Id == x.ApplicationUserId);
                    if (userCart is not null)
                    {
                        _CartRepository.Delete(userCart);
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            TempData["error"] = "Somthing is wrong";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Pay()
        {
            var user = await _UserManager.GetUserAsync(User);

            if (user is not null)
            {
                var userCart = _CartRepository.Get(x => x.ApplicationUserId == user.Id, include: [x => x.Movie]);

                if (userCart is not null)
                {
                    _ReservationRepository.Create(new Reservation()
                    {
                        ApplicationUserId = user.Id,
                        CreatedAt = DateTime.UtcNow,
                        Total = userCart.Sum(x => x.Movie.Price * x.Count) ?? 0,
                        IsPaid = false,
                        PaymentMethod = Global.PaymentMethod.Visa

                    });

                    var lastReservation = _ReservationRepository.Get(x => x.ApplicationUserId == user.Id).LastOrDefault();

                    if (lastReservation is null)
                        return BadRequest();
                    

                    var options = new SessionCreateOptions
                    {
                        PaymentMethodTypes = new List<string> { "card" },
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                        SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Success/{lastReservation.Id}",
                        CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel/{lastReservation.Id}",
                    };


                    foreach (var item in userCart)
                    {
                        options.LineItems.Add(new SessionLineItemOptions()
                        {
                            PriceData = new SessionLineItemPriceDataOptions()
                            {
                                Currency = "EGP",
                                ProductData = new SessionLineItemPriceDataProductDataOptions()
                                {
                                    Name = item.Movie.Name
                                },
                                UnitAmount = (long?)(item.Movie.Price * item.Count)
                            },
                            Quantity = item.Count,

                        });
                    }

                    var service = new SessionService();
                    var session = service.Create(options);
                    lastReservation.SessionId = session.Id; // This saves the session Id.
                    _ReservationRepository.Commit();

                    HttpContext.Session.SetString("Cinfirm", "true");

                    return Redirect(session.Url);
               
                }

                return new EmptyResult();
            }

            return new EmptyResult();

        }

    }

}

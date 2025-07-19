using ETickets.Models;
using ETickets.Repositry;
using ETickets.Repositry.IRepositry;
using ETickets.Utilities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Threading.Tasks;


namespace ETickets.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Customer)]
    public class CheckoutController : Controller
    {

        private readonly IReservationRepository _ReservationRepository;
        private readonly ICartRepository _CartRepository;
        private readonly IReservationDetailRepository _ReservationDetailRepository;
        private readonly UserManager<ApplicationUser> _UserManager;
        public CheckoutController(IReservationRepository reservationRepository,
                                  ICartRepository cartRepository,
                                  IReservationDetailRepository reservationDetailRepository,
                                  UserManager<ApplicationUser> userManager)
        {
            _ReservationRepository = reservationRepository;
            _UserManager = userManager;
            _CartRepository = cartRepository;
            _ReservationDetailRepository = reservationDetailRepository;

        }

        public async Task<IActionResult> Success(int Id)
        {

            if (HttpContext.Session.GetString("Cinfirm") != "true")
            {

                return BadRequest();
            }

            HttpContext.Session.Remove("Cinfirm");

            var reservation = _ReservationRepository.GetOne(x => x.Id == Id);

            if (reservation is not null)
            {
                var user = await _UserManager.GetUserAsync(User);

                 if (user is null)
                    return BadRequest();


                var userCart = _CartRepository.Get(x => x.ApplicationUserId == user.Id, [x => x.Movie]);

                if (!userCart.Any())
                    return BadRequest();

                reservation.PaymentId = new SessionService().Get(reservation.SessionId).PaymentIntentId;

                reservation.IsPaid = true;

                _ReservationRepository.Commit();

                var reservationDetails = new List<ReservationDetail>();

                foreach (var cartItem in userCart)
                {
                    reservationDetails.Add(new ReservationDetail()
                    {
                        ReservationId = reservation.Id,
                        MovieId = cartItem.MovieId,
                        CinemaId = cartItem.CinemaId,
                        MoviePrice = (decimal)cartItem.Movie.Price

                    });
             
                }
                _ReservationDetailRepository.AddRange(reservationDetails);

                _CartRepository.RemoveRange(userCart);

                return View();
            } else
            {
                return NotFound();
            }

        }

        public IActionResult Cancel()
        {
            return View();
        }
    }
}

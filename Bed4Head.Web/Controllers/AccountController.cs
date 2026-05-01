using Bed4Head.Application.DTOs;
using Bed4Head.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bed4Head.Web.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyAccount()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (user == null)
                return NotFound(new { message = "User not found" });

            var bookings = await GetBookingsForUser(user.Id);
            var today = DateTime.UtcNow.Date;
            var activeBookings = bookings
                .Where(b => !IsCancelled(b.Status))
                .ToList();

            var paymentMethods = await _context.PaymentMethods
                .AsNoTracking()
                .Where(p => p.UserId == user.Id)
                .OrderByDescending(p => p.IsPrimary)
                .ThenBy(p => p.CardType)
                .Select(p => new AccountPaymentMethodDTO
                {
                    Id = p.Id,
                    CardType = p.CardType,
                    LastFourDigits = p.LastFourDigits,
                    ExpiryDate = p.ExpiryDate,
                    IsPrimary = p.IsPrimary
                })
                .ToListAsync();

            var reviewsCount = await _context.Reviews
                .AsNoTracking()
                .CountAsync(r => r.UserId == user.Id);

            var summary = new AccountSummaryDTO
            {
                Profile = new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role,
                    DisplayName = user.DisplayName,
                    Phone = user.Phone,
                    BirthDate = user.BirthDate,
                    Country = user.Country,
                    City = user.City,
                    IsEmailConfirmed = user.IsEmailConfirmed ?? false,
                    AvatarUrl = user.AvatarUrl,
                    TravelPurpose = user.TravelPurpose,
                    PreferredCurrencyCode = user.PreferredCurrencyCode,
                    CreatedAt = user.CreatedAt,
                    NewsSeasonalOffers = user.NewsSeasonalOffers ?? false,
                    NewsFavoriteCities = user.NewsFavoriteCities ?? false,
                    NewsAcrossWorld = user.NewsAcrossWorld ?? false,
                    NewsAffordableTravel = user.NewsAffordableTravel ?? false
                },
                Bookings = new AccountBookingSectionsDTO
                {
                    Upcoming = activeBookings
                        .Where(b => b.CheckOut.Date >= today)
                        .OrderBy(b => b.CheckIn)
                        .ToList(),
                    Past = activeBookings
                        .Where(b => b.CheckOut.Date < today)
                        .OrderByDescending(b => b.CheckOut)
                        .ToList(),
                    Cancelled = bookings
                        .Where(b => IsCancelled(b.Status))
                        .OrderByDescending(b => b.CreatedAt)
                        .ToList()
                },
                TravelPreferences = new AccountTravelPreferencesDTO
                {
                    TravelPurpose = user.TravelPurpose,
                    PreferredCurrencyCode = user.PreferredCurrencyCode,
                    Country = user.Country,
                    City = user.City
                },
                NotificationPreferences = new AccountNotificationPreferencesDTO
                {
                    NewsSeasonalOffers = user.NewsSeasonalOffers ?? false,
                    NewsFavoriteCities = user.NewsFavoriteCities ?? false,
                    NewsAcrossWorld = user.NewsAcrossWorld ?? false,
                    NewsAffordableTravel = user.NewsAffordableTravel ?? false
                },
                PaymentMethods = paymentMethods,
                Stats = new AccountStatsDTO
                {
                    TotalBookings = bookings.Count,
                    UpcomingBookings = activeBookings.Count(b => b.CheckOut.Date >= today),
                    CancelledBookings = bookings.Count(b => IsCancelled(b.Status)),
                    TotalSpent = activeBookings
                        .Where(b => b.CheckOut.Date < today)
                        .Sum(b => b.TotalPrice),
                    ReviewsCount = reviewsCount
                }
            };

            return Ok(summary);
        }

        private async Task<List<BookingDTO>> GetBookingsForUser(Guid userId)
        {
            var rows = await (from booking in _context.Bookings.AsNoTracking()
                              join room in _context.Rooms.AsNoTracking()
                                  on booking.RoomId equals room.Id into roomJoin
                              from room in roomJoin.DefaultIfEmpty()
                              join hotel in _context.Hotels.AsNoTracking()
                                  on room.HotelId equals hotel.Id into hotelJoin
                              from hotel in hotelJoin.DefaultIfEmpty()
                              where booking.UserId == userId
                              orderby booking.CreatedAt descending
                              select new { Booking = booking, Room = room, Hotel = hotel })
                .ToListAsync();

            return rows
                .Select(row => new BookingDTO
                {
                    Id = row.Booking.Id,
                    UserId = row.Booking.UserId,
                    RoomId = row.Booking.RoomId,
                    HotelId = row.Room?.HotelId,
                    CheckIn = row.Booking.CheckIn,
                    CheckOut = row.Booking.CheckOut,
                    CreatedAt = row.Booking.CreatedAt,
                    AdultsCount = row.Booking.AdultsCount,
                    ChildrenCount = row.Booking.ChildrenCount,
                    Nights = Math.Max(0, (row.Booking.CheckOut.Date - row.Booking.CheckIn.Date).Days),
                    CallMe = row.Booking.CallMe,
                    SendEmail = row.Booking.SendEmail,
                    TotalPrice = row.Booking.TotalPrice,
                    PricePerNight = row.Room?.Price,
                    CurrencyCode = row.Room?.CurrencyCode ?? row.Hotel?.CurrencyCode,
                    Status = row.Booking.Status,
                    HotelName = row.Hotel?.Name,
                    HotelCity = row.Hotel?.City,
                    HotelCountry = row.Hotel?.Country,
                    HotelAddress = row.Hotel?.Address,
                    RoomTitle = row.Room?.Title
                })
                .ToList();
        }

        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                         ?? User.FindFirstValue("sub");

            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
        }

        private static bool IsCancelled(string? status)
        {
            return string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(status, "Canceled", StringComparison.OrdinalIgnoreCase);
        }
    }
}

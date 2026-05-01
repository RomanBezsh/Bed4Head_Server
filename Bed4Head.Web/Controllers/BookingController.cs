using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bed4Head.Web.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMemoryCache _cache;

        public BookingController(IBookingService bookingService, IMemoryCache cache)
        {
            _bookingService = bookingService;
            _cache = cache;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateBookingDTO dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            try
            {
                var booking = await _bookingService.CreateAsync(dto, userId.Value);
                return Ok(new { message = "Booking created successfully", booking });
            }
            catch (Exception ex) when (ex.Message.Contains("already booked", StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex) when (
                ex.Message.Contains("Room not found", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("Invalid dates", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("WORKS");
        }

        [HttpGet("user/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetUserBookings(Guid userId)
        {
            if (!IsAdmin() && GetCurrentUserId() != userId)
                return Forbid();

            var bookings = await _bookingService.GetByUserIdAsync(userId);
            return Ok(bookings);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var bookings = await _bookingService.GetByUserIdAsync(userId.Value);
            return Ok(bookings);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
                return NotFound();

            if (!IsAdmin() && GetCurrentUserId() != booking.UserId)
                return Forbid();

            return Ok(booking);
        }

        [HttpPatch("{id:guid}/cancel")]
        [Authorize]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
                return NotFound();

            if (!IsAdmin() && GetCurrentUserId() != booking.UserId)
                return Forbid();

            await _bookingService.CancelAsync(id);
            return Ok(new { message = "Booking cancelled successfully" });
        }

        [HttpGet("{id:guid}/pdf")]
        [Authorize]
        public async Task<IActionResult> DownloadPdf(Guid id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
                return NotFound();

            if (!IsAdmin() && GetCurrentUserId() != booking.UserId)
                return Forbid();

            var cacheKey = $"booking-pdf:{booking.Id:N}";
            if (!_cache.TryGetValue(cacheKey, out byte[]? pdf) || pdf == null)
            {
                pdf = BuildBookingPdf(booking);
                _cache.Set(cacheKey, pdf, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                });
            }

            Response.Headers.CacheControl = "private, max-age=600";
            return File(pdf, "application/pdf", $"booking-{booking.Id:N}.pdf");
        }

        [HttpPatch("{id:guid}/status")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] string status)
        {
            await _bookingService.UpdateStatusAsync(id, status);
            return Ok(new { message = "Status updated successfully" });
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();
            return Ok(bookings);
        }

        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                         ?? User.FindFirstValue("sub");

            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
        }

        private bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        private static byte[] BuildBookingPdf(BookingDTO booking)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily(Fonts.Arial));

                    page.Header()
                        .Column(column =>
                        {
                            column.Item().Text("Bed4Head").FontSize(24).Bold().FontColor(Colors.Blue.Darken2);
                            column.Item().Text("Booking confirmation").FontSize(16).FontColor(Colors.Grey.Darken2);
                        });

                    page.Content()
                        .PaddingVertical(24)
                        .Column(column =>
                        {
                            column.Spacing(16);

                            column.Item()
                                .Background(Colors.Grey.Lighten4)
                                .Padding(16)
                                .Column(card =>
                                {
                                    card.Spacing(6);
                                    card.Item().Text(booking.HotelName ?? "Unknown hotel").FontSize(18).Bold();
                                    card.Item().Text(FormatAddress(booking));
                                    card.Item().Text(booking.RoomTitle ?? "Unknown room");
                                });

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(130);
                                    columns.RelativeColumn();
                                });

                                AddInfoRow(table, "Booking ID", booking.Id.ToString());
                                AddInfoRow(table, "Status", booking.Status ?? "Unknown");
                                AddInfoRow(table, "Check-in", booking.CheckIn.ToString("yyyy-MM-dd"));
                                AddInfoRow(table, "Check-out", booking.CheckOut.ToString("yyyy-MM-dd"));
                                AddInfoRow(table, "Nights", booking.Nights.ToString());
                                AddInfoRow(table, "Guests", $"{booking.AdultsCount} adults, {booking.ChildrenCount} children");
                                AddInfoRow(table, "Created", booking.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
                            });

                            column.Item()
                                .AlignRight()
                                .Column(total =>
                                {
                                    total.Item().Text("Total").FontSize(12).FontColor(Colors.Grey.Darken1);
                                    total.Item().Text($"{booking.TotalPrice:0.##} {booking.CurrencyCode}".Trim())
                                        .FontSize(22)
                                        .Bold();
                                });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Generated by Bed4Head");
                            text.Span(" · Page ");
                            text.CurrentPageNumber();
                        });
                });
            }).GeneratePdf();
        }

        private static void AddInfoRow(TableDescriptor table, string label, string value)
        {
            table.Cell().PaddingVertical(5).Text(label).SemiBold().FontColor(Colors.Grey.Darken2);
            table.Cell().PaddingVertical(5).Text(value);
        }

        private static string FormatAddress(BookingDTO booking)
        {
            var address = string.Join(", ", new[]
            {
                booking.HotelAddress,
                booking.HotelCity,
                booking.HotelCountry
            }.Where(part => !string.IsNullOrWhiteSpace(part)));

            return string.IsNullOrWhiteSpace(address) ? "Address not specified" : address;
        }
    }
}

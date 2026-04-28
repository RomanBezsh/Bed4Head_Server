using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head.Web.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IHotelFaqService _hotelFaqService;
        private readonly IHotelPhotoService _hotelPhotoService;
        private readonly IWebHostEnvironment _env;

        public HotelController(
            IHotelService hotelService,
            IHotelFaqService hotelFaqService,
            IHotelPhotoService hotelPhotoService,
            IWebHostEnvironment env)
        {
            _hotelService = hotelService;
            _hotelFaqService = hotelFaqService;
            _hotelPhotoService = hotelPhotoService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hotels = await _hotelService.GetAllSummariesAsync();
            return Ok(hotels);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            return Ok(hotel);
        }

        [HttpGet("{id:guid}/facilities")]
        public async Task<IActionResult> GetFacilities(Guid id)
        {
            var hotel = await _hotelService.GetFullByIdAsync(id);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            return Ok(hotel.Amenities);
        }

        [HttpGet("{id:guid}/faqs")]
        public async Task<IActionResult> GetFaqs(Guid id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            var faqs = await _hotelFaqService.GetByHotelIdAsync(id);
            return Ok(faqs);
        }

        [HttpGet("{id:guid}/photos")]
        public async Task<IActionResult> GetPhotos(Guid id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            var photos = await _hotelPhotoService.GetByHotelIdAsync(id);
            return Ok(photos);
        }

        [HttpGet("{id:guid}/full")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetFullById(Guid id)
        {
            var hotel = await _hotelService.GetFullByIdAsync(id);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            return Ok(hotel);
        }

        [HttpGet("chain/{chainId:guid}")]
        public async Task<IActionResult> GetByChainId(Guid chainId)
        {
            var hotels = await _hotelService.GetByChainIdAsync(chainId);
            return Ok(hotels);
        }

        [HttpPost("admin")]
        [Authorize(Policy = "AdminOnly")]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> CreateFromAdmin([FromForm] CreateHotelAdminRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Address) ||
                string.IsNullOrWhiteSpace(request.City))
            {
                return BadRequest(new { message = "Name, address and city are required" });
            }

            var photoUrls = new List<string>();
            foreach (var photo in request.Photos)
            {
                var savedPhotoUrl = await SaveHotelPhotoAsync(photo);
                if (!string.IsNullOrWhiteSpace(savedPhotoUrl))
                {
                    photoUrls.Add(savedPhotoUrl);
                }
            }

            var hotelId = await _hotelService.CreateAdminAsync(new CreateAdminHotelDTO
            {
                Name = request.Name,
                Description = request.Description,
                ShortDescription = request.ShortDescription,
                Stars = request.Stars,
                HotelType = request.Type,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                PostalCode = request.PostalCode,
                Phone = request.Phone,
                Email = request.Email,
                BasePricePerNight = request.BasePricePerNight,
                CurrencyCode = request.CurrencyCode,
                Coordinates = request.Coordinates,
                NearbyPlaces = request.NearbyPlaces,
                ImportantInfo = request.ImportantInfo,
                Status = request.Status,
                Facilities = request.Facilities,
                Faqs = request.Faqs,
                PhotoUrls = photoUrls
            });

            return Ok(new
            {
                id = hotelId,
                message = "Hotel created successfully"
            });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(Guid id, [FromBody] HotelDetailsDTO request)
        {
            if (id != request.Id)
            {
                return BadRequest(new { message = "Route id does not match body id" });
            }

            var existingHotel = await _hotelService.GetByIdAsync(id);
            if (existingHotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            await _hotelService.UpdateAsync(request);
            return Ok(new { message = "Hotel updated successfully" });
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingHotel = await _hotelService.GetByIdAsync(id);
            if (existingHotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            await _hotelService.DeleteAsync(id);
            return Ok(new { message = "Hotel deleted successfully" });
        }

        private async Task<string?> SaveHotelPhotoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var uploadsFolder = Path.Combine(webRoot, "uploads", "hotels");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            return $"/uploads/hotels/{uniqueFileName}";
        }
    }
}

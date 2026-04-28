using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Domain.Entities;
using Bed4Head.Infrastructure.Data;
using Bed4Head.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
namespace Bed4Head.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _db;
        private readonly AppDbContext _context;

        public HotelService(IUnitOfWork db, AppDbContext context)
        {
            _db = db;
            _context = context;
        }
        public async Task<IEnumerable<HotelSummaryDTO>> GetAllSummariesAsync()
        {
            var hotels = await _db.Hotels.GetAllAsync();
            return hotels.Select(h => new HotelSummaryDTO
            {
                Id = h.Id,
                Name = h.Name,
                City = h.City,
                Country = h.Country,
                Stars = h.Stars,
                HotelType = h.HotelType,
                BasePricePerNight = h.BasePricePerNight,
                CurrencyCode = h.CurrencyCode,
                OverallRating = h.OverallRating,
                RatingLabel = h.RatingLabel,
                ReviewsCount = h.ReviewsCount,
                IsFeatured = h.IsFeatured
            });
        }
        public async Task<HotelDetailsDTO?> GetByIdAsync(Guid id)
        {
            var h = await _db.Hotels.GetByIdAsync(id);
            if (h == null) return null;
            return new HotelDetailsDTO
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                ShortDescription = h.ShortDescription,
                Stars = h.Stars,
                HotelType = h.HotelType,
                Address = h.Address,
                City = h.City,
                Country = h.Country,
                PostalCode = h.PostalCode,
                Latitude = h.Latitude,
                Longitude = h.Longitude,
                DistanceFromCenterKm = h.DistanceFromCenterKm,
                Phone = h.Phone,
                Email = h.Email,
                WebsiteUrl = h.WebsiteUrl,
                BasePricePerNight = h.BasePricePerNight,
                CurrencyCode = h.CurrencyCode,
                OverallRating = h.OverallRating,
                RatingLabel = h.RatingLabel,
                ReviewsCount = h.ReviewsCount,
                CheckInFrom = h.CheckInFrom,
                CheckOutUntil = h.CheckOutUntil,
                IsFeatured = h.IsFeatured,
                HotelChainId = h.HotelChainId
            };
        }
        public async Task<HotelFullDTO?> GetFullByIdAsync(Guid id)
        {
            var hotel = await _context.Hotels
                .AsNoTracking()
                .Include(h => h.Amenities)
                .Include(h => h.Photos)
                .Include(h => h.NearbyPlaces)
                .Include(h => h.Faqs)
                .Include(h => h.Reviews)
                .Include(h => h.Rooms)
                    .ThenInclude(r => r.Photos)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null)
            {
                return null;
            }

            return new HotelFullDTO
            {
                Hotel = new HotelDetailsDTO
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    Description = hotel.Description,
                    ShortDescription = hotel.ShortDescription,
                    Stars = hotel.Stars,
                    HotelType = hotel.HotelType,
                    Address = hotel.Address,
                    City = hotel.City,
                    Country = hotel.Country,
                    PostalCode = hotel.PostalCode,
                    Latitude = hotel.Latitude,
                    Longitude = hotel.Longitude,
                    DistanceFromCenterKm = hotel.DistanceFromCenterKm,
                    Phone = hotel.Phone,
                    Email = hotel.Email,
                    WebsiteUrl = hotel.WebsiteUrl,
                    BasePricePerNight = hotel.BasePricePerNight,
                    CurrencyCode = hotel.CurrencyCode,
                    OverallRating = hotel.OverallRating,
                    RatingLabel = hotel.RatingLabel,
                    ReviewsCount = hotel.ReviewsCount,
                    CheckInFrom = hotel.CheckInFrom,
                    CheckOutUntil = hotel.CheckOutUntil,
                    IsFeatured = hotel.IsFeatured,
                    HotelChainId = hotel.HotelChainId
                },
                Amenities = hotel.Amenities
                    .OrderBy(a => a.DisplayOrder)
                    .Select(a => new AmenityDTO
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Category = a.Category,
                        IsHighlighted = a.IsHighlighted,
                        DisplayOrder = a.DisplayOrder
                    })
                    .ToList(),
                Photos = hotel.Photos
                    .OrderBy(p => p.DisplayOrder)
                    .Select(p => new HotelPhotoDTO
                    {
                        Id = p.Id,
                        Url = p.Url,
                        Caption = p.Caption,
                        Category = p.Category,
                        IsPrimary = p.IsPrimary,
                        DisplayOrder = p.DisplayOrder,
                        HotelId = p.HotelId
                    })
                    .ToList(),
                NearbyPlaces = hotel.NearbyPlaces
                    .OrderBy(p => p.DistanceInMeters)
                    .Select(p => new NearbyPlaceDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        PlaceType = p.PlaceType,
                        Address = p.Address,
                        DistanceInMeters = p.DistanceInMeters,
                        WalkingMinutes = p.WalkingMinutes,
                        HotelId = p.HotelId
                    })
                    .ToList(),
                Faqs = hotel.Faqs
                    .OrderBy(f => f.DisplayOrder)
                    .Select(f => new HotelFaqDTO
                    {
                        Id = f.Id,
                        Question = f.Question,
                        Answer = f.Answer,
                        DisplayOrder = f.DisplayOrder,
                        HotelId = f.HotelId
                    })
                    .ToList(),
                Reviews = hotel.Reviews
                    .OrderByDescending(r => r.CreatedAt)
                    .Select(r => new ReviewDTO
                    {
                        Id = r.Id,
                        Comment = r.Comment,
                        Title = r.Title,
                        CreatedAt = r.CreatedAt,
                        StayedAt = r.StayedAt,
                        TripType = r.TripType,
                        IsVerifiedStay = r.IsVerifiedStay,
                        OverallScore = r.OverallScore,
                        Facilities = r.Facilities,
                        Staff = r.Staff,
                        Cleanliness = r.Cleanliness,
                        Comfort = r.Comfort,
                        Location = r.Location,
                        ValueForMoney = r.ValueForMoney,
                        HotelId = r.HotelId,
                        UserId = r.UserId
                    })
                    .ToList(),
                Rooms = hotel.Rooms
                    .Select(r => new RoomDTO
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Description = r.Description,
                        Price = r.Price,
                        CurrencyCode = r.CurrencyCode,
                        BedType = r.BedType,
                        RoomType = r.RoomType,
                        View = r.View,
                        AreaInSquareMeters = r.AreaInSquareMeters,
                        MaxGuests = r.MaxGuests,
                        AvailableUnits = r.AvailableUnits,
                        FreeCancellation = r.FreeCancellation,
                        BreakfastIncluded = r.BreakfastIncluded,
                        PrivateBathroom = r.PrivateBathroom,
                        HotelId = r.HotelId,
                        PhotoUrls = r.Photos.Select(p => p.Url).ToList()
                    })
                    .ToList()
            };
        }
        public async Task<Guid> CreateAsync(HotelDetailsDTO dto)
        {
            var hotel = new Hotel
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description ?? string.Empty,
                ShortDescription = dto.ShortDescription,
                Stars = dto.Stars,
                HotelType = dto.HotelType ?? "Hotel",
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                DistanceFromCenterKm = dto.DistanceFromCenterKm,
                Phone = dto.Phone,
                Email = dto.Email,
                WebsiteUrl = dto.WebsiteUrl,
                BasePricePerNight = dto.BasePricePerNight,
                CurrencyCode = dto.CurrencyCode,
                OverallRating = dto.OverallRating,
                RatingLabel = dto.RatingLabel,
                ReviewsCount = dto.ReviewsCount,
                CheckInFrom = dto.CheckInFrom,
                CheckOutUntil = dto.CheckOutUntil,
                IsFeatured = dto.IsFeatured,
                HotelChainId = dto.HotelChainId
            };
            await _db.Hotels.AddAsync(hotel);
            await _db.CompleteAsync();
            return hotel.Id;
        }

        public async Task<Guid> CreateAdminAsync(CreateAdminHotelDTO dto)
        {
            var hotel = new Hotel
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Description = dto.Description?.Trim() ?? string.Empty,
                ShortDescription = string.IsNullOrWhiteSpace(dto.ShortDescription) ? null : dto.ShortDescription.Trim(),
                Stars = Math.Clamp(dto.Stars, 1, 5),
                HotelType = string.IsNullOrWhiteSpace(dto.HotelType) ? "Hotel" : dto.HotelType.Trim(),
                Address = dto.Address.Trim(),
                City = dto.City.Trim(),
                Country = dto.Country.Trim(),
                PostalCode = string.IsNullOrWhiteSpace(dto.PostalCode) ? null : dto.PostalCode.Trim(),
                Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone.Trim(),
                Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim(),
                BasePricePerNight = dto.BasePricePerNight,
                CurrencyCode = string.IsNullOrWhiteSpace(dto.CurrencyCode) ? "USD" : dto.CurrencyCode.Trim().ToUpperInvariant(),
                OverallRating = 0,
                ReviewsCount = 0,
                IsFeatured = string.Equals(dto.Status, "Active", StringComparison.OrdinalIgnoreCase)
            };

            ApplyCoordinates(hotel, dto.Coordinates);

            hotel.Photos = dto.PhotoUrls
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .Select((url, index) => new HotelPhoto
                {
                    Id = Guid.NewGuid(),
                    Url = url,
                    IsPrimary = index == 0,
                    DisplayOrder = index
                })
                .ToList();

            hotel.NearbyPlaces = ParseNearbyPlaces(dto.NearbyPlaces)
                .Select(place => new NearbyPlace
                {
                    Id = Guid.NewGuid(),
                    Name = place.Name,
                    PlaceType = place.PlaceType,
                    DistanceInMeters = place.DistanceInMeters,
                    WalkingMinutes = place.WalkingMinutes,
                    Address = place.Address
                })
                .ToList();

            hotel.Faqs = ParseFaqs(dto.Faqs)
                .Select((faq, index) => new HotelFaq
                {
                    Id = Guid.NewGuid(),
                    Question = faq.Question,
                    Answer = faq.Answer,
                    DisplayOrder = index
                })
                .ToList();

            hotel.Amenities = await ResolveAmenitiesAsync(dto.Facilities);

            await _db.Hotels.AddAsync(hotel);
            await _db.CompleteAsync();

            return hotel.Id;
        }
        public async Task UpdateAsync(HotelDetailsDTO dto)
        {
            var hotel = await _db.Hotels.GetByIdAsync(dto.Id);
            if (hotel == null) return;
            hotel.Name = dto.Name;
            hotel.Description = dto.Description ?? string.Empty;
            hotel.ShortDescription = dto.ShortDescription;
            hotel.Stars = dto.Stars;
            hotel.HotelType = dto.HotelType ?? hotel.HotelType;
            hotel.Address = dto.Address;
            hotel.City = dto.City;
            hotel.Country = dto.Country;
            hotel.PostalCode = dto.PostalCode;
            hotel.Latitude = dto.Latitude;
            hotel.Longitude = dto.Longitude;
            hotel.DistanceFromCenterKm = dto.DistanceFromCenterKm;
            hotel.Phone = dto.Phone;
            hotel.Email = dto.Email;
            hotel.WebsiteUrl = dto.WebsiteUrl;
            hotel.BasePricePerNight = dto.BasePricePerNight;
            hotel.CurrencyCode = dto.CurrencyCode;
            hotel.OverallRating = dto.OverallRating;
            hotel.RatingLabel = dto.RatingLabel;
            hotel.ReviewsCount = dto.ReviewsCount;
            hotel.CheckInFrom = dto.CheckInFrom;
            hotel.CheckOutUntil = dto.CheckOutUntil;
            hotel.IsFeatured = dto.IsFeatured;
            hotel.HotelChainId = dto.HotelChainId;
            await _db.Hotels.UpdateAsync(hotel);
            await _db.CompleteAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            await _db.Hotels.DeleteAsync(id);
            await _db.CompleteAsync();
        }
        public async Task<IEnumerable<HotelSummaryDTO>> GetByChainIdAsync(Guid chainId)
        {
            var all = await _db.Hotels.GetAllAsync();
            return all.Where(h => h.HotelChainId == chainId)
                      .Select(h => new HotelSummaryDTO
                      {
                          Id = h.Id,
                          Name = h.Name,
                          City = h.City,
                          Country = h.Country,
                          Stars = h.Stars,
                          HotelType = h.HotelType,
                          BasePricePerNight = h.BasePricePerNight,
                          CurrencyCode = h.CurrencyCode,
                          OverallRating = h.OverallRating,
                          RatingLabel = h.RatingLabel,
                          ReviewsCount = h.ReviewsCount,
                          IsFeatured = h.IsFeatured
                      });
        }

        private void ApplyCoordinates(Hotel hotel, string? coordinates)
        {
            if (!TryParseCoordinates(coordinates, out var latitude, out var longitude))
            {
                return;
            }

            hotel.Latitude = latitude;
            hotel.Longitude = longitude;
        }

        private static bool TryParseCoordinates(string? coordinates, out double latitude, out double longitude)
        {
            latitude = default;
            longitude = default;

            if (string.IsNullOrWhiteSpace(coordinates))
            {
                return false;
            }

            var normalized = coordinates.Trim().Trim('(', ')', '[', ']');
            var semicolonParts = normalized
                .Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (semicolonParts.Length == 2 &&
                TryParseCoordinateValue(semicolonParts[0], out latitude) &&
                TryParseCoordinateValue(semicolonParts[1], out longitude))
            {
                return true;
            }

            var commaParts = normalized
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (commaParts.Length == 2 &&
                TryParseCoordinateValue(commaParts[0], out latitude) &&
                TryParseCoordinateValue(commaParts[1], out longitude))
            {
                return true;
            }

            if (commaParts.Length == 4 &&
                TryParseCoordinateValue($"{commaParts[0]},{commaParts[1]}", out latitude) &&
                TryParseCoordinateValue($"{commaParts[2]},{commaParts[3]}", out longitude))
            {
                return true;
            }

            var numberMatches = Regex.Matches(normalized, @"[-+]?\d+(?:[.,]\d+)?");
            if (numberMatches.Count >= 2 &&
                TryParseCoordinateValue(numberMatches[0].Value, out latitude) &&
                TryParseCoordinateValue(numberMatches[1].Value, out longitude))
            {
                return true;
            }

            return false;
        }

        private static bool TryParseCoordinateValue(string rawValue, out double value)
        {
            var normalized = rawValue.Trim();

            if (double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }

            normalized = normalized.Replace(',', '.');
            return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        private async Task<List<Amenity>> ResolveAmenitiesAsync(IEnumerable<string> facilityNames)
        {
            static (string Category, string Name) ParseFacility(string raw)
            {
                if (string.IsNullOrWhiteSpace(raw))
                {
                    return ("General", string.Empty);
                }

                var value = raw.Trim();
                var parts = value.Split("|||", 2, StringSplitOptions.TrimEntries);
                if (parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[0]) && !string.IsNullOrWhiteSpace(parts[1]))
                {
                    return (parts[0], parts[1]);
                }

                return ("General", value);
            }

            var facilities = facilityNames
                .Where(raw => !string.IsNullOrWhiteSpace(raw))
                .Select(ParseFacility)
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                .Select(g =>
                {
                    // If the same amenity name comes from different categories, prefer a non-General one.
                    var preferred = g.FirstOrDefault(x => !string.Equals(x.Category, "General", StringComparison.OrdinalIgnoreCase));
                    return preferred.Name != null ? preferred : g.First();
                })
                .ToList();

            if (facilities.Count == 0)
            {
                return [];
            }

            var names = facilities.Select(x => x.Name).ToList();
            var existingAmenities = await _context.Amenities
                .Where(a => names.Contains(a.Name))
                .ToListAsync();

            var amenitiesByName = existingAmenities.ToDictionary(a => a.Name, StringComparer.OrdinalIgnoreCase);
            var resolvedAmenities = new List<Amenity>(names.Count);
            var displayOrder = 0;

            var hasChanges = false;
            foreach (var facility in facilities)
            {
                if (amenitiesByName.TryGetValue(facility.Name, out var existingAmenity))
                {
                    if (!string.IsNullOrWhiteSpace(facility.Category) &&
                        !string.Equals(existingAmenity.Category, facility.Category, StringComparison.OrdinalIgnoreCase))
                    {
                        existingAmenity.Category = facility.Category;
                        hasChanges = true;
                    }

                    resolvedAmenities.Add(existingAmenity);
                    continue;
                }

                var amenity = new Amenity
                {
                    Id = Guid.NewGuid(),
                    Name = facility.Name,
                    Category = string.IsNullOrWhiteSpace(facility.Category) ? "General" : facility.Category,
                    DisplayOrder = displayOrder++
                };

                resolvedAmenities.Add(amenity);
            }

            if (hasChanges)
            {
                await _context.SaveChangesAsync();
            }

            return resolvedAmenities;
        }

        private static IEnumerable<ParsedNearbyPlace> ParseNearbyPlaces(string? rawNearbyPlaces)
        {
            if (string.IsNullOrWhiteSpace(rawNearbyPlaces))
            {
                yield break;
            }

            var entries = rawNearbyPlaces.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var entry in entries)
            {
                var parts = entry.Split(',', StringSplitOptions.TrimEntries);
                if (parts.Length < 3)
                {
                    continue;
                }

                yield return new ParsedNearbyPlace
                {
                    PlaceType = parts[0],
                    Name = parts[1],
                    DistanceInMeters = ParseDistanceToMeters(parts[2]),
                    WalkingMinutes = parts.Length > 3 && int.TryParse(parts[3], out var walkingMinutes)
                        ? walkingMinutes
                        : null,
                    Address = parts.Length > 4 ? parts[4] : null
                };
            }
        }

        private static double ParseDistanceToMeters(string rawDistance)
        {
            var value = rawDistance.Trim().ToLowerInvariant();

            if (value.EndsWith("km") && double.TryParse(value[..^2].Trim(), out var kilometers))
            {
                return kilometers * 1000;
            }

            if (value.EndsWith("m") && double.TryParse(value[..^1].Trim(), out var meters))
            {
                return meters;
            }

            return double.TryParse(value, out var plainMeters) ? plainMeters : 0;
        }

        private static IEnumerable<ParsedFaq> ParseFaqs(IEnumerable<string> rawFaqs)
        {
            foreach (var rawFaq in rawFaqs.Where(f => !string.IsNullOrWhiteSpace(f)))
            {
                var parts = rawFaq.Split("|||", 2, StringSplitOptions.TrimEntries);
                if (parts.Length != 2 ||
                    string.IsNullOrWhiteSpace(parts[0]) ||
                    string.IsNullOrWhiteSpace(parts[1]))
                {
                    continue;
                }

                yield return new ParsedFaq
                {
                    Question = parts[0],
                    Answer = parts[1]
                };
            }
        }

        private sealed class ParsedNearbyPlace
        {
            public required string PlaceType { get; init; }
            public required string Name { get; init; }
            public required double DistanceInMeters { get; init; }
            public int? WalkingMinutes { get; init; }
            public string? Address { get; init; }
        }

        private sealed class ParsedFaq
        {
            public required string Question { get; init; }
            public required string Answer { get; init; }
        }
    }
}

using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Domain.Entities;
using Bed4Head.Infrastructure.Repositories;
namespace Bed4Head.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _db;
        public ReviewService(IUnitOfWork db)
        {
            _db = db;
        }
        public async Task<IEnumerable<ReviewDTO>> GetByHotelIdAsync(Guid hotelId)
        {
            var reviews = await _db.Reviews.GetAllAsync();
            return reviews.Where(r => r.HotelId == hotelId)
                          .OrderByDescending(r => r.CreatedAt)
                          .Select(r => MapToDto(r));
        }

        public async Task<IEnumerable<ReviewDTO>> GetRandomAsync(int count)
        {
            var reviews = await _db.Reviews.GetAllAsync();
            return reviews.Where(r => !string.IsNullOrWhiteSpace(r.Comment))
                          .OrderBy(_ => Guid.NewGuid())
                          .Take(NormalizeCount(count))
                          .Select(r => MapToDto(r));
        }

        public async Task<IEnumerable<ReviewDTO>> GetRandomByHotelIdAsync(Guid hotelId, int count)
        {
            var reviews = await _db.Reviews.GetAllAsync();
            return reviews.Where(r => r.HotelId == hotelId && !string.IsNullOrWhiteSpace(r.Comment))
                          .OrderBy(_ => Guid.NewGuid())
                          .Take(NormalizeCount(count))
                          .Select(r => MapToDto(r));
        }

        public async Task<IEnumerable<ReviewDTO>> GetRandomFromRandomHotelAsync(int count)
        {
            var reviews = (await _db.Reviews.GetAllAsync())
                .Where(r => !string.IsNullOrWhiteSpace(r.Comment))
                .ToList();

            var hotelId = reviews.Select(r => r.HotelId)
                                 .Distinct()
                                 .OrderBy(_ => Guid.NewGuid())
                                 .FirstOrDefault();

            if (hotelId == Guid.Empty)
            {
                return Enumerable.Empty<ReviewDTO>();
            }

            return reviews.Where(r => r.HotelId == hotelId)
                          .OrderBy(_ => Guid.NewGuid())
                          .Take(NormalizeCount(count))
                          .Select(r => MapToDto(r));
        }

        public async Task<HotelRatingDTO> GetHotelRatingAsync(Guid hotelId)
        {
            var reviews = await _db.Reviews.GetAllAsync();
            var hotelReviews = reviews.Where(r => r.HotelId == hotelId).ToList();
            var overallRating = CalculateRating(hotelReviews);

            return new HotelRatingDTO
            {
                HotelId = hotelId,
                OverallRating = overallRating,
                RatingLabel = GetRatingLabel(overallRating),
                ReviewsCount = hotelReviews.Count
            };
        }

        public async Task<ReviewDTO?> GetByIdAsync(Guid id)
        {
            var r = await _db.Reviews.GetByIdAsync(id);
            return r == null ? null : MapToDto(r);
        }
        public async Task CreateAsync(CreateReviewDTO dto, Guid hotelId)
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                Comment = dto.Comment ?? string.Empty,
                Title = dto.Title,
                CreatedAt = DateTime.UtcNow,
                StayedAt = dto.StayedAt,
                TripType = dto.TripType,
                IsVerifiedStay = dto.IsVerifiedStay,
                OverallScore = dto.OverallScore,
                Facilities = dto.Facilities,
                Staff = dto.Staff,
                Cleanliness = dto.Cleanliness,
                Comfort = dto.Comfort,
                Location = dto.Location,
                ValueForMoney = dto.ValueForMoney,
                UserId = dto.UserId,
                HotelId = hotelId,
            };
            await _db.Reviews.AddAsync(review);
            await _db.CompleteAsync();
            await RefreshHotelRatingAsync(hotelId);
        }
        public async Task UpdateAsync(ReviewDTO dto)
        {
            var review = await _db.Reviews.GetByIdAsync(dto.Id);
            if (review != null)
            {
                review.Comment = dto.Comment ?? string.Empty;
                review.Title = dto.Title;
                review.StayedAt = dto.StayedAt;
                review.TripType = dto.TripType;
                review.IsVerifiedStay = dto.IsVerifiedStay;
                review.OverallScore = dto.OverallScore;
                review.Facilities = dto.Facilities;
                review.Staff = dto.Staff;
                review.Cleanliness = dto.Cleanliness;
                review.Comfort = dto.Comfort;
                review.Location = dto.Location;
                review.ValueForMoney = dto.ValueForMoney;
                await _db.Reviews.UpdateAsync(review);
                await _db.CompleteAsync();
                await RefreshHotelRatingAsync(review.HotelId);
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            var review = await _db.Reviews.GetByIdAsync(id);
            var hotelId = review?.HotelId;

            await _db.Reviews.DeleteAsync(id);
            await _db.CompleteAsync();

            if (hotelId.HasValue)
            {
                await RefreshHotelRatingAsync(hotelId.Value);
            }
        }

        private async Task RefreshHotelRatingAsync(Guid hotelId)
        {
            var hotel = await _db.Hotels.GetByIdAsync(hotelId);
            if (hotel == null)
            {
                return;
            }

            var rating = await GetHotelRatingAsync(hotelId);
            hotel.OverallRating = rating.OverallRating;
            hotel.RatingLabel = rating.RatingLabel;
            hotel.ReviewsCount = rating.ReviewsCount;

            await _db.Hotels.UpdateAsync(hotel);
            await _db.CompleteAsync();
        }

        private static double CalculateRating(IReadOnlyCollection<Review> reviews)
        {
            if (reviews.Count == 0)
            {
                return 0;
            }

            return Math.Round(reviews.Average(r => r.OverallScore), 1);
        }

        private static string? GetRatingLabel(double rating)
        {
            if (rating <= 0)
            {
                return null;
            }

            if (rating >= 9)
            {
                return "Excellent";
            }

            if (rating >= 8)
            {
                return "Very good";
            }

            if (rating >= 7)
            {
                return "Good";
            }

            return "Review score";
        }

        private static int NormalizeCount(int count) => Math.Clamp(count, 1, 50);

        private static ReviewDTO MapToDto(Review r) => new ReviewDTO
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
            UserId = r.UserId,
            HotelId = r.HotelId,
            AuthorDisplayName = r.User?.DisplayName
        };
    }
}

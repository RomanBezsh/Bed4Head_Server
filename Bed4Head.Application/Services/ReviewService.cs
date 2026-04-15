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

        public async Task<ReviewDTO?> GetByIdAsync(Guid id)
        {
            var r = await _db.Reviews.GetByIdAsync(id);
            return r == null ? null : MapToDto(r);
        }

        public async Task CreateAsync(ReviewDTO dto)
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow,

                Facilities = dto.Facilities,
                Staff = dto.Staff,
                Cleanliness = dto.Cleanliness,
                Comfort = dto.Comfort,
                Location = dto.Location,
                ValueForMoney = dto.ValueForMoney,

                UserId = dto.UserId,
                HotelId = dto.HotelId
            };

            await _db.Reviews.AddAsync(review);
            await _db.CompleteAsync();
        }

        public async Task UpdateAsync(ReviewDTO dto)
        {
            var review = await _db.Reviews.GetByIdAsync(dto.Id);
            if (review != null)
            {
                review.Comment = dto.Comment;
                review.Facilities = dto.Facilities;
                review.Staff = dto.Staff;
                review.Cleanliness = dto.Cleanliness;
                review.Comfort = dto.Comfort;
                review.Location = dto.Location;
                review.ValueForMoney = dto.ValueForMoney;

                await _db.Reviews.UpdateAsync(review);
                await _db.CompleteAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Reviews.DeleteAsync(id);
            await _db.CompleteAsync();
        }

        private static ReviewDTO MapToDto(Review r) => new ReviewDTO
        {
            Id = r.Id,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt,
            Facilities = r.Facilities,
            Staff = r.Staff,
            Cleanliness = r.Cleanliness,
            Comfort = r.Comfort,
            Location = r.Location,
            ValueForMoney = r.ValueForMoney,
            UserId = r.UserId,
            HotelId = r.HotelId,
        };
    }
}


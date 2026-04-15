using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Domain.Entities;
using Bed4Head.Infrastructure.Repositories;

namespace Bed4Head.Application.Services
{
    public class HotelFaqService : IHotelFaqService
    {
        private readonly IUnitOfWork _db;

        public HotelFaqService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HotelFaqDTO>> GetByHotelIdAsync(Guid hotelId)
        {
            var faqs = await _db.HotelFaqs.GetAllAsync();

            return faqs.Where(f => f.HotelId == hotelId)
                       .OrderBy(f => f.DisplayOrder) 
                       .Select(f => MapToDto(f));
        }

        public async Task<HotelFaqDTO?> GetByIdAsync(Guid id)
        {
            var faq = await _db.HotelFaqs.GetByIdAsync(id);
            return faq == null ? null : MapToDto(faq);
        }

        public async Task CreateAsync(HotelFaqDTO dto)
        {
            var faq = new HotelFaq
            {
                Id = Guid.NewGuid(),
                Question = dto.Question,
                Answer = dto.Answer,
                DisplayOrder = dto.DisplayOrder,
                HotelId = dto.HotelId
            };

            await _db.HotelFaqs.AddAsync(faq);
            await _db.CompleteAsync();
        }

        public async Task UpdateAsync(HotelFaqDTO dto)
        {
            var faq = await _db.HotelFaqs.GetByIdAsync(dto.Id);
            if (faq != null)
            {
                faq.Question = dto.Question;
                faq.Answer = dto.Answer;
                faq.DisplayOrder = dto.DisplayOrder;

                await _db.HotelFaqs.UpdateAsync(faq);
                await _db.CompleteAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.HotelFaqs.DeleteAsync(id);
            await _db.CompleteAsync();
        }

        private static HotelFaqDTO MapToDto(HotelFaq f) => new HotelFaqDTO
        {
            Id = f.Id,
            Question = f.Question,
            Answer = f.Answer,
            DisplayOrder = f.DisplayOrder,
            HotelId = f.HotelId
        };
    }
}


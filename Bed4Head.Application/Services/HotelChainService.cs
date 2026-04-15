using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Domain.Entities;
using Bed4Head.Infrastructure.Repositories;

namespace Bed4Head.Application.Services
{
    public class HotelChainService : IHotelChainService
    {
        private readonly IUnitOfWork _db;

        public HotelChainService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HotelChainDTO>> GetAllAsync()
        {
            var chains = await _db.HotelChains.GetAllAsync();
            return chains.Select(c => MapToDto(c));
        }

        public async Task<HotelChainDTO?> GetByIdAsync(Guid id)
        {
            var c = await _db.HotelChains.GetByIdAsync(id);
            return c == null ? null : MapToDto(c);
        }

        public async Task CreateAsync(HotelChainDTO dto)
        {
            var chain = new HotelChain
            {
                Id = Guid.NewGuid(),
                Name = dto.Name
            };

            await _db.HotelChains.AddAsync(chain);
            await _db.CompleteAsync();
        }

        public async Task UpdateAsync(HotelChainDTO dto)
        {
            var chain = await _db.HotelChains.GetByIdAsync(dto.Id);
            if (chain != null)
            {
                chain.Name = dto.Name;

                await _db.HotelChains.UpdateAsync(chain);
                await _db.CompleteAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.HotelChains.DeleteAsync(id);
            await _db.CompleteAsync();
        }

        private static HotelChainDTO MapToDto(HotelChain c) => new HotelChainDTO
        {
            Id = c.Id,
            Name = c.Name,
            HotelsCount = c.Hotels?.Count ?? 0
        };
    }
}


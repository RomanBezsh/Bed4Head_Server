using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Bed4Head.DAL.Entities;
using Bed4Head.DAL.Repositories;

namespace Bed4Head.BLL.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IUnitOfWork _db;

        public AmenityService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<AmenityDTO>> GetAllAsync()
        {
            var items = await _db.Amenities.GetAllAsync();
            return items.Select(a => new AmenityDTO
            {
                Id = a.Id,
                Name = a.Name,
                Category = a.Category,
                IconKey = a.IconKey
            });
        }

        public async Task<AmenityDTO?> GetByIdAsync(Guid id)
        {
            var a = await _db.Amenities.GetByIdAsync(id);
            if (a == null) return null;

            return new AmenityDTO
            {
                Id = a.Id,
                Name = a.Name,
                Category = a.Category,
                IconKey = a.IconKey
            };
        }

        public async Task CreateAsync(AmenityDTO dto)
        {
            var amenity = new Amenity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Category = dto.Category,
                IconKey = dto.IconKey
            };

            await _db.Amenities.AddAsync(amenity);
            await _db.CompleteAsync();
        }

        public async Task UpdateAsync(AmenityDTO dto)
        {
            var amenity = await _db.Amenities.GetByIdAsync(dto.Id);
            if (amenity != null)
            {
                amenity.Name = dto.Name;
                amenity.Category = dto.Category;
                amenity.IconKey = dto.IconKey;

                await _db.Amenities.UpdateAsync(amenity);
                await _db.CompleteAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Amenities.DeleteAsync(id);
            await _db.CompleteAsync();
        }
    }
}
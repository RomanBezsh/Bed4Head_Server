using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Bed4Head.DAL.Entities;
using Bed4Head.DAL.Repositories;

namespace Bed4Head.BLL.Services
{
    public class HotelPhotoService : IHotelPhotoService
    {
        private readonly IUnitOfWork _db;

        public HotelPhotoService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HotelPhotoDTO>> GetAllAsync()
        {
            var photos = await _db.HotelPhotos.GetAllAsync();
            return photos.Select(p => MapToDto(p));
        }

        public async Task<HotelPhotoDTO?> GetByIdAsync(Guid id)
        {
            var p = await _db.HotelPhotos.GetByIdAsync(id);
            return p == null ? null : MapToDto(p);
        }

        public async Task<IEnumerable<HotelPhotoDTO>> GetByHotelIdAsync(Guid hotelId)
        {
            var all = await _db.HotelPhotos.GetAllAsync();
            return all.Where(p => p.HotelId == hotelId)
                      .OrderBy(p => p.DisplayOrder)
                      .Select(p => MapToDto(p));
        }

        public async Task CreateAsync(HotelPhotoDTO dto)
        {
            var photo = new HotelPhoto
            {
                Id = Guid.NewGuid(),
                Url = dto.Url,
                IsPrimary = dto.IsPrimary,
                DisplayOrder = dto.DisplayOrder,
                HotelId = dto.HotelId
            };

            await _db.HotelPhotos.AddAsync(photo);
            await _db.CompleteAsync();
        }

        public async Task UpdateAsync(HotelPhotoDTO dto)
        {
            var photo = await _db.HotelPhotos.GetByIdAsync(dto.Id);
            if (photo != null)
            {
                photo.Url = dto.Url;
                photo.DisplayOrder = dto.DisplayOrder;
                photo.IsPrimary = dto.IsPrimary;

                await _db.HotelPhotos.UpdateAsync(photo);
                await _db.CompleteAsync();
            }
        }

        public async Task SetPrimaryAsync(Guid photoId)
        {
            var photo = await _db.HotelPhotos.GetByIdAsync(photoId);
            if (photo == null) return;

            var hotelPhotos = (await _db.HotelPhotos.GetAllAsync())
                                .Where(p => p.HotelId == photo.HotelId);

            foreach (var p in hotelPhotos)
            {
                p.IsPrimary = (p.Id == photoId);
                await _db.HotelPhotos.UpdateAsync(p);
            }

            await _db.CompleteAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.HotelPhotos.DeleteAsync(id);
            await _db.CompleteAsync();
        }

        private static HotelPhotoDTO MapToDto(HotelPhoto p) => new HotelPhotoDTO
        {
            Id = p.Id,
            Url = p.Url,
            IsPrimary = p.IsPrimary,
            DisplayOrder = p.DisplayOrder,
            HotelId = p.HotelId
        };
    }
}
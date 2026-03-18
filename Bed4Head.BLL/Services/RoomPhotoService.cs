using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Bed4Head.DAL.Entities;
using Bed4Head.DAL.Repositories;

namespace Bed4Head.BLL.Services
{
    public class RoomPhotoService : IRoomPhotoService
    {
        private readonly IUnitOfWork _db;

        public RoomPhotoService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<RoomPhotoDTO>> GetByRoomIdAsync(Guid roomId)
        {
            var photos = await _db.RoomPhotos.GetAllAsync();
            return photos.Where(p => p.RoomId == roomId)
                         .Select(p => MapToDto(p));
        }

        public async Task<RoomPhotoDTO?> GetByIdAsync(Guid id)
        {
            var p = await _db.RoomPhotos.GetByIdAsync(id);
            return p == null ? null : MapToDto(p);
        }

        public async Task CreateAsync(RoomPhotoDTO dto)
        {
            var photo = new RoomPhoto
            {
                Id = Guid.NewGuid(),
                Url = dto.Url,
                IsPreview = dto.IsPreview,
                RoomId = dto.RoomId
            };

            await _db.RoomPhotos.AddAsync(photo);
            await _db.CompleteAsync();
        }

        public async Task UpdateAsync(RoomPhotoDTO dto)
        {
            var photo = await _db.RoomPhotos.GetByIdAsync(dto.Id);
            if (photo != null)
            {
                photo.Url = dto.Url;
                photo.IsPreview = dto.IsPreview;

                await _db.RoomPhotos.UpdateAsync(photo);
                await _db.CompleteAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.RoomPhotos.DeleteAsync(id);
            await _db.CompleteAsync();
        }

        public async Task SetPreviewAsync(Guid photoId)
        {
            var photo = await _db.RoomPhotos.GetByIdAsync(photoId);
            if (photo == null) return;

            var roomPhotos = await _db.RoomPhotos.GetAllAsync();
            var siblings = roomPhotos.Where(p => p.RoomId == photo.RoomId);

            foreach (var s in siblings)
            {
                s.IsPreview = (s.Id == photoId);
                await _db.RoomPhotos.UpdateAsync(s);
            }

            await _db.CompleteAsync();
        }

        private static RoomPhotoDTO MapToDto(RoomPhoto p) => new RoomPhotoDTO
        {
            Id = p.Id,
            Url = p.Url,
            IsPreview = p.IsPreview ?? false, 
            RoomId = p.RoomId
        };
    }
}
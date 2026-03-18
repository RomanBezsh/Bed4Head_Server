using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Bed4Head.DAL.Entities;
using Bed4Head.DAL.Repositories;

namespace Bed4Head.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _db;

        public UserService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _db.Users.GetAllAsync();
            return users.Select(u => MapToDto(u));
        }

        public async Task<UserDTO?> GetByIdAsync(Guid id)
        {
            var user = await _db.Users.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<UserDTO?> GetByEmailAsync(string email)
        {
            var users = await _db.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == email);
            return user == null ? null : MapToDto(user);
        }

        public async Task UpdateAsync(UserDTO dto)
        {
            var user = await _db.Users.GetByIdAsync(dto.Id);
            if (user != null)
            {
                user.DisplayName = dto.DisplayName;
                user.Phone = dto.Phone;
                user.BirthDate = dto.BirthDate;
                user.Country = dto.Country;
                user.City = dto.City;
                user.AvatarUrl = dto.AvatarUrl;
                user.TravelPurpose = dto.TravelPurpose;

                user.NewsSeasonalOffers = dto.NewsSeasonalOffers;
                user.NewsFavoriteCities = dto.NewsFavoriteCities;
                user.NewsAcrossWorld = dto.NewsAcrossWorld;
                user.NewsAffordableTravel = dto.NewsAffordableTravel;

                await _db.Users.UpdateAsync(user);
                await _db.CompleteAsync();
            }
        }

        public async Task UpdateNewsPreferencesAsync(Guid userId, bool seasonal, bool favorite, bool world, bool affordable)
        {
            var user = await _db.Users.GetByIdAsync(userId);
            if (user != null)
            {
                user.NewsSeasonalOffers = seasonal;
                user.NewsFavoriteCities = favorite;
                user.NewsAcrossWorld = world;
                user.NewsAffordableTravel = affordable;

                await _db.Users.UpdateAsync(user);
                await _db.CompleteAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.Users.DeleteAsync(id);
            await _db.CompleteAsync();
        }

        private static UserDTO MapToDto(User u) => new UserDTO
        {
            Id = u.Id,
            Email = u.Email,
            DisplayName = u.DisplayName,
            Phone = u.Phone,
            BirthDate = u.BirthDate,
            Country = u.Country,
            City = u.City,
            IsEmailConfirmed = u.IsEmailConfirmed ?? false,
            AvatarUrl = u.AvatarUrl,
            TravelPurpose = u.TravelPurpose,
            CreatedAt = u.CreatedAt,
            NewsSeasonalOffers = u.NewsSeasonalOffers ?? false,
            NewsFavoriteCities = u.NewsFavoriteCities ?? false,
            NewsAcrossWorld = u.NewsAcrossWorld ?? false,
            NewsAffordableTravel = u.NewsAffordableTravel ?? false
        };
    }
}
using Bed4Head.BLL.Interfaces;
using Bed4Head.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bed4Head.BLL.Extensions
{
    public static class AppServicesExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IHotelChainService, HotelChainService>();
            services.AddScoped<IHotelPhotoService, HotelPhotoService>();

            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoomPhotoService, RoomPhotoService>();

            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();

            services.AddScoped<IAmenityService, AmenityService>();
            services.AddScoped<INearbyPlaceService, NearbyPlaceService>();
            services.AddScoped<IHotelFaqService, HotelFaqService>(); 
        }
    }
}
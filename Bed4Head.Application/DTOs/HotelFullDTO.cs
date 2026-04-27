namespace Bed4Head.Application.DTOs
{
    public class HotelFullDTO
    {
        public required HotelDetailsDTO Hotel { get; set; }
        public List<AmenityDTO> Amenities { get; set; } = [];
        public List<HotelPhotoDTO> Photos { get; set; } = [];
        public List<NearbyPlaceDTO> NearbyPlaces { get; set; } = [];
        public List<HotelFaqDTO> Faqs { get; set; } = [];
        public List<ReviewDTO> Reviews { get; set; } = [];
        public List<RoomDTO> Rooms { get; set; } = [];
    }
}

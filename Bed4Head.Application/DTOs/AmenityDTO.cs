namespace Bed4Head.Application.DTOs
{
    public class AmenityDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; } 
        public required string IconKey { get; set; }  
    }
}


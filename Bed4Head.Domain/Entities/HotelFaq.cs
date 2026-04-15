using System;

namespace Bed4Head.Domain.Entities
{
    public class HotelFaq
    {
        public Guid Id { get; set; }

        public required string Question { get; set; }

        public required string Answer { get; set; }

        public int DisplayOrder { get; set; }

        public Guid HotelId { get; set; }
        public virtual Hotel Hotel { get; set; } = null!;
    }
}


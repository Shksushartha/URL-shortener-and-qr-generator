using System;
namespace urlShortener.Models
{
    public class Url
    {
        public Url()
        {
        }
        public Guid Id { get; set; }
        public string urlIdentifier { get; set; }

        public string originalUrl { get; set; } = null!;





    }
}


using AppAngular.Domain.Enums;

namespace AppAngular.Domain.Models
{
    public class Publication
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int StockAvailable { get; set; }

        public DateTime PublicationDate { get; set; }

        public StatusEnums StatusEnums { get; set; }

        public string UserId { get; set; }

        public int CategoryId { get; set; }

        public AspNetUsers AspNetUsers { get; set; }
        public Category Category { get; set; }

    }
}
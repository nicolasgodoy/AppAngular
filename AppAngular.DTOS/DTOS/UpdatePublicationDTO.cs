namespace AppAngular.DTOS.DTOS
{
    public class UpdatePublicationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int StockAvailable { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }

        public int CategoryId { get; set; }
    }
}

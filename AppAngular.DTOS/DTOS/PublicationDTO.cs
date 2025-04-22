namespace AppAngular.DTOS.DTOS
{
    public class PublicationDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int StockAvailable { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Status { get; set; }

        public AspNetUserDTO AspNetUsers { get; set; }

        public CategoryDTO Category { get; set; }
    }
}

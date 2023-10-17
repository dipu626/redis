namespace Caching.Application.Dtos
{
    public class ProductResponse
    {
        public int Id { get; set; } = 0;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int Stock { get; set; } = 0;
    }
}

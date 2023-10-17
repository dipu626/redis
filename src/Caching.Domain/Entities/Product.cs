using Base.Domain.Entities;

namespace Caching.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int Stock { get; set; } = 0;
    }
}

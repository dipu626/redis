using System.ComponentModel.DataAnnotations;

namespace Base.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}

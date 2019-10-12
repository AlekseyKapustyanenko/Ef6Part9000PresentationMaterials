using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModel.Models
{
    public class OrderBook
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}

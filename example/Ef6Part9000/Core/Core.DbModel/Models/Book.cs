using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModel.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<OrderBook> OrderBooks { get; set; }

        [ForeignKey(nameof(DiscountForBook))]
        public int? DiscountForBookId { get; set; }

        public DiscountForBook DiscountForBook { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Models
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

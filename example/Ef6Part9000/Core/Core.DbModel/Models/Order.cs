using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.DbModel.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public StatusEnum Status { get; set; }

        public string Notice { get; set; }

        public virtual ICollection<OrderBook> OrderBooks { get; set; }

        public bool IsDiscountOrder { get; set; }

        public int DiscountValue { get; set; }
    }
}

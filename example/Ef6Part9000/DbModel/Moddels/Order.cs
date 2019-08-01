using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Moddels
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public StatusEnum Status { get; set; }

        public string Notice { get; set; }

        public virtual ICollection<OrderBook> OrderBooks { get; set; }
    }
}

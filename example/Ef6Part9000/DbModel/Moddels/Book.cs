using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Moddels
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<OrderBook> OrderBooks { get; set; }

    }
}

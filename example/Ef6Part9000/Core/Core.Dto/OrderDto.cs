using Core.DbModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string Notice { get; set; }

        public IEnumerable<BookDto> DiscountBooks { get; set; }

        public IEnumerable<BookDto> SimpleBooks { get; set; }


        public StatusEnum Status { get; set; }
    }
}

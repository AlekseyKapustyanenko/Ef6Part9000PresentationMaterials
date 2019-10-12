using Core.DbModel.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.DbModel
{
    public class BookStoreContext:DbContext
    {


        public BookStoreContext()
        {
            
        }
        public BookStoreContext(DbContextOptions options):base(options)
        {
            
        }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Book> Books{ get; set; }

        public virtual DbSet<OrderBook> OrderBooks { get; set; }
       
        public virtual DbSet<DiscountForBook> DiscountForBooks { get; set; }
        
    }
}

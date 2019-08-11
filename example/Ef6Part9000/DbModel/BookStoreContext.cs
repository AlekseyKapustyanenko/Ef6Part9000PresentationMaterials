
using DbModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel
{
    public class BookStoreContext:DbContext
    {


        public BookStoreContext():base("StoreDb")
        {
            Database.SetInitializer<BookStoreContext>(new CreateDatabaseIfNotExists<BookStoreContext>());
        }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Book> Books{ get; set; }

        public virtual DbSet<OrderBook> OrderBooks { get; set; }
       
        public virtual DbSet<DiscountForBook> DiscountForBooks { get; set; }
    }
}

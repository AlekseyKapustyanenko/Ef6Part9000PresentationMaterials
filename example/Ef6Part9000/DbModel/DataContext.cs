using DbModel.Moddels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel
{
    public class DataContext:DbContext
    {


        public DataContext():base("StoreDb")
        {
            Database.SetInitializer<DataContext>(new CreateDatabaseIfNotExists<DataContext>());
        }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Book> Books{ get; set; }

        public virtual DbSet<OrderBook> OrderBooks { get; set; }

    }
}

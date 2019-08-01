using DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef6Part9000
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var c=new DataContext()) {
                var x= c.Books.ToList();
            };
        }
    }
}

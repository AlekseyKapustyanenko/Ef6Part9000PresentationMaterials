using DbModel;
using DbModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveDemo4
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var context = new BookStoreContext())
            {
                context.Orders.ToList();
            }

            while (true)
            {
                Console.Write("Выбирете пример 1 - исходный, 2 - оптимизированный: ");
                var number = Console.ReadLine();
                switch (number)
                {
                    case "1":
                        {
                            var stopwatch = Stopwatch.StartNew();
                            var sql = BadQuery();
                            stopwatch.Stop();
                            Clipboard.SetText(sql);
                            Console.WriteLine($"Исходный запрос выполнился за {stopwatch.ElapsedMilliseconds} мс");
                            break;
                        }
                    case "2":
                        {
                            var stopwatch = Stopwatch.StartNew();
                            var sql = GoodQuery();
                            stopwatch.Stop();
                            Clipboard.SetText(sql);
                            Console.WriteLine($"Оптимизированный запрос выполнился за {stopwatch.ElapsedMilliseconds} мс");
                            break;
                        }
                }
            }

        }

        static string BadQuery()
        {
            
            using (var context = new BookStoreContext())
            {
                var query = context.Orders.Where(o =>o.Status==StatusEnum.Cancelled && o.IsDiscountOrder);
                var result = query.ToList();
                return ((DbQuery<Order>)query).Sql;
            }
        }

        static string GoodQuery()
        {
          
            using (var context = new BookStoreContext())
            {
                var cancelledStatusValue = StatusEnum.Cancelled;
                var trueValue = true;
                var query = context.Orders.Where(o => o.Status == cancelledStatusValue && o.IsDiscountOrder== trueValue);
                var result = query.ToList();
                return ((DbQuery<Order>)query).Sql;
            }
        }
    }
}

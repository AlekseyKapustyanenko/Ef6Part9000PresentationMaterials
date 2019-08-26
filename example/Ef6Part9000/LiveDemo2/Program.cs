using DbModel;
using DbModel.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveDemo2
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
                Console.Write("Выбирете пример 1 - исходный, 2 - замена на \"или\", 3 - постороение Expression 4 - LinqKit,: ");
                var number = Console.ReadLine();
                switch (number)
                {
                    case "1":
                        {
                            var stopwatch = Stopwatch.StartNew();
                            var sql = BadQuery();
                            stopwatch.Stop();
                            Clipboard.SetText(sql);
                            Console.WriteLine($"Плохой исходный выполнился за {stopwatch.ElapsedMilliseconds} мс");
                            break;
                        }
                    case "2":
                        {
                            var stopwatch = Stopwatch.StartNew();
                            var sql = NotGoodQuery();
                            stopwatch.Stop();
                            Clipboard.SetText(sql);
                            Console.WriteLine($"Запрос с заменой на \"или\" выполнился за {stopwatch.ElapsedMilliseconds} мс");
                            break;
                        }
                    case "3":
                        {
                            var stopwatch = Stopwatch.StartNew();
                            var sql = NotBadQuery();
                            stopwatch.Stop();
                            Clipboard.SetText(sql);
                            Console.WriteLine($"Запрос с построением выражений выполнился за {stopwatch.ElapsedMilliseconds} мс");
                            break;
                        }
                    case "4":
                        {
                            var stopwatch = Stopwatch.StartNew();
                            var sql = GoodQuery();
                            stopwatch.Stop();
                            Clipboard.SetText(sql);
                            Console.WriteLine($"Запрос с LinqKit выполнился за {stopwatch.ElapsedMilliseconds} мс");
                            break;
                        }
                }
            }

        }

        static string BadQuery()
        {
            var statusValues = new StatusEnum[] { StatusEnum.New, StatusEnum.Processed };
            using (var context = new BookStoreContext())
            {
                var query = context.Orders.Where(o => statusValues.Contains(o.Status));
                var result = query.ToList();
                return ((DbQuery<Order>)query).Sql;
            }
        }

        static string NotGoodQuery()
        {
            var statusValues = new StatusEnum[] { StatusEnum.New, StatusEnum.Processed };
            using (var context = new BookStoreContext())
            {
                var el1 = statusValues[0];
                var el2 = statusValues[1];
                var query = context.Orders.Where(o =>el1 ==o.Status ||el2 ==o.Status);
                var result = query.ToList();
                return ((DbQuery<Order>)query).Sql;
            }
        }

        static string NotBadQuery()
        {
            var statusValues = new StatusEnum[] { StatusEnum.New, StatusEnum.Processed };
            var expressionParameter = Expression.Parameter(typeof(Order), "o");
            var expressionMember = Expression.PropertyOrField(expressionParameter, "Status");
            Expression bodyExpr= Expression.Constant(false, typeof(bool));
            int n = 0;
            foreach(var status in statusValues)
            {
                var exprConst = Expression.Constant(status, typeof(StatusEnum));
                
                var exprVar=Expression.Property(Expression.Constant(new { Value = status }), $"Value");
                var expEq = Expression.Equal(expressionMember, exprVar);
                bodyExpr = Expression.Or(bodyExpr, expEq);
                n++;
            }
            var expressionLambda = Expression.Lambda<Func<Order, bool>>(bodyExpr, expressionParameter);

            using (var context = new BookStoreContext())
            {
                var query = context.Orders.Where(expressionLambda);
                var result = query.ToList();
                return ((DbQuery<Order>)query).Sql;
            }
        }

        static string GoodQuery()
        {
            var statusValues = new StatusEnum[] { StatusEnum.New, StatusEnum.Processed };          
            using (var context = new BookStoreContext())
            {         
                var predicate = PredicateBuilder.False<Order>();
                predicate = statusValues.Aggregate(predicate, (a, v) => a.Or(o => o.Status == v));
                var query = context.Orders.Where(predicate);
                var result = query.ToList();
                return ((DbQuery<Order>)query).Sql;
            }
        }
    }
}

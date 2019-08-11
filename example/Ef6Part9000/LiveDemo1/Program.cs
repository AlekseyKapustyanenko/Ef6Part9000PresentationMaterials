using AutoMapper;
using DbModel;
using DbModel.Models;
using Dto;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveDemo1
{
    
    class Program
    {
        const int maxDiscountValue = 5;

        [STAThread]
        static void Main(string[] args)
        {
            using (var context = new BookStoreContext())
            {
                context.Orders.ToList();
            }

            while (true)
            {
                Console.Write("Выбирете пример 1 - плохой, 2 - хороший: ");
                var number = Console.ReadLine();
                switch (number)
                {
                    case "1":
                        {
                            var stopwatch = Stopwatch.StartNew();
                            var sql = BadQuery();
                            stopwatch.Stop();                       
                            Clipboard.SetText(sql);
                            Console.WriteLine($"Плохой запрос выполнился за {stopwatch.ElapsedMilliseconds}");
                            break;
                        }
                    case "2":
                        {
                            var stopwatch = Stopwatch.StartNew();
                            var sql = GoodQuery();
                            stopwatch.Stop();                            
                            Clipboard.SetText(sql);
                            Console.WriteLine($"Хороший запрос выполнился за {stopwatch.ElapsedMilliseconds}");
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
                var ordersWithDiscountForBook = context.Orders
                    .Where(o => !o.IsDiscountOrder && o.OrderBooks
                        .Any(ob => ob.Book.DiscountForBookId.HasValue && ob.Book.DiscountForBook.DiscountValue > maxDiscountValue));

                var ordersWithCommonDiscounts = context.Orders.Where(o => o.IsDiscountOrder && o.DiscountValue > maxDiscountValue);
                var concatQuery = ordersWithDiscountForBook.Concat(ordersWithCommonDiscounts);

                var mappedQuery = concatQuery.Select(o => new OrderDto
                {
                    Id = o.Id,
                    Notice = o.Notice,
                    Status = o.Status,
                    DiscountBooks =
                     o.OrderBooks.Where(ob => ob.Order.IsDiscountOrder || (ob.Book.DiscountForBookId.HasValue && ob.Book.DiscountForBook.DiscountValue > maxDiscountValue)).Select(ob => new BookDto
                     {
                         Id = ob.BookId,
                         Name = ob.Book.Name,
                         DiscountValue = o.IsDiscountOrder ? o.DiscountValue : ob.Book.DiscountForBookId.HasValue ? ob.Book.DiscountForBook.DiscountValue : 0
                     }),
                    SimpleBooks = o.OrderBooks.Where(ob => !ob.Order.IsDiscountOrder && (!ob.Book.DiscountForBookId.HasValue || ob.Book.DiscountForBookId.HasValue && ob.Book.DiscountForBook.DiscountValue <= 5)).Select(ob => new BookDto
                    {
                        Id = ob.BookId,
                        Name = ob.Book.Name,
                        DiscountValue = o.IsDiscountOrder ? o.DiscountValue : ob.Book.DiscountForBookId.HasValue ? ob.Book.DiscountForBook.DiscountValue : 0
                    }),

                });

                var filteredQuery = mappedQuery.Where(o => statusValues.Contains(o.Status));
                var result = filteredQuery.ToList();
                return ((DbQuery<OrderDto>)filteredQuery).Sql;
            }
        }

        static string GoodQuery()
        {
            var statusValues = new StatusEnum[] { StatusEnum.New, StatusEnum.Processed };
            var fiveValue = maxDiscountValue;
            var trueValue = true;
            var falseValue = false;
            var zeroValue = 0;
            using (var context = new BookStoreContext())
            {
                var ordersWithDiscountForBook = context.Orders
                    .Where(o => o.IsDiscountOrder == falseValue && o.OrderBooks
                        .Any(ob => ob.Book.DiscountForBookId.HasValue && ob.Book.DiscountForBook.DiscountValue > fiveValue));

                var ordersWithCommonDiscounts = context.Orders.Where(o => o.IsDiscountOrder == trueValue && o.DiscountValue > fiveValue);
                var concatQuery = ordersWithDiscountForBook.Concat(ordersWithCommonDiscounts);

                var mappedQuery = concatQuery.Select(o => new OrderDto
                {
                    Id = o.Id,
                    Notice = o.Notice,
                    Status = o.Status,
                    DiscountBooks =
                     o.OrderBooks.Where(ob => ob.Order.IsDiscountOrder == trueValue || (ob.Book.DiscountForBookId.HasValue && ob.Book.DiscountForBook.DiscountValue > fiveValue)).Select(ob => new BookDto
                     {
                         Id = ob.BookId,
                         Name = ob.Book.Name,
                         DiscountValue = o.IsDiscountOrder == trueValue ? o.DiscountValue : ob.Book.DiscountForBookId.HasValue ? ob.Book.DiscountForBook.DiscountValue : zeroValue
                     }),
                    SimpleBooks = o.OrderBooks.Where(ob => ob.Order.IsDiscountOrder == falseValue && (!ob.Book.DiscountForBookId.HasValue || ob.Book.DiscountForBookId.HasValue && ob.Book.DiscountForBook.DiscountValue <= fiveValue)).Select(ob => new BookDto
                    {
                        Id = ob.BookId,
                        Name = ob.Book.Name,
                        DiscountValue = o.IsDiscountOrder == trueValue ? o.DiscountValue : ob.Book.DiscountForBookId.HasValue ? ob.Book.DiscountForBook.DiscountValue : zeroValue
                    }),

                });
                var predicate = PredicateBuilder.False<OrderDto>();
                predicate = statusValues.Aggregate(predicate, (a, v) => a.Or(o => o.Status == v));
                var filteredQuery = mappedQuery.Where(predicate);
                var result = filteredQuery.ToList();
                return ((DbQuery<OrderDto>)filteredQuery).Sql;
            }
        }
    }
}

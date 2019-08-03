using DbModel;
using QueryStore;
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
            using (var context=new BookStoreContext()) {
                context.Database.Log = Console.WriteLine;
                var testQueriesSource = new TestQueriesRepository(context);
                Console.WriteLine("Запрос 1 с contains.");
                testQueriesSource.BadQueryWithContainsStatement();
                Console.WriteLine("Запрос 1 с использованием LinqKit.");
                testQueriesSource.GoodQueryWithContainsStatement();
                Console.WriteLine("Запрос 2 с Any.");
                testQueriesSource.BadQueryWithAnyStatement();
                Console.WriteLine("Запрос 2 с LinqKit.");
                testQueriesSource.GoodQueryWithAnyStatement();
                Console.WriteLine("Запрос 3 с \"константой\".");
                testQueriesSource.BadQueryWithConstatnt();
                Console.WriteLine("Запрос 3 с \"переменной\".");
                testQueriesSource.GoodQueryWithConstatnt();

                Console.ReadLine();

            };
        }
    }
}

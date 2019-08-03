using DbModel;
using DbModel.Moddels;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryStore
{
    public class TestQueriesRepository 
    {
        private readonly BookStoreContext _dataContext;

        private readonly int[] _bookIds = new []{ 1, 2, 3 };

        public TestQueriesRepository(BookStoreContext dataContext)
        {
            _dataContext =dataContext;
        }

        public void BadQueryWithContainsStatement()
        {
            _dataContext.Books.Where(b => _bookIds.Contains(b.Id)).ToList();
        }

        public void BadQueryWithAnyStatement()
        {
            _dataContext.Orders.Where(o => o.OrderBooks.Any(ob=> _bookIds.Any(id=>id==ob.BookId))).ToList();
        }

        public void BadQueryWithConstatnt()
        {
            _dataContext.Orders.Where(o=>o.Status==StatusEnum.Cancelled).ToList();
        }

        public void GoodQueryWithConstatnt()
        {
            var statusCancelled = StatusEnum.Cancelled;
            _dataContext.Orders.Where(o => o.Status == statusCancelled).ToList();
        }

        public void GoodQueryWithAnyStatement()
        {
            var query = PredicateBuilder.New<OrderBook>(false);
            query = _bookIds.Aggregate(query, (a, v) => a.Or(ob => ob.BookId == v));
            _dataContext.Orders.Where(o => o.OrderBooks.AsQueryable().Any(query)).ToList();
        }

        public void GoodQueryWithContainsStatement()
        {
            var query = PredicateBuilder.New<Book>(false);
            query = _bookIds.Aggregate(query, (a, v) => a.Or(ob => ob.Id == v));
            _dataContext.Books.Where(query).ToList();
        }
    }
}

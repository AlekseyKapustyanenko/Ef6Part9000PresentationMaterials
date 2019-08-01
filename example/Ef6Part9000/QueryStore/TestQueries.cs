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
    public class TestQueries : IDisposable
    {
        private readonly DataContext _dataContext;

        private readonly int[] _bookIds = new []{ 1, 2, 3 };

        public TestQueries()
        {
            _dataContext = new DataContext();
        }

        public void BadQueryWithInStatement()
        {
            _dataContext.Orders.Where(o => o.OrderBooks.Any(ob => _bookIds.Any(id => id == ob.BookId))).ToList();
        }

        public void BadQueryWithConstatnt()
        {
            _dataContext.Orders.Where(o=>o.Status==StatusEnum.New).ToList();
        }

        public void GoodQueryWithConstatnt()
        {
            var statusNew= StatusEnum.New;
            _dataContext.Orders.Where(o => o.Status == statusNew).ToList();
        }

        public void GoodQueryWithInStatement()
        {
            var query = PredicateBuilder.New<OrderBook>(false);
            query = _bookIds.Aggregate(query, (a, v) => a.Or(ob => ob.BookId == v));
            _dataContext.Orders.Where(o => o.OrderBooks.Any(query)).ToList();
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}

using BookShoppingMVCUI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingMVCUI.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StockRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Stock?> GetStockByBookId(int bookId)
        {
            return await _dbContext.Stocks.FirstOrDefaultAsync(s => s.BookId == bookId);
        }

        public async Task ManageStock(StockDTO stockToManage)
        {
            var existingStock = await GetStockByBookId(stockToManage.BookId);
            if (existingStock is null)
            {
                var stock = new Stock
                {
                    BookId = stockToManage.BookId,
                    Quantity = stockToManage.Quantity,
                };
                _dbContext.Stocks.Add(stock);
            }
            else
            {
                existingStock.Quantity = stockToManage.Quantity;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "")
        {
            var stocks = await (from book in _dbContext.Books
                                join stock in _dbContext.Stocks
                                on book.Id equals stock.BookId
                                into book_stock
                                from bookStock in book_stock.DefaultIfEmpty()
                                where string.IsNullOrWhiteSpace(sTerm) || book.Name.ToLower().Contains(sTerm.ToLower())
                                select new StockDisplayModel
                                {
                                    BookId = book.Id,
                                    BookName = book.Name,
                                    Quantity = bookStock == null ? 0 : bookStock.Quantity
                                }).ToListAsync();
            return stocks;
        }
    }
}

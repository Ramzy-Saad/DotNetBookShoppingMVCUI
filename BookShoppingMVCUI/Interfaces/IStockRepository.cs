namespace BookShoppingMVCUI.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock?> GetStockByBookId(int bookId);
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task ManageStock(StockDTO stockToManage);
    }
}
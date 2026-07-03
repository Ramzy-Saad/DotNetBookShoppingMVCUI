namespace BookShoppingMVCUI.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<TopNSoldBookModel>> GetTopNSoldBooksAsync(DateTime startDate, DateTime endDate);
    }
}
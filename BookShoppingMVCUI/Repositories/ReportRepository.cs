using BookShoppingMVCUI.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingMVCUI.Repositories
{
    public class ReportRepository : IReportRepository
    {

        private readonly ApplicationDbContext _dbcontext;

        public ReportRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<TopNSoldBookModel>> GetTopNSoldBooksAsync(DateTime startDate, DateTime endDate)
        {
            var startDateParam = new SqlParameter("@StartDate", startDate);
            var endDateParam = new SqlParameter("@EndDate", endDate);

            var topFiveSoldBooks = await _dbcontext.Database.SqlQueryRaw<TopNSoldBookModel>
                ("exec Usp_GetTopNSellingBooksByDate @startDate, @endDate", startDateParam, endDateParam)
                .ToListAsync();
            return topFiveSoldBooks;
        }
    }
}


using Microsoft.EntityFrameworkCore;

namespace BookShoppingMVCUI.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public HomeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Book>> DisplayBooks(string sTerm="",int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Book> books =  await (from book in _dbContext.Books
                          join Genre in _dbContext.Genres
                          on book.GenreId equals Genre.Id
                          where string.IsNullOrWhiteSpace(sTerm) || (book!=null && book.Name.ToLower().StartsWith(sTerm))
                          select new Book
                          {
                              Id = book.Id,
                              Name = book.Name,
                              Image = book.Image,
                              AuthorName = book.AuthorName,
                              GenreId = book.GenreId,
                              GenreName = Genre.Name,
                              Price = book.Price,
                          }
                          ).ToListAsync();
            if (genreId>0)
            {
                books = books.Where(b=>b.GenreId==genreId).ToList();
            }
            return books;
        }
    
        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _dbContext.Genres.ToListAsync();
        }
    }
}

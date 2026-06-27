
using Microsoft.EntityFrameworkCore;

namespace BookShoppingMVCUI.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GenreRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext; 
        }
        public async Task AddGenre(Genre genre)
        {
            _dbContext.Add(genre);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateGenre(Genre genre)
        {
            _dbContext.Update(genre);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteGenre(Genre genre)
        {
            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();
           
        }

        public async Task<Genre?> GetGenreById(int id)
        {
            return await _dbContext.Genres.FindAsync(id);
        }

        public async Task<IEnumerable<Genre>> getGenres()
        {
            return await _dbContext.Genres.ToListAsync();
        }

    }
}

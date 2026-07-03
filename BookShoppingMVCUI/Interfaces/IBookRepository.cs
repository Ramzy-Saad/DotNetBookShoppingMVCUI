namespace BookShoppingMVCUI.Interfaces
{
    public interface IBookRepository
    {
        Task AddBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(Book book);
        Task<Book?> GetBookById(int id);
        Task<IEnumerable<Book>> getBooks();

    }
}

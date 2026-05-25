namespace BookShoppingMVCUI.Models.DTOs
{
    public class HomeDTO
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Genre> Genres { get; set; }

        public string Sterm { get; set; }
        public int GenreId { get; set; }


    }
}

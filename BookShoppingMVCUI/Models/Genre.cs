using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingMVCUI.Models
{

    [Table("Genre")]
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}

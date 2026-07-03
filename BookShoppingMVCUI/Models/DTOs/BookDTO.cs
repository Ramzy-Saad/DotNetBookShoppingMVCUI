using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookShoppingMVCUI.Models.DTOs
{
    public class BookDTO
    {
        public int id{ get; set; }
        [Required]
        [MaxLength(40)]
        public string BookName{ get; set; }
        [Required]
        [MaxLength(40)]
        public string? AuthorName { get; set; }

        public string? Image { get; set; }
        public double Price { get; set; }

        [Required]
        public int GenreId { get; set; }
        public int Quantity { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }


    }
}

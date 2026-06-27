using System.ComponentModel.DataAnnotations;

namespace BookShoppingMVCUI.Models.DTOs
{
    public class GenreDTO 
    {
        public int id{ get; set; }
        [Required]
        [MaxLength(40)]
        public string GenreName{ get; set; }


    }
}

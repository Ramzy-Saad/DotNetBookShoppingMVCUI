using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingMVCUI.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        public int Id { get; set; }
        [Required]
        public string userId { get; set; }
        public bool IsDeleted { get; set; }= false;

        public ICollection<CartDetail> CartDetails { get; set; }
    }
}

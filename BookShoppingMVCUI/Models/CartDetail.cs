using System.ComponentModel.DataAnnotations;

namespace BookShoppingMVCUI.Models
{
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        [Required]
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public Book Book{ get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace BookShoppingMVCUI.Models.DTOs
{
    public class CheckoutModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? MobileNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Address { get; set; }
        [Required]
        public string? PaymentMethod { get; set; }
    }
}

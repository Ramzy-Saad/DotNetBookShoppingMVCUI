using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingMVCUI.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int Id { get; set; }
        [Required]
        public int StatusID { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; }
    }
}

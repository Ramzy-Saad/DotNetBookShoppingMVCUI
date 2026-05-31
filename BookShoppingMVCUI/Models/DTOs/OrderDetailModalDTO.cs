namespace BookShoppingMVCUI.Models.DTOs
{
    public class OrderDetailModalDTO
    {
        public string DivID { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}

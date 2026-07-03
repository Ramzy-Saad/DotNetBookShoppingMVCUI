namespace BookShoppingMVCUI.Models.DTOs;

public record TopNSoldBookModel( string Name,string AuthorName, int TotalUnitSold );
public record TopNSoldBooksVm( DateTime StartDate, DateTime EndDate, IEnumerable<TopNSoldBookModel> TopNSoldBooks);


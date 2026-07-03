namespace BookShoppingMVCUI.Interfaces
{
    public interface IFileService
    {
        void DeleteImage(string fileName);
        Task<string> SaveImage(IFormFile file, string[] allowedExtensions);
    }
}

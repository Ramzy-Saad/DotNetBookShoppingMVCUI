using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingMVCUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository    )
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string sterm="", int genreId=0)
        {
            IEnumerable<Book> books = await _homeRepository.DisplayBooks( sterm , genreId);
            IEnumerable<Genre> genres = await _homeRepository.Genres();
            HomeDTO homeDTO = new HomeDTO()
            {
                Books = books,
                Genres = genres,
                Sterm = sterm,
                GenreId = genreId
            };
            return View(homeDTO);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

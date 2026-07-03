using BookShoppingMVCUI.Interfaces;
using BookShoppingMVCUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingMVCUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class GenreController : Controller
    {
        private IGenreRepository _genreRepo;

        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepo = genreRepository;
        }

        public async Task<IActionResult> Index()
        {
            var genre = await _genreRepo.getGenres();
            return View(genre);
        }

        public IActionResult AddGenre()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddGenre(GenreDTO genre)
        {
            if (!ModelState.IsValid)
            {
                return View(genre);
            }
            try
            {
                var genreToAdd = new Genre
                {
                    Name = genre.GenreName,
                    Id = genre.id
                };
                await _genreRepo.AddGenre(genreToAdd);
                TempData["successMessage"] = "Genre added successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex) { 
                TempData["errorMessage"] = "Genre couldn't be added.";
                return View(genre);
            }
        }

        public async Task<IActionResult> UpdateGenre(int id)
        {
            var genre = await _genreRepo.GetGenreById(id);
            if(genre is null)
            {
                throw new InvalidOperationException($"Genre with id {id} not found");
            }
            var genreToUpdate = new GenreDTO
            {
                id = genre.Id,
                GenreName = genre.Name
            };
            return View(genreToUpdate);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateGenre(GenreDTO genreToUpdate)
        {
            if (!ModelState.IsValid) { 
                return View(genreToUpdate);
            }
            try
            {
                var genre = new Genre
                {
                    Name = genreToUpdate.GenreName,
                    Id = genreToUpdate.id
                };
                await _genreRepo.UpdateGenre(genre);
                TempData["successMessage"] = "Genre updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception )
            {
                TempData["errorMessage"] = "Genre couldn't be updated.";
                return View(genreToUpdate);
            }
        }

        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreRepo.GetGenreById(id);
            if (genre is null)
            {
                throw new InvalidOperationException($"Gener with id {id} not exist.");
            }
            await _genreRepo.DeleteGenre(genre);
            return RedirectToAction("Index");
        }

    }
}

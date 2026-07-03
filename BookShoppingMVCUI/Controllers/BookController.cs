using BookShoppingMVCUI.Interfaces;
using BookShoppingMVCUI.Models;
using BookShoppingMVCUI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace BookShoppingMVCUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IFileService _fileService;
        private readonly IGenreRepository _genreRepository;
        private readonly IStockRepository _stockRepository;


        public BookController(IBookRepository bookRepository, IStockRepository stockRepository, IGenreRepository genreRepository, IFileService fileService)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _fileService = fileService; 
            _stockRepository = stockRepository;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.getBooks();
            return View(books);
        }

        public async Task<IActionResult> AddBook()
        {
            var genreSelectList = ( await _genreRepository.getGenres())
                .Select(genre => new SelectListItem
                {
                    Text = genre.Name,
                    Value = genre.Id.ToString()
                });
            BookDTO bookToAdd = new() { GenreList = genreSelectList };
            return View(bookToAdd);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(BookDTO bookToAdd)
        {
            var genreSelectList = (await _genreRepository.getGenres())
                .Select(genre => new SelectListItem
                {
                    Text = genre.Name,
                    Value = genre.Id.ToString()
                });
            bookToAdd.GenreList = genreSelectList;
            if(!ModelState.IsValid)
            {
                return View(bookToAdd);
            }
            try
            {
                if (bookToAdd.ImageFile != null)
                {
                    string[] allowExtensions = [".jpeg", ".png", "jpg"];
                    string imageName = await _fileService.SaveImage(bookToAdd.ImageFile, allowExtensions);
                    bookToAdd.Image = imageName;
                }
                Book book = new()
                {
                    Name = bookToAdd.BookName,
                    AuthorName = bookToAdd.AuthorName,
                    Image = bookToAdd.Image,
                    GenreId = bookToAdd.GenreId,
                    Price = bookToAdd.Price
                };
                await _bookRepository.AddBook(book);
                var stock = new StockDTO
                {
                    BookId = book.Id,
                    Quantity = bookToAdd.Quantity
                };
                await _stockRepository.ManageStock(stock);
                TempData["successMessage"] = "Book is added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex) {
                TempData["errorMessage"]= ex.ToString();
                return View(bookToAdd);
            }
            catch (FileNotFoundException ex) {
                TempData["errorMessage"]= ex.ToString();
                return View(bookToAdd);
            }
            catch (Exception ex) {
                TempData["errorMessage"]= "Error in saving data";
                return View(bookToAdd);
            }
        }


        public async Task<IActionResult> UpdateBook(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null) {
                TempData["errorMessage"] = $"Book with id {id} not exist.";
                return RedirectToAction(nameof(Index));
            }
            var genreSelectList = (await _genreRepository.getGenres())
                .Select(genre => new SelectListItem
                {
                    Text = genre.Name,
                    Value = genre.Id.ToString()
                });
            BookDTO bookToUpdate = new()
            {
                id = book.Id,
                GenreList = genreSelectList,
                BookName = book.Name,
                AuthorName = book.AuthorName,
                GenreId = book.GenreId,
                Price = book.Price,
                Image = book.Image,
                Quantity = book.Stock.Quantity
            };
            return View(bookToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBook(BookDTO bookToUpdate)
        {
            var genreSelectList = (await _genreRepository.getGenres())
                .Select(genre => new SelectListItem
                {
                    Text = genre.Name,
                    Value = genre.Id.ToString(),
                    Selected = genre.Id == bookToUpdate.GenreId
                });
            bookToUpdate.GenreList = genreSelectList;
            if (!ModelState.IsValid)
            {
                return View(bookToUpdate);
            }
            try
            {
                string oldImage = "";
                if(bookToUpdate.ImageFile != null)
                {
                    string[] allowExtensions = [".jpeg", ".png", "jpg"];
                    string imageName = await _fileService.SaveImage(bookToUpdate.ImageFile, allowExtensions);
                    oldImage = bookToUpdate.Image;
                    bookToUpdate.Image = imageName;
                }
                Book book = new()
                {
                    Id = bookToUpdate.id,
                    Name = bookToUpdate.BookName,
                    AuthorName = bookToUpdate.AuthorName,
                    Price = bookToUpdate.Price,
                    GenreId = bookToUpdate.GenreId,
                    Image = bookToUpdate.Image,
                };
                await _bookRepository.UpdateBook(book);
                var stock = new StockDTO
                {
                    BookId = bookToUpdate.id,
                    Quantity = bookToUpdate.Quantity
                };
                await _stockRepository.ManageStock(stock);
                if (! string.IsNullOrWhiteSpace(oldImage))
                {
                    _fileService.DeleteImage(oldImage);
                }
                TempData["successMessage"] = "Book is updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.ToString();
                return View(bookToUpdate);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.ToString();
                return View(bookToUpdate);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Error in updating data";
                return View(bookToUpdate);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var book = await _bookRepository.GetBookById(id);
                if (book == null)
                {
                    TempData["errorMessgae"] = $"Book with id: {id} not exist.";
                }
                await _bookRepository.DeleteBook(book);
                if(!string.IsNullOrWhiteSpace(book.Image))
                {
                    _fileService.DeleteImage(book.Image);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.ToString();
                return View();
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.ToString();
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Error in delete data";
                return View();
            }

        }
    }
}

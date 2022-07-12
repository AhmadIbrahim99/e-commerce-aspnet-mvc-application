using E_Commerce.eTickets.WebSite.data;
using E_Commerce.eTickets.WebSite.Models;
using eTickets.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;
        public MoviesController(IMoviesService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index() => View(await _service.GetAll(n => n.Cinema));

        //GET: Movies/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var data = await _service.GetMovieById(id);
            if (data == null) return View("NotFound");
            return View(data);
        }

        //GET: Movies/Create
        public async Task<IActionResult> Create()
        {
            var data = await _service.GetNewMovieDropDownsVMsValues();
            if (data == null) return View("NotFound");
            // we are creating List Items And Save it To View Bag (The Data, Pass BackGround Id, Present The Name ForeGround)
            ViewBag.Cinemas = new SelectList(data.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(data.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(data.Actors, "Id", "FullName");
            return View();
        }
        //POST: Movies/Create
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> ConfirmCreate(NewMovieVM movie)
        {
            var data = await _service.GetNewMovieDropDownsVMsValues();

            ViewBag.Cinemas = new SelectList(data.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(data.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(data.Actors, "Id", "FullName");
            if (!ModelState.IsValid)
                return View(movie);

            await _service.AddNewMovie(movie);
            return RedirectToAction(nameof(Index));
        }

        //GET: Movies/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _service.GetMovieById(id);
            var data = await _service.GetNewMovieDropDownsVMsValues();
            if (movie == null || data == null) return View("NotFound");
            var response = new NewMovieVM()
            {
                Id = id,
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                StartDate = movie.StartDate,
                EndDate = movie.EndDate,
                ImageURL = movie.ImageURL,
                MovieCategory = movie.MovieCategory,
                CinemaId = movie.CinemaId,
                ProducerId = movie.ProducerId,
                ActorsIds = movie.Actors_Movies.Select(a => a.ActorId).ToList(),
            };
            // we are creating List Items And Save it To View Bag (The Data, Pass BackGround Id, Present The Name ForeGround)
            ViewBag.Cinemas = new SelectList(data.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(data.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(data.Actors, "Id", "FullName");
            return View(response);
        }
        //POST: Movies/Edit body pameter
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> ConfirmEdit(int id, NewMovieVM movie)
        {
            if (id != movie.Id) return View("NotFound");
            var data = await _service.GetNewMovieDropDownsVMsValues();
            ViewBag.Cinemas = new SelectList(data.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(data.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(data.Actors, "Id", "FullName");
            if (!ModelState.IsValid)
                return View(movie);

            await _service.UpdateMovie(movie);
            return RedirectToAction(nameof(Index));
        }

        // Search Method
        public async Task<IActionResult> Filter(string searchString)
        {
            var data = await _service.GetAll(n => n.Cinema);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filterResult = data.Where(n => n.Name.Contains(searchString) || n.Description.Contains(searchString)).ToList();
                if (filterResult.Count == 0) return View("NotFound");

                return View("Index", filterResult);
            }
            return View("Index", data);
        }

    }
}

using E_Commerce.eTickets.WebSite.data;
using E_Commerce.eTickets.WebSite.Models;
using eTickets.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ICinemasService _service;
        public CinemasController(ICinemasService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index() => View(await _service.GetAll());

        //GET: Cinemas/Create
        public IActionResult Create() => View();

        //POST: Cinemas/Create/1
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateNewCinema([Bind("Logo, Name, Description")]Cinema cinema) 
        {
            if(!ModelState.IsValid)return View(cinema);
            await _service.Add(cinema);
            return RedirectToAction(nameof(Index));
        }

        //GET: Cinemas/Details/1
        public async Task<IActionResult> Details(int id) 
        {
            var data = await _service.GetById(id);
            if (data == null) return View("NotFound");

            return View(data); 
        }

        //GET: Cinemas/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetById(id);
            if (data == null) return View("NotFound");

            return View(data);
        }

        //POST: Cinemas/Model of Cinema
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditTheCenima(int id, [Bind]Cinema cinema) 
        {
            if (!ModelState.IsValid) return View(cinema);
            if (id != cinema.Id) return View(cinema);
            await _service.Update(id, cinema);
            return RedirectToAction(nameof(Index));
        }

        //GET: Cinemas/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _service.GetById(id);
            if (data == null) return View("NotFound");

            return View(data);
        }

        //POST: Cinemas/Cinemas/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id) 
        {
            var data = await _service.GetById(id);
            if (data == null) return View("NotFound");
            await _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

using E_Commerce.eTickets.WebSite.data;
using E_Commerce.eTickets.WebSite.Models;
using eTickets.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IActorsService _service;
        public ActorsController(IActorsService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index() => View(await _service.GetAll());
        
        //GET: Actors/Create
        public IActionResult Create() => View();

        //POST: From Actors/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName, ProfilePictureURL, Bio")]Actor actor) 
        {
            if ( !ModelState.IsValid ) return View(actor);

             await _service.Add(actor);
            return RedirectToAction(nameof(Index));
        }

        //GET: Actors/Details/1
        public async Task<IActionResult> Details(int id) 
        {
            var data = await _service.GetById(id);
            if (data == null) return View("NotFound");

            return View(data);
        }

        //GET: Actors/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetById(id);
            if (data == null) return View("NotFound");

            return View(data);
        }

        //POST: From Actors/Edit/1
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FullName, ProfilePictureURL, Bio")] Actor actor)
        {
            if (!ModelState.IsValid) return View(actor);

            await _service.Update(id, actor);
            return RedirectToAction(nameof(Index));
        }

        //GET: Actors/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _service.GetById(id);
            if (data == null) return View("NotFound");

            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var data = await _service.GetById(id);
            if (data == null) return View("NotFound");

            await _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }


    }
}

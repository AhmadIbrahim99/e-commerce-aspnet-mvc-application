using E_Commerce.eTickets.WebSite.data;
using E_Commerce.eTickets.WebSite.Models;
using eTickets.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class ProducersController : Controller
    {
        private readonly IProducersService _service;
        public ProducersController(IProducersService service)
        {
            _service = service;
        }
        //GET: Actors/Create
        public IActionResult Create() => View();

        //POST: From Actors/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName, ProfilePictureURL, Bio")] Producer producer)
        {
            if (!ModelState.IsValid) return View(producer);

            await _service.Add(producer);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Index() => View(await _service.GetAll());
        //GET: Producers/Details/1
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
        public async Task<IActionResult> Edit(int id, [Bind("Id, FullName, ProfilePictureURL, Bio")] Producer producer)
        {
            if (!ModelState.IsValid) return View(producer);
            
            if(id!= producer.Id)return View(producer);

            await _service.Update(id, producer);
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

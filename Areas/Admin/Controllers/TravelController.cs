using Doorang.Areas.Admin.ViewModels;
using Doorang.DAL;
using Doorang.Models;
using Doorang.Utilities.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Doorang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class TravelController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public TravelController(AppDbContext db,IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Travel> travels= await _db.Travels.Include(x=>x.Category).ToListAsync();
            return View(travels);
        }

        public async Task<IActionResult> Create()
        {
            CreateTravelVM create = new CreateTravelVM
            {
                Categories = await _db.Categories.ToListAsync(),
            };
            return View(create);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTravelVM create)
        {
            if (!ModelState.IsValid)
            {
                create.Categories= await _db.Categories.ToListAsync();
                return View(create);
            }

            bool result = await _db.Travels.AnyAsync(x=>x.Name.Trim().ToLower()==create.Name.Trim().ToLower());
            if (result)
            {
                create.Categories = await _db.Categories.ToListAsync();
                ModelState.AddModelError("Name","Is exists");
                return View(create);
            }
            if (!create.Photo.ValidateType())
            {
                create.Categories = await _db.Categories.ToListAsync();
                ModelState.AddModelError("Photo","Is not valid");
                return View(create);
            }
            if (!create.Photo.ValidateSize(10))
            {
                create.Categories = await _db.Categories.ToListAsync();
                ModelState.AddModelError("Photo", "Max 10mb");
                return View(create);
            }

            Travel travel = new Travel
            {
                Name = create.Name,
                Description = create.Description,
                Image = await create.Photo.CreateFileAsync(_env.WebRootPath,"assets","imgs"),
                CategoryId = create.CategoryId,
            };
            await _db.Travels.AddAsync(travel);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Travel travel = await _db.Travels.FirstOrDefaultAsync(x=>x.Id==id);
            if (travel == null) return NotFound();

            UpdateTravelVM update = new UpdateTravelVM
            { 
                Name=travel.Name,
                Description=travel.Description,
                CategoryId = travel.CategoryId,
                Categories=await _db.Categories.ToListAsync(),
                Image=travel.Image
            };
            return View(update);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateTravelVM update)
        {
            if (!ModelState.IsValid)
            {
                update.Categories=await _db.Categories.ToListAsync();
                return View(update);
            }
            Travel travel = await _db.Travels.FirstOrDefaultAsync(x => x.Id == id);
            if (travel == null) return NotFound();

            bool result = await _db.Travels.AnyAsync(x => x.Name.Trim().ToLower() == update.Name.Trim().ToLower() && x.Id != id);
            if (result)
            {
                update.Categories = await _db.Categories.ToListAsync();
                ModelState.AddModelError("Name","Is exists");
                return View(update);
            }

            if(update.Photo is not null)
            {
                if (!update.Photo.ValidateType())
                {
                    update.Categories = await _db.Categories.ToListAsync();
                    ModelState.AddModelError("Photo","Is not valid");
                    return View(update);
                }
                if (!update.Photo.ValidateSize(10))
                {
                    update.Categories = await _db.Categories.ToListAsync();
                    ModelState.AddModelError("Photo", "Max limit 10mb");
                    return View(update);
                }
                travel.Image.DeleteFileAsync(_env.WebRootPath, "assets", "imgs");
                travel.Image=await update.Photo.CreateFileAsync(_env.WebRootPath, "assets", "imgs");
            }
            travel.Name= update.Name;
            travel.Description= update.Description;
            travel.CategoryId= update.CategoryId;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Travel travel = await _db.Travels.FirstOrDefaultAsync(x => x.Id == id);
            if (travel == null) return NotFound();
            _db.Travels.Remove(travel);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); 
        }
    }
}

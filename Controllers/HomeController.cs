using Doorang.DAL;
using Doorang.Models;
using Doorang.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doorang.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        public HomeController(AppDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;

        }
        public async Task<IActionResult> Index()
        {
            List<Travel> travels = await _db.Travels.Include(x=>x.Category).ToListAsync();
            return View(travels);
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(UserRole.Admin.ToString()))
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

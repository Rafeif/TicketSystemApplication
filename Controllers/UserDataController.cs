using TicketSystemApplication.Data;
using TicketSystemApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketSystemApplication.Controllers
{
    public class UserDataController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public UserDataController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError Error in result.Errors)
            {
                ModelState.AddModelError("", Error.Description);
            }
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(x=>
             new UserViewModel()
             {
                 Id = x.Id,
                 Email = x.Email,
                 UserName = x.UserName,
                 PhoneNumber = x.PhoneNumber,
                 Role = String.Join(",",
                _userManager.GetRolesAsync(x).Result.ToArray()) ?? ""
             }).ToListAsync();

            return View(users);

          
        }
        public async Task<IActionResult> Roles()
        {

            return View(await _roleManager.Roles.ToListAsync());
        }
        public IActionResult CreateRole(string name)
        {

           return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(IFormCollection keyValues)
        {

            string name = keyValues["Name"].ToString();
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    Errors(result);
                }
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role is not null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");

                }
                else
                {
                    Errors(result);
                }
            }
            return View();
        }
        public async Task<IActionResult> EditRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string id, IdentityRole role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Roles));

                }
                else { Errors(result); }
            }
            return View(role);
        }
        public async Task<IActionResult> EditUser(string Id)
        {
            if (Id is null) return NotFound();
            IdentityUser user = await _userManager.FindByIdAsync(Id);
            if(user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string Id, string RoleId)
        {
            if (Id is null || RoleId is null) return NotFound();
            IdentityUser user = await _userManager.FindByIdAsync(Id);
            if (user == null) { return NotFound(); }
            IdentityRole role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null) { return NotFound(); }
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role.Name);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            //_context.UserRoles.Add(new IdentityUserRole<string> { RoleId = RoleId,UserId= Id });
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> userRoles()
        {
            var users = await _userManager.Users.Select(x =>
            new UserViewModel()
            {
                Id = x.Id,
                Email = x.Email,
                UserName = x.UserName,
                PhoneNumber = x.PhoneNumber,
                Role = String.Join(",",
                _userManager.GetRolesAsync(x).Result.ToArray()) ?? ""
            }).ToListAsync();

            return View(users);

        }
    }
 
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Application.DTOs;
using TaskManagement.Infrastructure.Utilities;

namespace TaskManagement.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {

        private readonly IHelperMethods _helperMethods;
        private readonly AppDbContext _context;
        public AuthController(IHelperMethods helperMethods, AppDbContext context)
        {
            _helperMethods = helperMethods;
            _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([Bind] RegisterUserDTO dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = _context.AppUsers.FirstOrDefault(u => u.Email == dto.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError(string.Empty, "Username or Email already exists.");
                        return RedirectToAction("Login");
                    }
                    Guid uniqueId = Guid.NewGuid();

                    // Convert to a string for use in models, views, or database fields
                    string id = uniqueId.ToString();
                    PasswordHashResult result = _helperMethods.HashPassword(dto.EnteredPassword!);
                    var u = new AppUser
                    {
                        Id = id,
                        UserName = dto.UserName,
                        FirstName = dto.FirstName!,
                        LastName = dto.LastName!,
                        Email = dto.Email,
                        DateRegistered = DateTime.Now,
                        PasswordHash = result.Hash,
                        Salt = result.Salt,
                    };
                    _context.AppUsers.Add(u);
                    _context.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    return View(dto);
                }

            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return RedirectToAction("Register");
            }
        }

        // POST: Auth/Login
        [HttpPost]

        public IActionResult Login([Bind] LoginUserDTO dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _context.AppUsers.FirstOrDefault(u => u.Email == dto.Email);
                    if (user == null)
                    {
                        TempData["Error"] = "Invalid username or password.";
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                        return RedirectToAction("Login");
                    }
                    bool result = _helperMethods.VerifyPassword(dto.EnteredPassword!, user?.PasswordHash!, user?.Salt!);
                    // Validate user (DB check)
                    if (result)
                    {
                        var UserRoles =  _context.AssignUserRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).ToList();

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName!),
                            new Claim("UserId", user.Id.ToString()),
                            
                        };
                        foreach (var role in UserRoles)
                        {
                            Console.WriteLine("User Role: " + role);
                            var roleName = _context.UserRoles.FirstOrDefault(r => r.RoleId == role);
                            Console.WriteLine("Role Name: " + roleName?.RoleName);
                            claims.Add(new Claim(ClaimTypes.Role, roleName?.RoleName!));
                            claims.Add(new Claim("ManagerOnly", roleName?.RoleName!));
                        }
                        var identity = new ClaimsIdentity(claims, "Cookies");
                        var principal = new ClaimsPrincipal(identity);

                        HttpContext.SignInAsync("Cookies", principal);
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return RedirectToAction("Login");
                }
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return RedirectToAction("Login");
            }

        }

        // Method for logging out
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Auth");
        }
    }
}

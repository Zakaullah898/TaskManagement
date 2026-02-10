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
        private readonly ITaskManagementRepo<UserRole> _userRolesRepo;
        private readonly IAssignUserRoleRepo _assignUserRoleRepo;
        private readonly IAuthService _authService;
        public AuthController(
            IHelperMethods helperMethods,
            AppDbContext context,
            ITaskManagementRepo<UserRole> userRolesRepo,
            IAssignUserRoleRepo assignUserRoleRepo,
            IAuthService authService
            )
        {
            _helperMethods = helperMethods;
            _context = context;
            _userRolesRepo = userRolesRepo;
            _assignUserRoleRepo = assignUserRoleRepo;
            _authService = authService;
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
        public async Task<IActionResult> Register([Bind] RegisterUserDTO dto)
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
                    var role = await _userRolesRepo.GetAsync(r => r.RoleName == "Employee");
                    if (role != null)
                    {

                        var assignRole = new AssignUserRole
                        {
                            UserId = u.Id,
                            RoleId = role.RoleId
                        };
                        var isCreated = await _assignUserRoleRepo.CreateAsync(assignRole);
                        if (isCreated)
                        {

                            TempData["Success"] = "Registration successful. Please log in.";

                        }

                    }


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

        public async Task<IActionResult> Login([Bind] LoginUserDTO dto)
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
                    if (!user.IsActive)
                    {
                        TempData["Error"] = "Your account is inactive. Please contact the administrator.";
                        ModelState.AddModelError(string.Empty, "Your account is inactive. Please contact the administrator.");
                        return RedirectToAction("Login");
                    }
                    bool result = _helperMethods.VerifyPassword(dto.EnteredPassword!, user?.PasswordHash!, user?.Salt!);
                    // Validate user (DB check)
                    if (result)
                    {
                        var UserRoles = await _assignUserRoleRepo.GettingRoleIds(u => u.UserId == user.Id);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user!.UserName!),
                            new Claim("UserId", user.Id!.ToString()!),

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

                        TempData["Success"] = "Login successful.";
                        user.LastLogin = DateTime.Now;
                        _context.AppUsers.Update(user);
                        _context.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Error"] = "Invalid username or password.";
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

        // Method for varify email
        [HttpGet]
        public IActionResult GettingVerifyEmail()
        {
            return PartialView("_VarifyEmail");
        }
        // verify the user
        [HttpPost("Auth/GettingVerifyEmail")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDTO model)
        {
            try 
            {
                var verifyEmail = await _authService.VarifyEmail(model.Email!);
                if (verifyEmail)
                {
                      
                    return PartialView("_ResetNewPassword", model.Email);
                }
                else
                {
                    return PartialView("_VarifyEmail");
                }

            }
            catch(ArgumentNullException e)
            {
                TempData["Error"] = e.Message;
                return PartialView("_VarifyEmail");
            }
            catch (Exception ex) 
            {
                TempData["Error"] = "An error occurred while processing your request.";
                return PartialView("_VarifyEmail");
            }
        }

        // Resetting Password 
        [HttpPut("Auth/ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            try 
            {
                var isPasswordReset = await _authService.ResetPassword(model.Password!, model.Email!);
                if (isPasswordReset)
                {
                    return Ok(new {response ="Password is updated successfully"});
                }
                else 
                {
                    return BadRequest();
                }
            }
            catch(ArgumentNullException ex) 
            {
                return BadRequest(ex);
            }
            catch(Exception e)
            {
                return BadRequest("An error occurred while resetting password.");
            }
        }
    }
}

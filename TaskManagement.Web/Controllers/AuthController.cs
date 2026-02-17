using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;
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
        private readonly ITaskManagementRepo<OTP> _OtpRepo;
        public AuthController(
            IHelperMethods helperMethods,
            AppDbContext context,
            ITaskManagementRepo<UserRole> userRolesRepo,
            IAssignUserRoleRepo assignUserRoleRepo,
            IAuthService authService,
            ITaskManagementRepo<OTP> OtpRepo
            )
        {
            _helperMethods = helperMethods;
            _context = context;
            _userRolesRepo = userRolesRepo;
            _assignUserRoleRepo = assignUserRoleRepo;
            _authService = authService;
            _OtpRepo = OtpRepo;
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
                        IsActive = true,
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
                        var UserRoles = await _assignUserRoleRepo.GettingRoleIds(u => u.UserId == user!.Id);

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
                        var hasProfile = user.HasProfile;
                        user.LastLogin = DateTime.Now;
                        _context.AppUsers.Update(user);
                        _context.SaveChanges();
                        if (hasProfile)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("CreateUserProfile", "UserProfile");
                        }
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
        public IActionResult VerifyEmail()
        {
            return View();
        }
        // verify the user
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDTO model)
        {
            if (!ModelState.IsValid)
                return View("VerifyEmail", model);

            try
            {


                var user = await _authService.GetUserByEmail(model.Email!);
                if (user != null)
                {
                    var otpRecord =await _OtpRepo.GetAllAsync();
                    foreach(var otprec in otpRecord)
                    {
                        
                        if (otprec.UserId  == user.Id)
                        {
                            await _OtpRepo.DeleteAsync(o => o.UserId == user.Id);
                        }
                    }

                    var otp = _helperMethods.GenerateSecureOtp();
                    var HashingOtp = _helperMethods.HashOtp(otp);
                    var email = user.Email;
                    var newOtp = new OTP
                    {
                        UserId = user.Id,
                        IsUsed = false,
                        Email = email,
                        OtpHash = HashingOtp,
                        ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                        AttemptCount = 0,
                        CreatedAt = DateTime.UtcNow,
                    };
                    await _authService.SendOtpEmail(email!, otp);
                    await _OtpRepo.CreateAsync(newOtp);
                    var verifyOtp = new VerifyOtpDTO
                    {
                        Email = model.Email
                    };
                    TempData["Success"] = "Your Email verfy successfully.";
                    return View("VerifyOtp", verifyOtp);
                }
                else
                {
                    ModelState.AddModelError("", "Email not found.");
                    return View("VerifyEmail", model);
                }
            }
            catch (ArgumentNullException e)
            {
                TempData["Error"] = e.Message;
                return View("VerifyEmail", model);
            }
            catch (Exception)
            {
                TempData["Error"] = "An error occurred while processing your request.";
                return View("VerifyEmail", model);
            }
        }

        // go to verfy otp view 
        public IActionResult VerifyOtp()
        {
            return View();
        }
        // Veryfying otp 
        [HttpPost]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDTO model)
        {
            var user = await _authService.GetUserByEmail(model.Email!);
            if (user == null)
            {
                TempData["Error"] = "Invalid Request";
                View("VerifyOtp");
            }

            var otpRecord = await _OtpRepo.GetAsync(o => o.UserId == user!.Id);

            if (otpRecord == null) 
            {
                TempData["Error"] = "OTP not found";
                View("VerifyOtp");
            }


            if (otpRecord!.ExpiryTime < DateTime.UtcNow) 
            {
                TempData["Error"] = "OTP not use to longer";
                return View("VerifyOtp");
            }

            if (otpRecord.IsUsed) 
            {
                TempData["Error"] = "OTP already used.";
                return BadRequest("OTP already used.");
            }

            if (otpRecord.AttemptCount >= 3) 
            {
                TempData["Error"] = "You reach you attempt limit";
                return View("VerifyOtp");
            }

            var hashedInput = _helperMethods.HashOtp(model.Otp!);

            if (hashedInput != otpRecord.OtpHash)
            {
                otpRecord.AttemptCount++;
                await _OtpRepo.UpdateAsync(otpRecord);
                TempData["Error"] = "Invalid OTP.";
                return View("VerifyOtp");
            }

            otpRecord.IsUsed = true;
            await _OtpRepo.UpdateAsync(otpRecord);
            TempData["Success"] = "OTP verified successfully.";
            var resetPasswordDto = new ResetPasswordDTO
            {
                Email = model.Email
            };
            return View("ResetPassword", resetPasswordDto);
        }

        // Resetting Password 
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var isPasswordReset = await _authService.ResetPassword(model.NewPassword!, model.Email!);
                if (isPasswordReset)
                {
                    TempData["Success"] = "Password is updated successfully.";
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    TempData["Error"] = "Password updating is failed";
                    return View(model);
                }
            }
            catch (ArgumentNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            catch (Exception)
            {
                TempData["Error"] = "An error occurred while resetting password.";
                ModelState.AddModelError("", "An error occurred while resetting password.");
                return View(model);
            }
        }

    

    }
}

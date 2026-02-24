using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManagement.Application.CustomException;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Web.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService  _userProfileService;
        private readonly ITaskManagementRepo<UserRole> _userRolesRepo;
        private readonly IAssignUserRoleRepo _assignUserRoleRepo;
        private readonly IAuthService _authService;
        private readonly ICloudinaryService _cloudinaryService;
        IMapper _mapper;
        public UserProfileController
            (
            IUserProfileService userProfileService,
            IMapper mapper,
            ITaskManagementRepo<UserRole> userRolesRepo,
            IAssignUserRoleRepo assignUserRoleRepo,
            IAuthService authService,
            ICloudinaryService cloudinaryService
            ) 
        {
            _userProfileService = userProfileService;
            _mapper = mapper;
            _userRolesRepo = userRolesRepo;
            _assignUserRoleRepo = assignUserRoleRepo;
            _authService = authService;
            _cloudinaryService = cloudinaryService;
        }
        // User profile 
        public async Task<IActionResult> Profile()
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                var userProfile = await _userProfileService.GetUserProfileById(userId!);
                var user = await _authService.GetUserById(userId!);

                var profileDto = _mapper.Map<GettingUserProfileDTO>(userProfile);
                // getting assigned role to the user
                var assignedRoles = await _assignUserRoleRepo.GettingRoleIds(r => r.UserId == userId);
                if (assignedRoles != null && user.HasProfile)
                {
                    foreach (var assignedRole in assignedRoles)
                    {
                       
                        var role = await _userRolesRepo.GetAsync(r => r.RoleId == assignedRole, true);
                        profileDto.Role!.Add(role.RoleName!);
                       
                    }
                    return View(profileDto);
                }
                else
                {
                    TempData["Error"] = "User profile not found. Please create your profile.";
                    return RedirectToAction("CreateUserProfile", "UserProfile");
                }
            }
            catch (ArgumentNullException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
            catch (KeyNotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("CreateUserProfile", "UserProfile");
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine($"The Error Message in Exception :{ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult CreateUserProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserProfile(UserProfileDTO dtoModel)
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                var profileImageUrl = await _cloudinaryService.UploadImageAsync(dtoModel.ProfileImage!);
                UserProfile userProfile = new UserProfile
                {
                    UserId = userId,
                    FirstName = dtoModel.FirstName,
                    LastName = dtoModel.LastName,
                    UserName = dtoModel.UserName,
                    Email = dtoModel.Email,
                    PhoneNumber = dtoModel.PhoneNumber,
                    JobTitle = dtoModel.JobTitle,
                    DateJoined = dtoModel.DateJoined,
                    ProfileImagePath = profileImageUrl
                };
                var IsCreated = await _userProfileService.CreateUserProfileAsync(userProfile);
                if (IsCreated)
                {
                    var profile = await _userProfileService.GetUserProfileById(userId!);
                    var userProfileDto = _mapper.Map<GettingUserProfileDTO>(profile);
                    return View("Profile", userProfileDto);
                }
                else
                {
                    return View();
                }
            }
            catch (ArgumentNullException ex)
            {
                TempData["Error"] = "An error occurred while resetting password.";
                ModelState.AddModelError("", "An error occurred while resetting password.");
                return View(dtoModel);
            }
            catch (ConflictException ex)
            {
                TempData["Error"] = "An error occurred while resetting password.";
                ModelState.AddModelError("", "An error occurred while resetting password.");
                return View(dtoModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while resetting password.";
                ModelState.AddModelError("", "An error occurred while resetting password.");
                return View(dtoModel);
            }
        }

        // Updating user Profile
        [HttpGet]
        public async Task<ActionResult> UpdateProfile() 
        {
            var userId = User.FindFirst("UserId")?.Value;
            var profile = await _userProfileService.GetUserProfileById(userId!);
            if (profile != null)
            {
                var usePro =  _mapper.Map<UserProfileDTO>(profile);
                return View(usePro);
            }
            else
            {
                return View("Profile");
            }
        }


        [HttpPost]
        public async Task<ActionResult> UpdateProfile(UserProfileDTO dto)
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                var userProfile = _mapper.Map<UserProfile>(dto);
                if (dto.ProfileImage != null) 
                {
                    var profileImageUrl =  await _cloudinaryService.UploadImageAsync(dto.ProfileImage!); ;
                    userProfile.ProfileImagePath = profileImageUrl;
                }
                else
                {
                    userProfile.ProfileImagePath = dto.ProfileImagePath;
                }
                    userProfile.UserId = userId;
               var isUpdated = await _userProfileService.UpdatingUserProfile(userProfile);
                if (isUpdated)
                {
                    TempData["Success"] = "Profile updated successfully.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(dto);
                }
            }
            catch (ArgumentNullException ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
            catch (ConflictException ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }
    }
}

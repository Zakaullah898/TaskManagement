using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Interfaces; // your service namespace

public class NavbarProfileViewComponent : ViewComponent
{
    private readonly IUserProfileService _service;

    public NavbarProfileViewComponent(IUserProfileService service)
    {
        _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = UserClaimsPrincipal.FindFirst("UserId")?.Value;

        // Safely get profile
        var profile = await _service.GetUserProfileById(userId!);

        // If profile not found, use default
        var imagePath = profile?.ProfileImagePath ?? "/images/default/default-profile.png";


        return View("Default", imagePath); // returns Views/Shared/Components/NavbarProfile/Default.cshtml
    }
}
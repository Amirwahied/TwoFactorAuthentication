using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TwoFactorAuthentication.Authentication.Constants;
using TwoFactorAuthentication.Models;
using TwoFactorAuthentication.ViewModels.User;

namespace TwoFactorAuthentication.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }



    public IActionResult Index()
    {

        LoginViewModel loginView = new()
        {
            Id = ((ClaimsIdentity?)User.Identity)?.FindFirst(CustomClaimsTypes.UserID)?.Value
                                          ,
            Token = ((ClaimsIdentity?)User.Identity)?.FindFirst(CustomClaimsTypes.Token)?.Value
                                          ,
            Username = User.Identity?.Name
        };

        return View(loginView);
    }

    public IActionResult AccessDenied()
    {
        return View("AccessDenied"); // Return the AccessDenied view
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

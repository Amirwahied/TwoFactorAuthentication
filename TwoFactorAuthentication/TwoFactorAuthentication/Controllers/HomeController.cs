using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using TwoFactorAuthentication.Contracts;
using TwoFactorAuthentication.Models;

namespace TwoFactorAuthentication.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISqlConnectionFactory _sqlConnection;

    public HomeController(ILogger<HomeController> logger, ISqlConnectionFactory sqlConnection)
    {
        _logger = logger;
        _sqlConnection = sqlConnection;
    }

    public async Task<IActionResult> Index()
    {
        await using SqlConnection sqlConnection = _sqlConnection.CreateSqlConnection();

        var users = await sqlConnection.QueryAsync<User_Login_Info>(@"select * from users_login_info");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

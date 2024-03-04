using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wikirace.Models;

namespace Wikirace.Controllers;

[Route("[controller]/[action]")]
public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger4;

    public HomeController(ILogger<HomeController> logger) {
        _logger4 = logger;
    }

    [HttpGet]
    [Route("/")]
    public IActionResult Index() {

        return View();
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

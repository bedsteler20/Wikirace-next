using Microsoft.AspNetCore.Mvc;
using Wikirace.Models;

namespace Wikirace.Controllers;

[Route("[controller]/[action]")]
public class SessionController : Controller {
    private readonly ILogger<SessionController> _logger;

    public SessionController(ILogger<SessionController> logger) {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index() {
        return RedirectToAction("Join");
    }

    [HttpGet]
    public IActionResult Join() {
        return View();
    }

    [HttpPost]
    public IActionResult Join(JoinGameModel model) {
        return RedirectToAction("Play", new { gameId = model });
    }

    [HttpGet]
    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public IActionResult Create(string startPage, string endPage) {
        return RedirectToAction("Play", new { id = "123" });
    }


}

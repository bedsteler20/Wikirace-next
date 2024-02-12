

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Wikirace.Controllers;



[Route("[controller]/[action]")]
public class GameController : Controller {
    private readonly ILogger<GameController> _logger;

    public override void OnActionExecuting(ActionExecutingContext context) {
        

        base.OnActionExecuting(context);
    }

    public GameController(ILogger<GameController> logger) {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Join(string id) {
        return RedirectToAction("Play", new { id });
    }

    [HttpGet]
    [Authorize(Policy = "IsInGame")]
    public IActionResult Create() {
        return View();
    }


}
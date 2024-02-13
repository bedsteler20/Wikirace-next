using Htmx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wikirace.Data;
using Wikirace.Repository;
using Wikirace.Security;

namespace Wikirace.Controllers;

// Todo: Add server sent events for game state updates.

[Route("[controller]/{gameId}/[action]")]
[Authorize(Policy = Polices.IsInGame)]
public class GameController : Controller {
    private readonly ILogger<GameController> _logger;
    private readonly IRepository _repository;

    // Helper properties to access the game and player from the HttpContext.
    // These are set in the IsInGameRequirementHandler. So they will only be 
    // available if the action has the Authorization(Policy = "IsInGame") attribute.
    private Game? Game => HttpContext.Items["Game"] as Game;
    private Player? Player => HttpContext.Items["Player"] as Player;


    public GameController(ILogger<GameController> logger, IRepository repository) {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public IActionResult Play() {
        return View();
    }

    [HttpGet]
    public IActionResult Lobby() {
        return Request.IsHtmx()
            ? PartialView("_PlayerList", Game!.Players)
            : View(Game);
    }

    [HttpGet]
    public async Task<IActionResult> PageFrame() {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Authorize(Policy = Polices.IsGameOwner)]
    public async Task<IActionResult> End() {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Authorize(Policy = Polices.IsGameOwner)]
    public async Task<IActionResult> Start() {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Authorize(Policy = "IsGameOwner")]
    public async Task<IActionResult> KickPlayer() {
        throw new NotImplementedException();
    }


    [HttpPost]
    public async Task<IActionResult> Leave() {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePage() {
        throw new NotImplementedException();
    }
}
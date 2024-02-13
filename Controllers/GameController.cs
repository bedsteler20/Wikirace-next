using Htmx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wikirace.Data;
using Wikirace.Repository;
using Wikirace.Security;
using Wikirace.Services;

namespace Wikirace.Controllers;

// Todo: Add server sent events for game state updates.

[Route("[controller]/{gameId}/[action]")]
[Authorize(Policy = Polices.IsInGame)]
public class GameController : Controller {
    private readonly ILogger<GameController> _logger;
    private readonly IRepository _repository;
    private readonly ClientEventsService _eventSender;

    // Helper properties to access the game and player from the HttpContext.
    // These are set in the IsInGameRequirementHandler. So they will only be 
    // available if the action has the Authorization(Policy = "IsInGame") attribute.
    private Game Game => (HttpContext.Items["Game"] as Game)!;
    private Player Player => (HttpContext.Items["Player"] as Player)!;


    public GameController(ILogger<GameController> logger, IRepository repository, ClientEventsService eventSender) {
        _logger = logger;
        _repository = repository;
        _eventSender = eventSender;
    }

    [HttpGet]
    public IActionResult Play() {
        return View();
    }

    [HttpGet]
    public IActionResult Lobby() {
        return Request.IsHtmx()
            ? PartialView("_PlayerList", Game.Players)
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
        await _repository.StartGame(Game.Id);
        await _eventSender.SendEvent(EventNames.StartGame, Game!.Id);

        return Ok();
    }

    [HttpPost]
    [Authorize(Policy = Polices.IsGameOwner)]
    public async Task<IActionResult> KickPlayer([FromQuery] string playerId) {
        if (playerId == Player.Id) return BadRequest("You cannot kick yourself silly!");
        await _eventSender.SendEvent(EventNames.KickPlayer, Game!.Id, playerId);
        await _eventSender.SendEvent(EventNames.RefreshLobby, Game!.Id);
        var result = await _repository.KickPlayer(Game.Id, playerId);

        return result == null
            ? NotFound()
            : Ok();
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
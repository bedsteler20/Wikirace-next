using Htmx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wikirace.Data;
using Wikirace.Repository;
using Wikirace.Security;
using Wikirace.Services;

namespace Wikirace.Controllers;

// Todo: Add server sent events for game state updates.

[Route("[controller]/{gameId}/[action]")]
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

    public override void OnActionExecuting(ActionExecutingContext context) {
        if (Game.State == GameState.Ended) {
            context.Result = RedirectToAction("Index", "Home");
        }
        base.OnActionExecuting(context);
    }

    [HttpGet]
    [Authorize(Policy = Polices.IsInGame)]
    public IActionResult Play() {
        return View(Player);
    }

    [HttpGet]
    [Authorize(Policy = Polices.IsInGame)]
    public IActionResult Lobby() {
        if (Game.State != GameState.WaitingForPlayers) {
            return RedirectToAction("Play");
        }
        return Request.IsHtmx()
            ? PartialView("_PlayerList", Player)
            : View(Player);
    }

    [HttpGet]
    [Authorize(Policy = Polices.IsInGame)]
    public IActionResult PageFrame() {
        return View(Player);
    }

    [HttpPost]
    [Authorize(Policy = Polices.IsInGame)]
    [Authorize(Policy = Polices.IsGameOwner)]
    public async Task<IActionResult> End() {
        await _repository.EndGame(Game.Id);
        await _eventSender.SendEvent(EventNames.EndGame, Game.Id);
        Response.Htmx(h => h.Redirect(Url.Action("Index", "Home")!));
        return Ok();
    }

    [HttpPost]
    [Authorize(Policy = Polices.IsInGame)]
    [Authorize(Policy = Polices.IsGameOwner)]
    public async Task<IActionResult> Start() {
        await _repository.StartGame(Game.Id);
        await _eventSender.SendEvent(EventNames.StartGame, Game!.Id);

        Response.Htmx(h => h.Redirect(Url.Action("Play", new { gameId = Game!.Id })!));

        return Ok();
    }

    [HttpPost]
    [Authorize(Policy = Polices.IsInGame)]
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
    [Authorize(Policy = Polices.IsInGame)]
    public async Task<IActionResult> Leave() {
        Response.Htmx(h => h.Redirect("/"));
        await _repository.KickPlayer(Game.Id, Player.Id);
        await _eventSender.SendEvent(EventNames.RefreshLobby, Game.Id);
        return Ok();
    }

    [HttpPost]
    [Authorize(Policy = Polices.IsInGame)]
    public async Task<IActionResult> UpdatePage([FromQuery] string name) {
        if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name cannot be empty");
        if (Game.State != GameState.InProgress) return BadRequest("Game has not started yet");
        await _repository.UpdatePage(Game.Id, Player.Id, name);

        if (Game.EndPage == name) {
            await _eventSender.SendEvent(EventNames.WinGame, Game.Id, Player.Id);
        }

        return Ok();
    }
}
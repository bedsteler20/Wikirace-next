using Microsoft.AspNetCore.Mvc;
using Wikirace.Data;
using Wikirace.Models;
using Wikirace.Repository;
using Wikirace.Services;
using Wikirace.Utils;

namespace Wikirace.Controllers;

/// <summary>
/// Controller for managing game sessions.
/// </summary>
[Route("[controller]/[action]")]
public class SessionController : Controller {
    private readonly ILogger<SessionController> _logger;
    private readonly IRepository _repository;
    private readonly ClientEventsService _clientEventsService;

    public SessionController(ILogger<SessionController> logger, IRepository repository, ClientEventsService clientEventsService) {
        _logger = logger;
        _repository = repository;
        _clientEventsService = clientEventsService;
    }

    [HttpGet]
    public IActionResult Index() {
        return RedirectToAction("Join");
    }

    [HttpGet]
    public IActionResult Join([FromQuery] string? gameId = null) {
        if (gameId != null) {
            return View(new JoinGameModel { JoinCode = gameId, HideJoinCode = true });
        }
        return View(new JoinGameModel {
            HideJoinCode = false
        });
    }

    [HttpPost]
    public async Task<IActionResult> Join(JoinGameModel model) {
        if (!ModelState.IsValid) {
            return View(model);
        }

        var game = await _repository.GetGameByJoinCode(model.JoinCode);
        if (game == null) {
            ModelState.AddModelError("JoinCode", "Game not found");
            return View(model);
        }

        if (game.Players.Count() >= game.MaxPlayers) {
            ModelState.AddModelError("JoinCode", "Game is full");
            return View(model);
        }

        if (game.Players.Any(p => p.UserId == User!.GetUserId())) {
            ModelState.AddModelError("JoinCode", "You are already in this game");
            return View(model);
        }

        await _repository.JoinGame(game.Id, model.DisplayName, User!.GetUserId()!);
        await _clientEventsService.SendEvent(EventNames.RefreshLobby, game.Id, null);

        return RedirectToAction("Lobby", "Game", new { gameId = game.Id });
    }

    // TODO: Disallow anonymous users from creating games
    [HttpGet]
    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGameModel model) {
        if (!ModelState.IsValid) {
            return View(model);
        }

        var game = await _repository.CreateGame(model.StartPage, model.EndPage, model.MaxPlayers, model.GameType);
        await _repository.JoinGame(game.Id, model.DisplayName, User!.GetUserId()!, isOwner: true);

        return RedirectToAction("Lobby", "Game", new { gameId = game.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Completions([FromQuery] string? q, [FromServices] WikipediaClient wikipediaClient) {
        return Json(await wikipediaClient.SearchAsync(q ?? ""));
    }
}

using Microsoft.AspNetCore.Mvc;
using Wikirace.Models;
using Wikirace.Repository;

namespace Wikirace.Controllers;

[Route("[controller]/[action]")]
public class SessionController : Controller {
    private readonly ILogger<SessionController> _logger;
    private readonly IRepository _repository;

    public SessionController(ILogger<SessionController> logger, IRepository repository) {
        _logger = logger;
        _repository = repository;
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
    public async Task<IActionResult> Join(JoinGameModel model) {
        if (!ModelState.IsValid) {
            return View(model);
        }

        var game = await _repository.GetGameByJoinCode(model.JoinCode);
        if (game == null) {
            ModelState.AddModelError("JoinCode", "Game not found");
            return View(model);
        }

        await _repository.JoinGame(game.Id, model.DisplayName, User!.Identity!.Name!);

        return RedirectToAction("Play", "Game", new { gameId = game.Id });
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

        return RedirectToAction("Play", "Game", new { gameId = game.Id });
    }


}

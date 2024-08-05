using WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace WebApp.Controllers;

public class WebHooksController : ControllerBase
{
    private readonly BotUpdateHandler _botUpdateHandler;

    public WebHooksController(BotUpdateHandler botUpdateHandler)
        => _botUpdateHandler = botUpdateHandler;


    [HttpPost]
    public async Task<IActionResult> Index([FromBody] Update update, CancellationToken cancellationToken = default)
    {
        await _botUpdateHandler.HandleUpdateAsync(update, cancellationToken);

        return Ok();
    }
}
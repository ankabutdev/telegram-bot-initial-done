using Microsoft.Extensions.Localization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebApp.Resources;

namespace WebApp.Services;

public partial class BotUpdateHandler
{
    public string BaseUrl { get; }
    private readonly HttpClient _httpClient;
    private readonly ILogger<BotUpdateHandler> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private IStringLocalizer _localizer;

    public BotUpdateHandler(IConfiguration configuration,
        ILogger<BotUpdateHandler> logger,
        ITelegramBotClient botClient,
        IServiceScopeFactory scopeFactory)
    {
        _httpClient = new HttpClient();
        _logger = logger;
        _botClient = botClient;
        _scopeFactory = scopeFactory;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        await RegisterServicesAsync();

        // multilanguage region
        //var culture = await GetCultureForUser(update, _botClient);

        //CultureInfo.CurrentCulture = culture;
        //CultureInfo.CurrentUICulture = culture;

        var handler = update.Type switch

        {
            UpdateType.Message => HandlerMessageAsync(_botClient, update.Message!, cancellationToken),
            UpdateType.CallbackQuery => HandlerCallBackQueryAsync(_botClient, update, cancellationToken),

            // handler other updates
            _ => HandlerUnknownUpdate(_botClient, update, cancellationToken),
        };

        try
        {
            await handler;
        }
        catch (Exception ex)
        {
            await HandlerWebHookErrorAsync(_botClient, ex, cancellationToken);
        }
    }

    // don't need if bot is not multilanguage

    //private async Task<CultureInfo> GetCultureForUser(Update update, ITelegramBotClient botClient)
    //{
    //    var chatId = update.Type switch
    //    {
    //        UpdateType.Message => update.Message!.Chat.Id,
    //        UpdateType.EditedMessage => update.EditedMessage!.Chat.Id,
    //        UpdateType.CallbackQuery => update.CallbackQuery!.Message!.Chat.Id,
    //        UpdateType.InlineQuery => update.InlineQuery!.From.Id,
    //        UpdateType.Unknown => update.Message!.Chat.Id,
    //        _ => 0
    //    };

    //    var user = await _userService.GetByChatIdAsync(chatId.ToString()!);

    //    return new CultureInfo(user == null ? "uz" : user.Language!);

    //}

    public async Task RegisterServicesAsync()
    {
        using var scope = _scopeFactory.CreateScope();

        _localizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer<BotLocalizer>>();

        // Register region for Services
        // _userService = scope.ServiceProvider.GetRequiredService<UserService>();
    }

    private Task HandlerUnknownUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Update type {0} received", update.Type);

        return Task.CompletedTask;
    }

    public Task HandlerWebHookErrorAsync(ITelegramBotClient botClient, Exception ex, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Error occured with Telegram bot: {0}", ex.Message);

        return Task.CompletedTask;
    }
}

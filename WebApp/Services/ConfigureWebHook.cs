using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using WebApp.Models;

namespace WebApp.Services;

public class ConfigureWebHook : IHostedService
{
    private readonly ILogger<ConfigureWebHook> _logger;
    private readonly IServiceProvider _serviceProvider;
    private BotConfiguration? _botConfig;

    public ConfigureWebHook(ILogger<ConfigureWebHook> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _botConfig = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var botClinet = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        var webHookAddress = $@"{_botConfig!.HostAddress}/bot/{_botConfig!.Token}";

        await botClinet.SendTextMessageAsync(
            chatId: 1904461384,
            text: $"Webhook is being Starting!");

        await botClinet.SetWebhookAsync(
            url: webHookAddress,
            allowedUpdates: Array.Empty<UpdateType>(),
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        _logger.LogInformation("WebHook removing!");

        await botClient.SendTextMessageAsync(
            chatId: 1904461384,
            text: "Bot sleeping");
    }
}

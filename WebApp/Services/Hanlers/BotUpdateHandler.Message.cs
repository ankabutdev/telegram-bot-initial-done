using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace WebApp.Services;

public partial class BotUpdateHandler
{
    private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var chat = message.Chat;

        _logger.LogInformation("Received message from {0}", chat?.FirstName);

        var handler = message.Type switch
        {
            MessageType.Text => HandleTextMessageAsync(botClient, message, cancellationToken),
            //MessageType.Contact => HandleShareContactAsync(botClient, message, cancellationToken),
            _ => HandleUnknownMessageAsync(message, cancellationToken),
        };

        await handler;
    }

    private async Task HandleTextMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var text = message.Text;

        _logger.LogInformation("Chat: {chat.FirstName}", message.Chat.FirstName);

        var handler = text switch
        {
            "/start" => HandleStartMessageAsync(botClient, message, cancellationToken),
            _ => HandleDefaultAsync(botClient, message, cancellationToken)
        };

        await handler;
    }

    private Task HandleUnknownMessageAsync(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received message type {0}", message.Type);

        return Task.CompletedTask;
    }
}
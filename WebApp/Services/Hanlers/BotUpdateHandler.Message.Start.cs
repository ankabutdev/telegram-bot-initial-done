using Telegram.Bot.Types;
using Telegram.Bot;

namespace WebApp.Services;

public partial class BotUpdateHandler
{
    private async Task HandleStartMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        // echo

        await _botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                text: $"Hello {message.Chat.FirstName}");
    }
}
using Telegram.Bot.Types;
using Telegram.Bot;

namespace WebApp.Services;

public partial class BotUpdateHandler
{
    private async Task HandleDefaultAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {

        // default echo

        await _botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                text: message.Text);
    }
}
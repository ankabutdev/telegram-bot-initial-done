using Telegram.Bot.Types;
using Telegram.Bot;

namespace WebApp.Services;

public partial class BotUpdateHandler
{
    private async Task HandlerCallBackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(update);

        var data = update.CallbackQuery.Data;

        var handler = data switch
        {
            _ => HandleDefaultAsync(botClient, update.Message, cancellationToken)
        };

        await handler;
    }

    //private async Task CreateUserAsync(string language, User user)
    //{
    //    var userResult = new UserEntity()
    //    {
    //        ChatId = user.Id.ToString(),
    //        Language = language,
    //        UserName = user.Username,
    //        FirstName = user.FirstName,
    //        LastName = user.LastName,
    //    };

    //    var result = await _userService.GetByChatIdAsync(user.Id.ToString());

    //    if (result == null)
    //    {
    //        await _userService.CreateUserAsync(userResult);
    //    }
    //    else
    //    {
    //        await _userService.UpdateUserLanguageAsync(user.Id.ToString(), language);
    //    }
    //}

}
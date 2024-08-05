using Telegram.Bot;
using WebApp.Models;
using WebApp.Services;

#pragma warning disable

// default settings

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var config = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

builder.Services.AddHttpClient("webhook").AddTypedClient<ITelegramBotClient>(
    httpClient => new TelegramBotClient(config.Token, httpClient));

// Add hosted service and scoped service
builder.Services.AddHostedService<ConfigureWebHook>();
builder.Services.AddTransient<BotUpdateHandler>();
builder.Services.AddControllers().AddNewtonsoftJson();

// builder.Services.AddScoped<UserService>();

builder.Services.AddLocalization();

var app = builder.Build();

var supportedCultures = new[] { "uz-Uz", "en-Us", "ru-Ru" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseRouting();
app.UseCors();
app.UseHttpsRedirection();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "webhook",
        pattern: $"bot/{config.Token}",
        new { controller = "WebHooks", action = "Index" });

    endpoints.MapControllers();
});
app.Run();
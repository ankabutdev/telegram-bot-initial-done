namespace WebApp.Abstractions;

public class Url
{
    public string? BaseUrl { get; }
    public HttpClient _httpClient;

    public Url(IConfiguration configuration)
    {
        BaseUrl = configuration["BotConfiguration:API_URL"];
        _httpClient = new HttpClient();
    }
}

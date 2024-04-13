namespace eShop.WebhookClient.Services;
using System.Text.Json;

public class WebhookData
{
    public DateTime When { get; set; }

    public string? Payload { get; set; }

    public string? Type { get; set; }
}

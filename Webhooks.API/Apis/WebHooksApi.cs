﻿namespace Webhooks.API;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Webhooks.API.Extensions;

public static class WebHooksApi
{
    public static RouteGroupBuilder MapWebhooksApi(this RouteGroupBuilder app)
    {
        app.MapGet("/", async (WebhooksContext context, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserId();
            var data = await context.Subscriptions.Where(s => s.UserId == userId).ToListAsync();
            return TypedResults.Ok(data);
        });

        app.MapGet("/{id:int}", async Task<Results<Ok<WebhookSubscription>, NotFound<string>>> (
            WebhooksContext context,
            ClaimsPrincipal user,
            int id) =>
        {
            var userId = user.GetUserId();
            var subscription = await context.Subscriptions
                .SingleOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (subscription != null)
            {
                return TypedResults.Ok(subscription);
            }
            return TypedResults.NotFound($"Subscriptions {id} not found");
        });

        app.MapPost("/", async Task<Results<Created, BadRequest<string>>> (
            WebhookSubscriptionRequest request,
            IGrantUrlTesterService grantUrlTester,
            WebhooksContext context,
            ClaimsPrincipal user) =>
        {
            var grantOk = await grantUrlTester.TestGrantUrl(request.Url, request.GrantUrl, request.Token ?? string.Empty);

            if (grantOk)
            {
                var subscription = new WebhookSubscription()
                {
                    Date = DateTime.UtcNow,
                    DestUrl = request.Url,
                    Token = request.Token,
                    Type = Enum.Parse<WebhookType>(request.Event, ignoreCase: true),
                    UserId = user.GetUserId()
                };

                context.Add(subscription);
                await context.SaveChangesAsync();

                return TypedResults.Created($"/api/v1/webhooks/{subscription.Id}");
            }
            else
            {
                return TypedResults.BadRequest($"Invalid grant URL: {request.GrantUrl}");
            }
        })
        .ValidateWebhookSubscriptionRequest();

        app.MapDelete("/{id:int}", async Task<Results<Accepted, NotFound<string>>> (
            WebhooksContext context,
            ClaimsPrincipal user,
            int id) =>
        {
            var userId = user.GetUserId();
            var subscription = await context.Subscriptions.SingleOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (subscription != null)
            {
                context.Remove(subscription);
                await context.SaveChangesAsync();
                return TypedResults.Accepted($"/api/v1/webhooks/{subscription.Id}");
            }

            return TypedResults.NotFound($"Subscriptions {id} not found");
        });

        return app;
    }
}

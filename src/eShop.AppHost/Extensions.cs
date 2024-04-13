namespace eShop.AppHost;

using System.Threading.Tasks;
using Aspire.Hosting.Lifecycle;

/// <summary>
/// Extension methods for the IDistributedApplicationBuilder interface.
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Adds a hook to set the ASPNETCORE_FORWARDEDHEADERS_ENABLED environment variable to true for all projects in the application.
    /// </summary>
    /// <param name="builder">The IDistributedApplicationBuilder instance.</param>
    /// <returns>The IDistributedApplicationBuilder instance.</returns>
    public static IDistributedApplicationBuilder AddForwardedHeaders(this IDistributedApplicationBuilder builder)
    {
        builder.Services.TryAddLifecycleHook<AddForwardHeadersHook>();
        return builder;
    }

    /// <summary>
    /// The add forward headers hook.
    /// </summary>
    private class AddForwardHeadersHook : IDistributedApplicationLifecycleHook
    {
        /// <summary>
        /// Executes before the application starts.
        /// </summary>
        /// <param name="appModel">The DistributedApplicationModel instance.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task BeforeStartAsync(DistributedApplicationModel appModel, CancellationToken cancellationToken = default)
        {
            foreach (var p in appModel.GetProjectResources())
            {
                p.Annotations.Add(new EnvironmentCallbackAnnotation(context =>
                {
                    context.EnvironmentVariables["ASPNETCORE_FORWARDEDHEADERS_ENABLED"] = "true";
                }));
            }

            return Task.CompletedTask;
        }
    }
}

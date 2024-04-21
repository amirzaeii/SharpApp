namespace SharpSoft.App.Client.Core.Services;
public class LocationService(IJSRuntime jsRuntime) : ILocationService , IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>
        ("import", "./_content/SharpSoft.App.Client.Core/scripts/loadmap.js").AsTask());

    public async ValueTask<bool> CanUseGeoLocation()
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<bool>("canUseGeoLocation");
    }

    public async ValueTask<double[]?> GetCurrentLocation()
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<double[]?>("getCurrentPosition");
    }
    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}

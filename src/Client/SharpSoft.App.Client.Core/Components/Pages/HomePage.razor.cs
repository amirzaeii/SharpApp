
using System.Text.Json;
using Microsoft.JSInterop;

namespace SharpSoft.App.Client.Core.Components.Pages;

public partial class HomePage : AppComponentBase
{
    private IJSObjectReference? module;
    private bool isLoading;
   
    private bool okSubmited;
    private bool errorSubmited;
    private double[]? geoLocation;
    private bool locationIsOff;
    [AutoInject]
    private ILocationService _locationService = default!;
    private string currentTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss");
    private System.Timers.Timer? secondsTimer;
    private string? serverAddress;

    protected override async Task OnInitAsync()
    {       
        //one second interval
        secondsTimer = new System.Timers.Timer(1000)
        {
            Enabled = true,
            AutoReset = true
        };
        secondsTimer.Elapsed += (sender, e) => UpdateTime();

        await base.OnInitAsync();
    }

    private void UpdateTime()
    {
        currentTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss");
        InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            serverAddress = await StorageService.GetItem("ServerAddress");
            if (string.IsNullOrEmpty(serverAddress) is true)
            {
                NavigationManager.NavigateTo("/sign-in");
                return;
            }
            var raw = await HttpClient.GetStringAsync($"{serverAddress}api/Hr/locationapi/list");
            raw = raw.Replace('\n', ' ');
            module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/SharpSoft.App.Client.Core/scripts/loadmap.js");

            if (module != null)
            {
               await module.InvokeAsync<string>("load_map", Convert.ToString(raw));
            }

            await RefreshLocation();

        }
    }

    private async Task CheckIn(bool type)
    {
        if (isLoading) return;
        isLoading = true;

        var personid = await StorageService.GetItem("PersonId");
        if(string.IsNullOrEmpty(personid) is true)
        {
            errorSubmited = true;
            isLoading = false;
            return;
        }
        try
        {
            var model = new
            {
                PersonId = personid!,
                Date = DateTimeOffset.Now,
                Location = new { Latitude = geoLocation?[0], Longitude = geoLocation?[1] }
            };
            var method = type ? "checkin" : "checkout";
            var response = await HttpClient.PostAsJsonAsync($"{serverAddress}api/Hr/attendanceapi/{method}", model);
            var responseCode = await response.Content.ReadFromJsonAsync<int>();

            if (responseCode > 0) okSubmited = true;
            else errorSubmited = true;
        }
        catch (KnownException e)
        {
            errorSubmited = true;
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task RefreshLocation()
    {
        geoLocation = await _locationService.GetCurrentLocation();
        if (geoLocation is null)
        {
            locationIsOff = true;
            StateHasChanged();
            return;
        }
        await module.InvokeAsync<string>("refreshGeoLocation", JsonSerializer.Serialize(geoLocation));
        
    }
    //async ValueTask IAsyncDisposable.DisposeAsync()
    //{
        //if (module is not null)
        //{
        //    await module.DisposeAsync();
        //}
    //}
}

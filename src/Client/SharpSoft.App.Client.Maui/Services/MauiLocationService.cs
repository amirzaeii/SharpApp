
namespace SharpSoft.App.Client.Maui.Services;
internal class MauiLocationService : ILocationService
{
    private CancellationTokenSource? _cancelTokenSource;
    private bool _isCheckingLocation;
    public async ValueTask<bool> CanUseGeoLocation()
    {
        return true;
    }

    public async ValueTask<double[]?> GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            _cancelTokenSource = new CancellationTokenSource();

            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                var permissionRequest = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
                return [location.Latitude, location.Longitude]; 
            return null;
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
            return null;
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }

    public void CancelRequest()
    {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
    }
}

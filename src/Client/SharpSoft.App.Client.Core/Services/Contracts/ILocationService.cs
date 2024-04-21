

namespace SharpSoft.App.Client.Core.Services.Contracts;
public interface ILocationService
{
    ValueTask<double[]?> GetCurrentLocation();
    ValueTask<bool> CanUseGeoLocation();
}

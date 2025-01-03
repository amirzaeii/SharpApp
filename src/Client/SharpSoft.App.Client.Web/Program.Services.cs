﻿using SharpSoft.App.Client.Web.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Bit.BlazorUI;

namespace SharpSoft.App.Client.Web;

public static partial class Program
{
    public static void ConfigureServices(this WebAssemblyHostBuilder builder)
    {
        // Services being registered here can get injected in web project only.

        var services = builder.Services;
        var configuration = builder.Configuration;

        configuration.AddClientConfigurations();

        builder.Logging.AddConfiguration(configuration.GetSection("Logging"));

        Uri.TryCreate(configuration.GetApiServerAddress(), UriKind.RelativeOrAbsolute, out var apiServerAddress);

        if (apiServerAddress!.IsAbsoluteUri is false)
        {
            apiServerAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), apiServerAddress);
        }

        services.TryAddTransient(sp => new HttpClient(sp.GetRequiredKeyedService<DelegatingHandler>("DefaultMessageHandler")) { BaseAddress = apiServerAddress });

        services.AddClientWebProjectServices();
    }

    public static void AddClientWebProjectServices(this IServiceCollection services)
    {
        // Services being registered here can get injected in both web project and server (during prerendering).

        services.TryAddTransient<IBitDeviceCoordinator, WebDeviceCoordinator>();
        services.TryAddTransient<IExceptionHandler, WebExceptionHandler>();
        services.TryAddScoped<ILocationService, LocationService>();
        services.AddClientCoreProjectServices();
    }
}

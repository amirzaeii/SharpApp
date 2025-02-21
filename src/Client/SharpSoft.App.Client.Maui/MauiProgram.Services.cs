﻿using SharpSoft.App.Client.Maui.Services;
using Microsoft.Extensions.Logging;

namespace SharpSoft.App.Client.Maui;

public static partial class MauiProgram
{
    public static void ConfigureServices(this MauiAppBuilder builder)
    {
        // Services being registered here can get injected in Maui (Android, iOS, macOS, Windows)

        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddMauiBlazorWebView();

        if (BuildConfiguration.IsDebug())
        {
            services.AddBlazorWebViewDeveloperTools();
        }
       
        //services.AddBlazorWebViewDeveloperTools();

        Uri.TryCreate(configuration.GetApiServerAddress(), UriKind.Absolute, out var apiServerAddress);

        services.TryAddTransient(sp =>
        {
            var handler = sp.GetRequiredKeyedService<DelegatingHandler>("DefaultMessageHandler");
            HttpClient httpClient = new(handler)
            {
                BaseAddress = apiServerAddress
            };
            return httpClient;
        });

        builder.Logging.AddConfiguration(configuration.GetSection("Logging"));

        if (BuildConfiguration.IsDebug())
        {
            builder.Logging.AddDebug();
            builder.Logging.AddConsole();
        }

        if (OperatingSystem.IsWindows())
        {
            builder.Logging.AddEventLog();
        }

        builder.Logging.AddEventSourceLogger();

        
        services.TryAddTransient<MainPage>();
        services.TryAddTransient<IStorageService, MauiStorageService>();
        services.TryAddSingleton<IBitDeviceCoordinator, MauiDeviceCoordinator>();
        services.TryAddTransient<IExceptionHandler, MauiExceptionHandler>();
        services.TryAddTransient<ILocationService, MauiLocationService>();

#if ANDROID
        services.AddClientMauiProjectAndroidServices();
#elif iOS
        services.AddClientMauiProjectIosServices();
#elif Mac
        services.AddClientMauiProjectMacCatalystServices();
#elif Windows
        services.AddClientMauiProjectWindowsServices();
#endif

        services.AddClientCoreProjectServices();
    }
}

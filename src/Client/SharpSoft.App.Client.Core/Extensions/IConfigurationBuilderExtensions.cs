using System.Reflection;

namespace Microsoft.Extensions.Configuration;

public static class IConfigurationBuilderExtensions
{
    public static void AddClientConfigurations(this IConfigurationBuilder builder)
    {
        var assembly = Assembly.Load("SharpSoft.App.Client.Core");
        builder.AddJsonStream(assembly.GetManifestResourceStream("SharpSoft.App.Client.Core.appsettings.json")!);

        if (BuildConfiguration.IsDebug())
        {
            var settings = assembly.GetManifestResourceStream("SharpSoft.App.Client.Core.appsettings.Development.json");
            if (settings is not null)
            {
                builder.AddJsonStream(settings);
            }
        }
        else
        {
            var settings = assembly.GetManifestResourceStream("SharpSoft.App.Client.Core.appsettings.Production.json");
            if (settings is not null)
            {
                builder.AddJsonStream(settings);
            }
        }
    }
}

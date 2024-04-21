using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using SharpSoft.App.Client.Core;
using Java.Net;

namespace SharpSoft.App.Client.Maui.Platforms.Android;

[IntentFilter([Intent.ActionView],
                        DataScheme = "http",
                        DataHost = "135.181.171.147",
                        DataPathPrefix = "/",
                        AutoVerify = true,
                        Categories = [Intent.ActionView, Intent.CategoryDefault, Intent.CategoryBrowsable])]

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleInstance,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var url = Intent?.DataString;
        if (string.IsNullOrWhiteSpace(url) is false)
        {
            var _ = Routes.OpenUniversalLink(new URL(url).File ?? "/");
        }
    }

    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);

        var action = intent!.Action;
        var url = intent.DataString;
        if (action is Intent.ActionView && string.IsNullOrWhiteSpace(url) is false)
        {
            var _ = Routes.OpenUniversalLink(new URL(url).File ?? "/");
        }
    }
}

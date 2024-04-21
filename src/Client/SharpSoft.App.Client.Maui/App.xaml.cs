[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SharpSoft.App.Client.Maui;

public partial class App
{
    public App(MainPage mainPage)
    {
        InitializeComponent();

        MainPage = new NavigationPage(mainPage);
    }
}

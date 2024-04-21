namespace SharpSoft.App.Client.Core.Components.Layout;

public partial class Header
{
    private bool disposed;
    private bool isUserAuthenticated;
    private string? displayName;

    [Parameter] public EventCallback OnToggleMenu { get; set; }
   
    protected override async Task OnInitAsync()
    {
       
        AuthenticationManager.AuthenticationStateChanged += VerifyUserIsAuthenticatedOrNot;

        //isUserAuthenticated = await PrerenderStateService.GetValue(async () => (await AuthenticationStateTask).User.IsAuthenticated());

        displayName = await StorageService.GetItem("DisplayName");
        isUserAuthenticated = displayName != null;
        await base.OnInitAsync();
    }

    async void VerifyUserIsAuthenticatedOrNot(Task<AuthenticationState> task)
    {
        try
        {
            isUserAuthenticated = (await task).User.IsAuthenticated();
            displayName = await StorageService.GetItem("DisplayName");
        }
        catch (Exception ex)
        {
            ExceptionHandler.Handle(ex);
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task ToggleMenu()
    {
        await OnToggleMenu.InvokeAsync();
    }

    protected override async ValueTask DisposeAsync(bool disposing)
    {
        await base.DisposeAsync(disposing);

        if (disposed || disposing is false) return;

        AuthenticationManager.AuthenticationStateChanged -= VerifyUserIsAuthenticatedOrNot;

        disposed = true;
    }
}

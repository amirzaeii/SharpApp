using SharpSoft.App.Iac;
using Pulumi;

public class Program
{
    static Task<int> Main() => Deployment.RunAsync<AppStack>();
}

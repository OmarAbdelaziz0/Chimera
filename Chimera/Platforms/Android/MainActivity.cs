using Android.OS;
using Android.Runtime;
using AndroidX.Lifecycle;
using Grpc.Net.Client;
namespace MauiApp6;
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
    }
}

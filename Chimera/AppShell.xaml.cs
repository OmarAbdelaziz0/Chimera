using Microsoft.Maui.Controls;

namespace MauiApp6
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        }
    }
}

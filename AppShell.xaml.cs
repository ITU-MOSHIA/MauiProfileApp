using Microsoft.Maui.Controls;
using MauiProfileApp.Views;

namespace MauiProfileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        }
    }
}

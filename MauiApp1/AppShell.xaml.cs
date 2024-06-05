using MauiApp1.Pages;

namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));
            RegisterRoutes();
        }

        private readonly static Type[] _routablePageTypes = 
            [
            typeof(SignInPage),
            typeof(MyCountsPage),
            typeof(CountDetailsPage),
            typeof(itemDetailsPage)
            ];

        private static void RegisterRoutes()
        {
            foreach (var pageType in _routablePageTypes)
            {
                Routing.RegisterRoute(pageType.Name, pageType);
            }
        }

        private async void FlyoutFooter_Tapped(object sender, TappedEventArgs e)
        {
            await Launcher.OpenAsync("https://www.facebook.com/BGCiPOS.Net");
        }

        private async void SignOutMenuItem_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.DisplayAlert("Sign Out?", "Are you sure you want to Sign Out?", "Cancel", "OK");
        }
    }
}

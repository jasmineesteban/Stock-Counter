﻿using MauiApp1.Pages;
using System.Diagnostics;


namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private readonly static Type[] _routablePageTypes = 
            [
            typeof(SignInPage),
            typeof(CountSheetsPage),
            typeof(ModalPage),
            typeof(AddItemPage),
             typeof(EmployeeSelectorPage),
            typeof(ItemSelectorPage),
            ];

        private static void RegisterRoutes()
        {
            foreach (var pageType in _routablePageTypes)
            {
                Routing.RegisterRoute(pageType.Name, pageType);
            }
            foreach (var viewType in _routablePageTypes)
            {
                Routing.RegisterRoute(viewType.Name, viewType);
            }
        }

        private async void FlyoutFooter_Tapped(object sender, TappedEventArgs e)
        {
            await Launcher.OpenAsync("https://www.facebook.com/BGCiPOS.Net");
        }

        private async void Exit_Clicked(object sender, EventArgs e)
        {
            bool answer = await Shell.Current.DisplayAlert("Exit App?", "Are you sure you want to Exit?", "Yes", "Cancel");
            if (answer)
            {
#if ANDROID
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif WINDOWS
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
#endif
            }
        }
    }
}

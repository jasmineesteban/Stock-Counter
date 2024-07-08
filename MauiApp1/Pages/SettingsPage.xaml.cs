namespace MauiApp1.Pages
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && e.Item is string selectedOption)
            {
                ((ListView)sender).SelectedItem = null;

                switch (selectedOption)
                {
                    case "Change User":
                        bool userConfirmed = await DisplayAlert("Change User", "Are you sure you want to switch user?", "Yes", "No");

                        if (userConfirmed)
                        {
                            await Shell.Current.GoToAsync($"{nameof(EmployeeSelectorPage)}");
                        }

                        break;
                    case "Contact Us":
                        await Launcher.OpenAsync("https://www.facebook.com/BGCiPOS.Net");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

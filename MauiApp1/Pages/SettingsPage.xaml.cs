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
                // Handle the tapped item here
                switch (selectedOption)
                {
                    case "Change User":
                        // Handle Change Employee action
                        await Shell.Current.GoToAsync(nameof(EmployeeSelectorPage));
                        break;
                    case "Contact Us":
                        await Launcher.OpenAsync("https://www.facebook.com/BGCiPOS.Net");
                        break;
                    default:
                        break;
                }

                // Optionally, deselect the tapped item
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}
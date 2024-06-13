namespace MauiApp1.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    private async void Form_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushModalAsync(new ModalPage());
    }

    private async void Frame_Tapped(object sender, TappedEventArgs e)
    {
        // Navigate to the "Count Sheets" page
        await Navigation.PushAsync(new CountSheetsPage());
    }
}
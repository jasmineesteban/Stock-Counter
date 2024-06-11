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
}
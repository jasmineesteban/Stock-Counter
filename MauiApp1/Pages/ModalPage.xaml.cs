namespace MauiApp1.Pages;

public partial class ModalPage : ContentPage
{
    public ModalPage()
    {
        InitializeComponent();
    }

    private async void Cancel_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopModalAsync();
    }


    private void Save_Clicked(object sender, EventArgs e)
    {
    }
}
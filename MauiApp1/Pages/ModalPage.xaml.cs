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

    private void InventoryDate_DateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime selectedDate = e.NewDate;

        DateTime today = DateTime.Today;

        if (selectedDate > today)
        {
            InventoryDate.Date = today;

            DisplayAlert("Alert", "Please select a date on or before today.", "OK");
        }
    }



    private void Save_Clicked(object sender, EventArgs e)
    {
    }
    private async void OnAppearing(object sender, EventArgs e)
    {
        base.OnAppearing();

        await FadeInModalFrame();
    }


    private async Task FadeInModalFrame()
    {
        this.Opacity = 0;

        await this.FadeTo(1, 250, Easing.Linear);
    }
}
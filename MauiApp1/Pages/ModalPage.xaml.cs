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
        // Get the selected date
        DateTime selectedDate = e.NewDate;

        // Get today's date
        DateTime today = DateTime.Today;

        // Compare selected date with today's date
        if (selectedDate > today)
        {
            // If selected date is in the future, reset to today
            InventoryDate.Date = today;

            // Display alert message
            DisplayAlert("Alert", "Please select a date on or before today.", "OK");
        }
    }



    private void Save_Clicked(object sender, EventArgs e)
    {
    }
}
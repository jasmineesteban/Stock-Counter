using Microsoft.Maui.Controls;

namespace MauiApp1.Pages
{
    [QueryProperty(nameof(EmployeeDetails), "EmployeeDetails")]
    public partial class ModalPage : ContentPage
    {
        private string employeeDetails;
        public string EmployeeDetails
        {
            get => employeeDetails;
            set
            {
                employeeDetails = value;
                OnPropertyChanged();
                EmployeeEntry.Text = employeeDetails;
            }
        }

        public ModalPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await FadeInModalFrame();
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
                DateEntry.Date = today;
                DisplayAlert("Alert", "Please select a date on or before today.", "OK");
            }
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            // Implement save logic here
        }

        private async Task FadeInModalFrame()
        {
            this.Opacity = 0;
            await this.FadeTo(1, 250, Easing.Linear);
        }
    }
}
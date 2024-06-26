using Microsoft.Maui.Controls;

namespace MauiApp1.Pages
{
    [QueryProperty(nameof(EmployeeDetails), "EmployeeDetails")]
    public partial class HomePage : ContentPage
    {
        private string employeeDetails;
        public string EmployeeDetails
        {
            get => employeeDetails;
            set
            {
                employeeDetails = value;
                OnPropertyChanged();
            }
        }

        public HomePage()
        {
            InitializeComponent();
        }

        private async void Form_Clicked(object sender, EventArgs e)
        {
            var modalPage = new ModalPage
            {
                EmployeeDetails = EmployeeDetails
            };
            await Shell.Current.Navigation.PushModalAsync(modalPage);
        }

        private async void Frame_Tapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new CountSheetsPage());
        }
    }
}
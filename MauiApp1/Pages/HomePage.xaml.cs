using Microsoft.Maui.Controls;
using MauiApp1.Models;
using MauiApp1.ViewModels;
using System;

namespace MauiApp1.Pages
{
    [QueryProperty(nameof(EmployeeDetails), "EmployeeDetails")]
    public partial class HomePage : ContentPage
    {
        private readonly IServiceProvider _serviceProvider;

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

        public HomePage(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        private async void Form_Clicked(object sender, EventArgs e)
        {
            var countSheetViewModel = _serviceProvider.GetService<CountSheetViewModel>();
            var modalPage = new ModalPage(countSheetViewModel)
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

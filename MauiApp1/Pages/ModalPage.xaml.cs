using MauiApp1.ViewModels;
using Microsoft.Maui.Controls;
using System;

namespace MauiApp1.Pages
{
    [QueryProperty(nameof(EmployeeDetails), "EmployeeDetails")]
    public partial class ModalPage : ContentPage
    {
        private readonly CountSheetViewModel _countSheetViewModel;
        private string employeeId;

        public string EmployeeDetails
        {
            get => employeeId;
            set
            {
                employeeId = value;
                OnPropertyChanged();
                EmployeeEntry.Text = employeeId;
            }
        }

        public ModalPage(CountSheetViewModel countSheetViewModel)
        {
            InitializeComponent();
            _countSheetViewModel = countSheetViewModel;
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

        private async void Save_Clicked(object sender, EventArgs e)
        {
            var description = CountSheetEntry.Text;
            var date = DateEntry.Date;

            await _countSheetViewModel.AddCountSheet(employeeId, description, date);

            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task FadeInModalFrame()
        {
            this.Opacity = 0;
            await this.FadeTo(1, 250, Easing.Linear);
        }
    }
}

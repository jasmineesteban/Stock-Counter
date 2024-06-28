using Microsoft.Maui.Controls;
using MauiApp1.Models;
using MauiApp1.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiApp1.Pages
{
    [QueryProperty(nameof(EmployeeDetails), "EmployeeDetails")]
    public partial class HomePage : ContentPage
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CountSheetViewModel _countSheetViewModel;

        private string employeeDetails;
        private string employeeId;
        private string employeeName;

        public string EmployeeDetails
        {
            get => employeeDetails;
            set
            {
                employeeDetails = value;
                var details = value.Split(new[] { " - " }, StringSplitOptions.None);
                employeeId = details[0];
                employeeName = details.Length > 1 ? details[1] : string.Empty;
                OnPropertyChanged();
                LoadCountSheets();
            }
        }

        public ObservableCollection<CountSheet> CountSheets { get; set; } = new ObservableCollection<CountSheet>();

        public HomePage(IServiceProvider serviceProvider, CountSheetViewModel countSheetViewModel)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _countSheetViewModel = countSheetViewModel;
            BindingContext = this;
        }

        private async void LoadCountSheets()
        {
            var countSheets = await _countSheetViewModel.ShowCountSheet(employeeId);
            CountSheets.Clear();
            foreach (var sheet in countSheets)
            {
                CountSheets.Add(sheet);
            }
        }

        private async void Form_Clicked(object sender, EventArgs e)
        {
            var modalPage = new ModalPage(_countSheetViewModel)
            {
                EmployeeDetails = EmployeeDetails
            };
            await Shell.Current.Navigation.PushModalAsync(modalPage);
        }

        private async void OnCountSheetTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is CountSheet selectedCountSheet)
            {
                var countSheetsPage = new CountSheetsPage
                {
                    BindingContext = selectedCountSheet,
                    EmployeeDetails = this.EmployeeDetails 
                };
                await Navigation.PushAsync(countSheetsPage);
            }
        }


    }
}

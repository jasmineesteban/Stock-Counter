using MauiApp1.Models;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace MauiApp1.Pages
{
    [QueryProperty(nameof(EmployeeDetails), "EmployeeDetails")]
    public partial class HomePage : ContentPage
    {
        private readonly CountSheetViewModel _countSheetViewModel;
        private readonly ItemCountService _itemCountService; 

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

        public HomePage(CountSheetViewModel countSheetViewModel, ItemCountService itemCountService)
        {
            InitializeComponent();
            _countSheetViewModel = countSheetViewModel;
            _itemCountService = itemCountService;
          
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
                var itemCountViewModel = new ItemCountViewModel(_itemCountService); 
                var countSheetsPage = new CountSheetsPage(itemCountViewModel)
                {
                    BindingContext = selectedCountSheet,
                    EmployeeDetails = this.EmployeeDetails
                };
                await Navigation.PushAsync(countSheetsPage);
            }
        }

    }
}

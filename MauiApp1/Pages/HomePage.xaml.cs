using MauiApp1.Controls;
using MauiApp1.Models;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using System.Collections.ObjectModel;
using MauiApp1.Extensions;

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
            BindingContext = _countSheetViewModel;
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

        private bool isNavigating = false;

        private async void OnCountSheetTapped(object sender, EventArgs e)
        {
            if (isNavigating) return; // Prevents multiple simultaneous navigations
            isNavigating = true;

            try
            {
                if (sender is BindableObject bindable && bindable.BindingContext is CountSheet selectedCountSheet)
                {
                    var grid = (Grid)bindable;
                    grid.BackgroundColor = Colors.LightGray;

                  
                    await Task.Delay(500);
                    grid.BackgroundColor = Colors.White;

                    var itemCountViewModel = new ItemCountViewModel(_itemCountService);
                    var countSheetsPage = new CountSheetsPage(itemCountViewModel, selectedCountSheet.CountCode)
                    {
                        BindingContext = selectedCountSheet,
                        EmployeeDetails = this.EmployeeDetails
                    };
                    await Shell.Current.Navigation.PushAsync(countSheetsPage);
                }
            }
            finally
            {
                isNavigating = false; 
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.BindingContext is CountSheet selectedCountSheet)
            {
                var customEntry = new SelectAllEntry
                {
                    Text = selectedCountSheet.CountDescription
                };

                var page = new ContentPage
                {
                    BackgroundColor = Colors.Transparent,
                    Content = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                {
                    new Label { Text = $"Editing {selectedCountSheet.CountDescription}", TextColor = Colors.White },
                    customEntry
                }
                    }
                };

                await Navigation.PushModalAsync(page);

                string newDescription = await customEntry.GetInputAsync("Edit", "Save", "Cancel");

                await Navigation.PopModalAsync();

                if (!string.IsNullOrEmpty(newDescription))
                {
                    await _countSheetViewModel.EditCountSheet(selectedCountSheet.CountCode, newDescription);
                    await DisplayAlert("Success", $"Updated {selectedCountSheet.CountDescription} to {newDescription}", "OK");
                    LoadCountSheets();
                }
            }
        }


        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.BindingContext is CountSheet selectedCountSheet)
            {
                bool answer = await DisplayAlert("Delete", $"Are you sure you want to delete {selectedCountSheet.CountDescription}?", "Yes", "No");
                if (answer)
                {
                    try
                    {
                        await _countSheetViewModel.DeleteCountSheet(selectedCountSheet.CountCode);
                        await DisplayAlert("Success", $"Deleted {selectedCountSheet.CountDescription}", "OK");
                        LoadCountSheets(); 
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Failed to delete: {ex.Message}", "OK");
                    }
                }
            }
        }
    }
}

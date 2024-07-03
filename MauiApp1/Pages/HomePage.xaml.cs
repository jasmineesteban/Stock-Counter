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
            if (isNavigating) return; 
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
                    var countSheetsPage = new CountSheetsPage(itemCountViewModel, selectedCountSheet.CountCode, 0)
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
                var descriptionEntry = new Entry
                {
                    Text = selectedCountSheet.CountDescription,
                    HorizontalOptions = LayoutOptions.Fill
                };

                var saveButton = new Button
                {
                    Text = "Save",
                    BackgroundColor = Color.FromRgb(173, 216, 230),
                    TextColor = Colors.White,
                    WidthRequest = 100,
                    HeightRequest = 40
                };

                var cancelButton = new Button
                {
                    Text = "Cancel",
                    BackgroundColor = Color.FromRgb(255, 127, 127),
                    TextColor = Colors.White,
                    WidthRequest = 100,
                    HeightRequest = 40
                };

                var buttonStack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 20,
                    Children = { saveButton, cancelButton }
                };

                var page = new ContentPage
                {
                    BackgroundColor = new Color(0, 0, 0, 0.1f), // Semi-transparent black
                    Content = new Frame
                    {
                        BackgroundColor = Colors.White,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Padding = new Thickness(20),
                        WidthRequest = 300, // Set a fixed width for the frame
                        Content = new StackLayout
                        {
                            Spacing = 10,
                            Children =
                    {
                        new Label { Text = $"Editing {selectedCountSheet.CountDescription}", FontAttributes = FontAttributes.Bold },
                        new Label { Text = "Description" },
                        descriptionEntry,
                        buttonStack
                    }
                        }
                    }
                };

                var tcs = new TaskCompletionSource<bool>();

                saveButton.Clicked += async (s, args) =>
                {
                    string newDescription = descriptionEntry.Text;
                    if (!string.IsNullOrEmpty(newDescription))
                    {
                        await _countSheetViewModel.EditCountSheet(selectedCountSheet.CountCode, newDescription);
                        await DisplayAlert("Success", $"Updated {selectedCountSheet.CountDescription} to {newDescription}", "OK");
                        LoadCountSheets();
                        tcs.SetResult(true);
                    }
                    else
                    {
                        await DisplayAlert("Error", "Description cannot be empty", "OK");
                    }
                };

                cancelButton.Clicked += (s, args) => tcs.SetResult(false);

                await Navigation.PushModalAsync(page);
                bool result = await tcs.Task;
                await Navigation.PopModalAsync();
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

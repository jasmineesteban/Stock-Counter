using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiApp1.Models;
using MauiApp1.ViewModels;

namespace MauiApp1.Helpers
{
    public class EditCountDialogHelper
    {
        private readonly CountSheetViewModel _countSheetViewModel;

        public EditCountDialogHelper(CountSheetViewModel countSheetViewModel)
        {
            _countSheetViewModel = countSheetViewModel;
        }

        public async Task EditCountSheetDialog(CountSheet selectedCountSheet)
        {
            var descriptionEntry = new Entry
            {
                Text = selectedCountSheet.CountDescription,
                HorizontalOptions = LayoutOptions.Fill
            };

            var saveButton = new Button
            {
                Text = "Save",
                BackgroundColor = Color.FromHex("#0066CC"),
                TextColor = Colors.White,
                WidthRequest = 100,
                HeightRequest = 40
            };

            var buttonStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,
                Children = { saveButton }
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
                    try
                    {
                        await _countSheetViewModel.EditCountSheet(selectedCountSheet.CountCode, newDescription);
                        var toast = Toast.Make($"Updated {selectedCountSheet.CountDescription} to {newDescription}", ToastDuration.Short);
                        await toast.Show();
                        tcs.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", $"Failed to update: {ex.Message}", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Description cannot be empty", "OK");
                }
            };

            await Application.Current.MainPage.Navigation.PushModalAsync(page);
            await tcs.Task;
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}

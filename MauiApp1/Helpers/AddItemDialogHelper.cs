using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiApp1.Services;
using MauiApp1.ViewModels;

namespace MauiApp1.Helpers
{
    public static class AddItemDialogHelper
    {
        public static async Task<bool> AddItem(string itemCode, string itemDescription, string itemUom, string countCode, ItemCountViewModel itemCountViewModel, HttpClientService httpClientService)
        {
            var itemQuantityEntry = new Entry { Placeholder = "Enter quantity", HorizontalOptions = LayoutOptions.Fill, Keyboard = Keyboard.Numeric };
            var itemBatchLotNumberEntry = new Entry { Placeholder = "Enter batch & lot number", HorizontalOptions = LayoutOptions.Fill };
            var itemExpiryEntry = new Entry
            {
                Placeholder = "YYYY-MM-DD",
                HorizontalOptions = LayoutOptions.Fill,
                Keyboard = Keyboard.Numeric,
            };

            itemExpiryEntry.TextChanged += ExpiryEntry_TextChanged;

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
                BackgroundColor = new Color(0, 0, 0, 0.1f),
                Content = new Frame
                {
                    BackgroundColor = Colors.White,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Padding = new Thickness(20),
                    WidthRequest = 300,
                    Content = new StackLayout
                    {
                        Spacing = 10,
                        Children =
                        {
                            new Label { Text = "Add New Product", FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center, FontSize = 18},
                            new Label { Text = $"Code: {itemCode}", FontAttributes = FontAttributes.Bold },
                            new Label { Text = $"Description: {itemDescription}", FontAttributes = FontAttributes.Bold },
                            new Label { Text = $"UOM: {itemUom}", FontAttributes = FontAttributes.Bold },
                            new Label
                            {
                                FormattedText = new FormattedString
                                {
                                    Spans =
                                    {
                                        new Span { Text = "Quantity", FontAttributes = FontAttributes.Bold },
                                        new Span { Text = " *", TextColor = Colors.Red }
                                    }
                                }
                            },
                            itemQuantityEntry,
                            new Label { Text = "Batch & Lot Number", FontAttributes = FontAttributes.Bold },
                            itemBatchLotNumberEntry,
                            new Label { Text = "Expiry (YYYY-MM-DD)", FontAttributes = FontAttributes.Bold },
                            itemExpiryEntry,
                            buttonStack
                        }
                    }
                }
            };

            var tcs = new TaskCompletionSource<bool>();

            saveButton.Clicked += async (s, args) =>
            {
                string itemBatchLotNumber = itemBatchLotNumberEntry.Text;
                string itemExpiry = itemExpiryEntry.Text;
                string itemQuantityString = itemQuantityEntry.Text;

                if (string.IsNullOrEmpty(itemBatchLotNumber))
                {
                    itemBatchLotNumber = "N.A";
                }
                if (string.IsNullOrEmpty(itemExpiry))
                {
                    itemExpiry = "0000-00-00";
                }

                if (!string.IsNullOrEmpty(itemQuantityString) && int.TryParse(itemQuantityString, out int itemQuantity))
                {
                    if (itemQuantity < 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Quantity cannot be negative.", "OK");
                        return;
                    }

                    bool success = await itemCountViewModel.AddItemCount(countCode, itemCode, itemDescription, itemUom, itemBatchLotNumber, itemExpiry, itemQuantity);
                    if (success)
                    {
                        var toast = Toast.Make($"{itemDescription} added", ToastDuration.Short);
                        await toast.Show();
                        tcs.SetResult(true);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to add item", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please fill all fields correctly.", "OK");
                }
            };

            await Application.Current.MainPage.Navigation.PushModalAsync(page);
            bool result = await tcs.Task;
            await Application.Current.MainPage.Navigation.PopModalAsync();

            return result;
        }

        private static void ExpiryEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            EntryFormatHelper.ExpiryEntry_TextChanged(sender, e);
        }
    }
}

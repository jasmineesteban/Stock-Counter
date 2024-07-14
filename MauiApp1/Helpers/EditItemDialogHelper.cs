using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiApp1.Models;
using MauiApp1.ViewModels;


namespace MauiApp1.Helpers
{
    public static class EditItemDialogHelper
    {
        public static async Task EditItem(ItemCount selectedItemCount, ItemCountViewModel itemCountViewModel)
        {
            var batchAndLotEntry = new Entry { Text = selectedItemCount.ItemBatchLotNumber, HorizontalOptions = LayoutOptions.Fill };
            var expiryEntry = new Entry
            {
                Text = selectedItemCount.ItemExpiry,
                HorizontalOptions = LayoutOptions.Fill,
                Placeholder = "YYYY-MM-DD",
                Keyboard = Keyboard.Numeric
            };
            expiryEntry.TextChanged += ExpiryEntry_TextChanged;

            var quantityEntry = new Entry { Text = selectedItemCount.ItemQuantity.ToString(), HorizontalOptions = LayoutOptions.Fill, Keyboard = Keyboard.Numeric };

            quantityEntry.Focus();
            quantityEntry.Completed += (s, e) =>
            {
                batchAndLotEntry.Focus();
                batchAndLotEntry.CursorPosition = 0;
                batchAndLotEntry.SelectionLength = batchAndLotEntry.Text.Length;
            };
            batchAndLotEntry.Completed += (s, e) =>
            {
                expiryEntry.Focus();
                expiryEntry.CursorPosition = 0;
                expiryEntry.SelectionLength = expiryEntry.Text.Length;
            };
            expiryEntry.Completed += (s, e) =>
            {
                expiryEntry.Unfocus();
                expiryEntry.CursorPosition = 0;
                expiryEntry.SelectionLength = expiryEntry.Text.Length;
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
                            new Label
                            {
                                Text = "Edit Product",
                                FontAttributes = FontAttributes.Bold,
                                HorizontalOptions = LayoutOptions.Center,
                                FontSize = 18
                            },
                            new Label
                            {
                                FormattedText = new FormattedString
                                {
                                    Spans =
                                    {
                                        new Span { Text = "Code: ", FontAttributes = FontAttributes.Bold },
                                        new Span { Text = $"{selectedItemCount.ItemCode}\n\n" },
                                        new Span { Text = "Description: ", FontAttributes = FontAttributes.Bold },
                                        new Span { Text = $"{selectedItemCount.ItemDescription}\n\n" },
                                        new Span { Text = "UOM: ", FontAttributes = FontAttributes.Bold },
                                        new Span { Text = $"{selectedItemCount.ItemUom}" }
                                    }
                                }
                            },
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
                            quantityEntry,
                            new Label { Text = "Batch & Lot", FontAttributes = FontAttributes.Bold },
                            batchAndLotEntry,
                            new Label { Text = "Expiry (YYYY-MM-DD)", FontAttributes = FontAttributes.Bold },
                            expiryEntry,
                            buttonStack
                        }
                    }
                }
            };


            var tcs = new TaskCompletionSource<bool>();

            saveButton.Clicked += async (s, args) =>
            {
                string newBatchAndLot = batchAndLotEntry.Text;
                string newExpiry = expiryEntry.Text;
                string newQuantityString = quantityEntry.Text;

                if (!string.IsNullOrEmpty(newBatchAndLot) || !string.IsNullOrEmpty(newExpiry) || !string.IsNullOrEmpty(newQuantityString))
                {

                    if (int.TryParse(newQuantityString, out int newQuantity))
                    {
                        if (newQuantity < 0)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Quantity cannot be negative.", "OK");
                            return;
                        }

                        await itemCountViewModel.EditItemCount(selectedItemCount.ItemKey, newBatchAndLot, newExpiry, newQuantity);
                        var toast = Toast.Make($"{selectedItemCount.ItemDescription} Updated", ToastDuration.Short);
                        await toast.Show();

                        quantityEntry.Unfocus();
                        batchAndLotEntry.Unfocus();
                        expiryEntry.Unfocus();

                        tcs.SetResult(true);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Invalid quantity.", "OK");
                    }
                }
            };

            await Application.Current.MainPage.Navigation.PushModalAsync(page);

            Device.BeginInvokeOnMainThread(() =>
            {
                quantityEntry.Focus();
                quantityEntry.CursorPosition = 0;
                quantityEntry.SelectionLength = quantityEntry.Text.Length;
            });

            bool result = await tcs.Task;
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private static void ExpiryEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            EntryFormatHelper.ExpiryEntry_TextChanged(sender, e);
        }
    }
}

using MauiApp1.ViewModels;
using MauiApp1.Services;
using Microsoft.Maui.Controls;
using System;
using System.Linq;

namespace MauiApp1.Pages
{
    [QueryProperty(nameof(ItemDescription), "ItemDescription")]
    [QueryProperty(nameof(SellingUom), "SellingUom")]
    [QueryProperty(nameof(ItemNumber), "ItemNumber")]
    public partial class AddItemPage : ContentPage
    {
        private readonly ItemCountViewModel _itemCountViewModel;
        private bool hasNavigatedToItemSelector = false;
        public string CountCode { get; set; }

        public string? ItemDescription
        {
            set => EntryProductName.Text = value;
        }

        public string? SellingUom
        {
            set => EntryUOM.Text = value;
        }

        public string? ItemNumber
        {
            set => EntryItemCode.Text = value;
        }

        public AddItemPage(string countCode, ItemCountViewModel itemCountViewModel)
        {
            InitializeComponent();
            CountCode = countCode;
            _itemCountViewModel = itemCountViewModel;
        }

        protected override async void OnAppearing()
        {
            if (!hasNavigatedToItemSelector)
            {
                hasNavigatedToItemSelector = true;
                await Task.Delay(10); // Delay if needed
                await Shell.Current.GoToAsync(nameof(ItemSelectorPage));
            }
        }

            private async void AddItem_Clicked(object sender, EventArgs e)
        {
            // Gather all necessary data from the entries
            string itemCode = EntryItemCode.Text;
            string itemDescription = EntryProductName.Text;
            string itemUom = EntryUOM.Text;
            string itemBatchLotNumber = EntryBatchNo.Text;
            string itemExpiry = EntryExpiryDate.Text;
            int itemQuantity = int.Parse(EntryQuantity.Text);

            // Call the ViewModel to add the item count
            bool result = await _itemCountViewModel.AddItemCount(CountCode, itemCode, itemDescription, itemUom, itemBatchLotNumber, itemExpiry, itemQuantity);

            if (result)
            {
                // Send a message to refresh the CountSheetsPage
                MessagingCenter.Send(this, "RefreshItemCount", CountCode);

                await Navigation.PopAsync(); // Navigate back to the previous page
            }
            else
            {
                await DisplayAlert("Error", "Failed to add item count.", "OK");
            }
        }

        private void EntryExpiryDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            var text = entry.Text;
            // Remove any non-numeric characters
            string cleanedText = new string(text.Where(char.IsDigit).ToArray());
            // Handle max length
            if (cleanedText.Length > 8)
            {
                cleanedText = cleanedText.Substring(0, 8);
            }
            // Format the cleaned text to ####-##-##
            if (cleanedText.Length > 4 && cleanedText[4] != '-')
            {
                cleanedText = cleanedText.Insert(4, "-");
            }
            if (cleanedText.Length > 7 && cleanedText[7] != '-')
            {
                cleanedText = cleanedText.Insert(7, "-");
            }
            // Update the Entry text and cursor position
            entry.Text = cleanedText;
            entry.CursorPosition = cleanedText.Length;
        }

        private async void ToItemSelector_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ItemSelectorPage));
        }
    }
}
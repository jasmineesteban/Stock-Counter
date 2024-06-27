using MauiApp1.Services;
using MauiApp1.ViewModels;
using MauiApp1.Helpers;
using MauiApp1.Services.HttpBaseService;
using Microsoft.Extensions.DependencyInjection;


namespace MauiApp1.Pages;

[QueryProperty(nameof(ItemDescription), "ItemDescription")]
[QueryProperty(nameof(SellingUom), "SellingUom")]
[QueryProperty(nameof(ItemNumber), "ItemNumber")]
public partial class AddItemPage : ContentPage
{
    
    private readonly ItemCountViewModel _itemCountViewModel;
    private readonly ItemCountService itemCountService;
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


    public AddItemPage(ItemCountViewModel itemCountViewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _itemCountViewModel = itemCountViewModel;

    }

    private async void AddItem_Clicked(object sender, EventArgs e)
    {
        var itemno = EntryItemCode.Text;
        var description = EntryProductName.Text;
        var uom = EntryUOM.Text;
        var batchlot = EntryBatchNo.Text;
        var expiry = EntryExpiryDate.Text;
        var countcode = EntryCountCode.Text;

        if (int.TryParse(EntryQuantity.Text, out var quantity))
        {
            try
            {
                await _itemCountViewModel.AddItemCount(itemno, description, uom, batchlot, expiry, quantity, countcode);
                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Quantity must be a valid number.", "OK");
        }
    }

    private void EntryExpiryDate_TextChanged(object sender, TextChangedEventArgs e)
    {
        //var entry = (Entry)sender;
        //var text = entry.Text;

        //string cleanedText = new string(text.Where(char.IsDigit).ToArray());

        //if (cleanedText.Length > 8)
        //{
        //    cleanedText = cleanedText.Substring(0, 8);
        //}
        //if (cleanedText.Length > 4 && cleanedText[4] != '-')
        //{
        //    cleanedText = cleanedText.Insert(4, "-");
        //}
        //if (cleanedText.Length > 7 && cleanedText[7] != '-')
        //{
        //    cleanedText = cleanedText.Insert(7, "-");
        //}

        //entry.Text = cleanedText;
        //entry.CursorPosition = cleanedText.Length;
    }

    private async void ToItemSelector_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ItemSelectorPage));
    }

    private void EntryQuantity_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        if (entry != null)
        {
            string newText = e.NewTextValue;

            string filteredText = new string(newText.Where(char.IsDigit).ToArray());

            if (newText != filteredText)
            {
                entry.Text = filteredText;
            }
        }
    }
}
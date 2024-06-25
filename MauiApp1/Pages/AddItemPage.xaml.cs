using System;
using Microsoft.Maui.Controls;
using ZXing.Mobile;
using MauiApp1.Services;

namespace MauiApp1.Pages;

[QueryProperty(nameof(ItemDescription), "ItemDescription")]
[QueryProperty(nameof(SellingUom), "SellingUom")]
public partial class AddItemPage : ContentPage
{

    private readonly HttpClientService _httpClientService;

    public string? ItemDescription
    {
        set => EntryProductName.Text = value;
    }

    public string? SellingUom
    {
        set => EntryUOM.Text = value;
    }

    public AddItemPage()
    {
        InitializeComponent();

    }

    private void AddItem_Clicked(object sender, EventArgs e)
    {


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
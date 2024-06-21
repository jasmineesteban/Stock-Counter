using System;
using Microsoft.Maui.Controls;
using ZXing.Mobile;

namespace MauiApp1.Pages;

public partial class AddItemPage : ContentPage
{

    public AddItemPage()
    {
        InitializeComponent();
    }

    private void AddItem_Clicked(object sender, EventArgs e)
    {

    }

    private async void Scan_Clicked(object sender, EventArgs e)
    {
        var options = new MobileBarcodeScanningOptions
        {
            AutoRotate = true,
            UseFrontCameraIfAvailable = false,
        };


        var scanner = new MobileBarcodeScanner
        {
            TopText = "Hold the camera up to the barcode",
            BottomText = "Scanning will happen automatically",
        };

        var result = await scanner.Scan(options);
        if (result != null)
        {
            EntryBarcode.Text = result.Text;
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
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
}
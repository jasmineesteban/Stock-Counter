 using System;
using Microsoft.Maui.Controls;
using ZXing.Net.Maui;
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
            UseFrontCameraIfAvailable = false
        };

        var scanner = new MobileBarcodeScanner
        {
            TopText = "Hold the camera up to the barcode",
            BottomText = "Scanning will happen automatically"
        };

        var result = await scanner.Scan(options);
        if (result != null)
        {
            EntryBarcode.Text = result.Text;
        }
    }

}
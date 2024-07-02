using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace MauiApp1.Pages
{
    public partial class ColumnSelectionPage : ContentPage
    {
        private CountSheetsPage _countSheetsPage;

        public ColumnSelectionPage(CountSheetsPage countSheetsPage)
        {
            InitializeComponent();
            _countSheetsPage = countSheetsPage;
            BindingContext = this;
        }

        //public bool ShowCtr
        //{
        //    get => _countSheetsPage.ShowCtr;
        //    set => _countSheetsPage.ShowCtr = value;
        //}

        //public bool ShowItemNo
        //{
        //    get => _countSheetsPage.ShowItemNo;
        //    set => _countSheetsPage.ShowItemNo = value;
        //}

        //public bool ShowDescription
        //{
        //    get => _countSheetsPage.ShowDescription;
        //    set => _countSheetsPage.ShowDescription = value;
        //}

        //public bool ShowUom
        //{
        //    get => _countSheetsPage.ShowUom;
        //    set => _countSheetsPage.ShowUom = value;
        //}

        //public bool ShowBatchLot
        //{
        //    get => _countSheetsPage.ShowBatchLot;
        //    set => _countSheetsPage.ShowBatchLot = value;
        //}

        //public bool ShowExpiry
        //{
        //    get => _countSheetsPage.ShowExpiry;
        //    set => _countSheetsPage.ShowExpiry = value;
        //}

        //public bool ShowQuantity
        //{
        //    get => _countSheetsPage.ShowQuantity;
        //    set => _countSheetsPage.ShowQuantity = value;
        //}

        private async void Save_Clicked(object sender, EventArgs e)
        {
            // Close the column selection page
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async void OnAppearing(object sender, EventArgs e)
        {
            base.OnAppearing();
            await FadeInModalFrame();
        }

        private async Task FadeInModalFrame()
        {
            this.Opacity = 0;
            await this.FadeTo(1, 250, Easing.Linear);
        }
    }
}

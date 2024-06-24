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

        public bool ShowName
        {
            get => _countSheetsPage.ShowName;
            set => _countSheetsPage.ShowName = value;
        }

        public bool ShowQuantity
        {
            get => _countSheetsPage.ShowQuantity;
            set => _countSheetsPage.ShowQuantity = value;
        }

        public bool ShowUOM
        {
            get => _countSheetsPage.ShowUOM;
            set => _countSheetsPage.ShowUOM = value;
        }

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
namespace MauiApp1.Pages
{
    public partial class ColumnSelectionPage : ContentPage
    {
        private CountSheetsPage _countSheetsPage;
        private Dictionary<string, bool> _tempSettings;

        public ColumnSelectionPage(CountSheetsPage countSheetsPage)
        {
            InitializeComponent();
            _countSheetsPage = countSheetsPage;
            _tempSettings = new Dictionary<string, bool>
            {
                {"ShowItemNo", _countSheetsPage.ColumnVisibility.ShowItemNo},
                {"ShowDescription", _countSheetsPage.ColumnVisibility.ShowDescription},
                {"ShowUom", _countSheetsPage.ColumnVisibility.ShowUom},
                {"ShowBatchLot", _countSheetsPage.ColumnVisibility.ShowBatchLot},
                {"ShowExpiry", _countSheetsPage.ColumnVisibility.ShowExpiry},
                {"ShowQuantity", _countSheetsPage.ColumnVisibility.ShowQuantity}
            };
            BindingContext = this;
        }

        public bool ShowItemNo
        {
            get => _tempSettings["ShowItemNo"];
            set
            {
                _tempSettings["ShowItemNo"] = value;
                OnPropertyChanged(nameof(ShowItemNo));
            }
        }

        public bool ShowDescription
        {
            get => _tempSettings["ShowDescription"];
            set
            {
                _tempSettings["ShowDescription"] = value;
                OnPropertyChanged(nameof(ShowDescription));
            }
        }

        public bool ShowUom
        {
            get => _tempSettings["ShowUom"];
            set
            {
                _tempSettings["ShowUom"] = value;
                OnPropertyChanged(nameof(ShowUom));
            }
        }

        public bool ShowBatchLot
        {
            get => _tempSettings["ShowBatchLot"];
            set
            {
                _tempSettings["ShowBatchLot"] = value;
                OnPropertyChanged(nameof(ShowBatchLot));
            }
        }

        public bool ShowExpiry
        {
            get => _tempSettings["ShowExpiry"];
            set
            {
                _tempSettings["ShowExpiry"] = value;
                OnPropertyChanged(nameof(ShowExpiry));
            }
        }

        public bool ShowQuantity
        {
            get => _tempSettings["ShowQuantity"];
            set
            {
                _tempSettings["ShowQuantity"] = value;
                OnPropertyChanged(nameof(ShowQuantity));
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
          
            ((Button)sender).IsEnabled = false;

         
            SaveLoadingIndicator.IsRunning = true;
            SaveLoadingIndicator.IsVisible = true;

            try
            {
             
                await Task.Run(() => _countSheetsPage.ApplyColumnSettings(_tempSettings));
               
                await Shell.Current.Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
            
                await DisplayAlert("Error", $"Failed to save settings: {ex.Message}", "OK");
            }
            finally
            {
             
                SaveLoadingIndicator.IsRunning = false;
                SaveLoadingIndicator.IsVisible = false;

           
                ((Button)sender).IsEnabled = true;
            }

            await FadeOutModalFrame();
        }

        private async void OnAppearing(object sender, EventArgs e)
        {
            base.OnAppearing();
            await FadeInModalFrame();
        }

        private async Task FadeInModalFrame()
        {
            this.Opacity = 0;
            await this.FadeTo(1, 500, Easing.CubicInOut);
        }

        private async Task FadeOutModalFrame()
        {
            await this.FadeTo(0, 500, Easing.CubicInOut);
        }
    }
}
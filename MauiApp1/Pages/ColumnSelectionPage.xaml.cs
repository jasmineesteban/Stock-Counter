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
                {"ShowCtr", _countSheetsPage.ShowCtr},
                {"ShowItemNo", _countSheetsPage.ShowItemNo},
                {"ShowDescription", _countSheetsPage.ShowDescription},
                {"ShowUom", _countSheetsPage.ShowUom},
                {"ShowBatchLot", _countSheetsPage.ShowBatchLot},
                {"ShowExpiry", _countSheetsPage.ShowExpiry},
                {"ShowQuantity", _countSheetsPage.ShowQuantity}
            };
            BindingContext = this;
        }

        public bool ShowCtr
        {
            get => _tempSettings["ShowCtr"];
            set
            {
                _tempSettings["ShowCtr"] = value;
                OnPropertyChanged(nameof(ShowCtr));
            }
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
            // Disable the save button to prevent multiple clicks
            ((Button)sender).IsEnabled = false;

            // Show loading indicator
            SaveLoadingIndicator.IsRunning = true;
            SaveLoadingIndicator.IsVisible = true;

            try
            {
                // Apply settings asynchronously
                await Task.Run(() => _countSheetsPage.ApplyColumnSettings(_tempSettings));
                // Close the modal page
                await Shell.Current.Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                // Handle any errors
                await DisplayAlert("Error", $"Failed to save settings: {ex.Message}", "OK");
            }
            finally
            {
                // Hide loading indicator
                SaveLoadingIndicator.IsRunning = false;
                SaveLoadingIndicator.IsVisible = false;

                // Re-enable the save button
                ((Button)sender).IsEnabled = true;
            }
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
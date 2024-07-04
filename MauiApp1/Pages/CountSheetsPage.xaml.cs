
using MauiApp1.Helpers;
using MauiApp1.Models;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using System.Collections.ObjectModel;

namespace MauiApp1.Pages
{
    [QueryProperty(nameof(ItemDescription), "ItemDescription")]
    [QueryProperty(nameof(SellingUom), "SellingUom")]
    [QueryProperty(nameof(ItemNumber), "ItemNumber")]
    [QueryProperty(nameof(EmployeeDetails), "EmployeeDetails")]
    public partial class CountSheetsPage : ContentPage
    {
        private string? _itemDescription;
        public string? ItemDescription
        {
            get => _itemDescription;
            set => _itemDescription = value;
        }

        private string? _sellingUom;
        public string? SellingUom
        {
            get => _sellingUom;
            set => _sellingUom = value;
        }

        private string? _itemNumber;
        public string? ItemNumber
        {
            get => _itemNumber;
            set => _itemNumber = value;
        }

       



        private bool _showCtr;
        public bool ShowCtr
        {
            get => _showCtr;
            set
            {
                _showCtr = value;
                OnPropertyChanged(nameof(ShowCtr));
                UpdateColumnVisibility();
            }
        }

        private bool _showItemNo;
        public bool ShowItemNo
        {
            get => _showItemNo;
            set
            {
                _showItemNo = value;
                OnPropertyChanged(nameof(ShowItemNo));
                UpdateColumnVisibility();
            }
        }

        private bool _showDescription;
        public bool ShowDescription
        {
            get => _showDescription;
            set
            {
                _showDescription = value;
                OnPropertyChanged(nameof(ShowDescription));
                UpdateColumnVisibility();
            }
        }

        private bool _showUom;
        public bool ShowUom
        {
            get => _showUom;
            set
            {
                _showUom = value;
                OnPropertyChanged(nameof(ShowUom));
                UpdateColumnVisibility();
            }
        }

        private bool _showBatchLot;
        public bool ShowBatchLot
        {
            get => _showBatchLot;
            set
            {
                _showBatchLot = value;
                OnPropertyChanged(nameof(ShowBatchLot));
                UpdateColumnVisibility();
            }
        }

        private bool _showExpiry;
        public bool ShowExpiry
        {
            get => _showExpiry;
            set
            {
                _showExpiry = value;
                OnPropertyChanged(nameof(ShowExpiry));
                UpdateColumnVisibility();
            }
        }

        private bool _showQuantity;
        public bool ShowQuantity
        {
            get => _showQuantity;
            set
            {
                _showQuantity = value;
                OnPropertyChanged(nameof(ShowQuantity));
                UpdateColumnVisibility();
            }
        }

        private void UpdateColumnVisibility()
        {
            GridColumnVisibilityHelper.UpdateColumnVisibility(HeaderGrid, dataGrid, ShowCtr, ShowItemNo, ShowDescription, ShowUom, ShowBatchLot, ShowExpiry, ShowQuantity, this);
        }

        private string _employeeDetails;
        private string _employeeId;  
        public string EmployeeDetails
        {
            get => _employeeDetails;
            set
            {
                _employeeDetails = value;
                OnPropertyChanged(nameof(EmployeeDetails));

                var details = value.Split(new[] { " - " }, StringSplitOptions.None);
                _employeeId = details[0];
                var employeeName = details.Length > 1 ? details[1] : string.Empty;
                EmployeeDetailsLabel.Text = employeeName;
            }
        }

        public ObservableCollection<ItemCount> ItemCount { get; set; }
        private ItemCountViewModel _itemCountViewModel;
        private readonly string _countCode;
        private int _sort = 0;
        private int tapCount = 0;

        private readonly HttpClientService _httpClientService;

        public void ApplyColumnSettings(Dictionary<string, bool> settings)
        {
            ShowCtr = settings["ShowCtr"];
            ShowItemNo = settings["ShowItemNo"];
            ShowDescription = settings["ShowDescription"];
            ShowUom = settings["ShowUom"];
            ShowBatchLot = settings["ShowBatchLot"];
            ShowExpiry = settings["ShowExpiry"];
            ShowQuantity = settings["ShowQuantity"];

            UpdateColumnVisibility();
        }

        public CountSheetsPage(ItemCountViewModel itemCountViewModel, string countCode, int sortValue, HttpClientService httpClientService)
        {
            InitializeComponent();
            _itemCountViewModel = itemCountViewModel;
            ItemCount = new ObservableCollection<ItemCount>();
            _countCode = countCode;
            _httpClientService = httpClientService;
            BindingContext = this;

            ShowCtr = true;
            ShowItemNo = true;
            ShowDescription = true;
            ShowUom = true;
            ShowBatchLot = true;
            ShowExpiry = true;
            ShowQuantity = true;
            dataGrid.ItemsSource = ItemCount;
            UpdateColumnVisibility();

            LoadItemCountData();
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnHeaderGridTapped;
            HeaderGrid.GestureRecognizers.Add(tapGesture);

            _sort = sortValue;
        }

        private void OnHeaderGridTapped(object sender, EventArgs e)
        {
            tapCount++;

            if (tapCount == 1)
            {
                _sort = 1;
            }
            else if (tapCount == 2)
            {
                _sort = 2;
            }
            else
            {
                // Reset 
                tapCount = 0;
                _sort = 0;
            }

            LoadItemCountData();
        }

        private async void LoadItemCountData()
        {
            try
            {
                // Show loading indicator
                LoadingIndicator.IsRunning = true;
                LoadingIndicator.IsVisible = true;

                var items = await _itemCountViewModel.ShowItemCount(_countCode, _sort);

                ItemCount.Clear();
                foreach (var item in items)
                {
                    ItemCount.Add(item);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load items: {ex.Message}", "OK");
            }
            finally
            {
                // Hide loading indicator
                LoadingIndicator.IsRunning = false;
                LoadingIndicator.IsVisible = false;
            }
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            var itemViewModel2 = new ItemViewModel2(_httpClientService);
            var itemSelectorPage2 = new ItemSelectorPage2(itemViewModel2);
            itemSelectorPage2.Disappearing += (s, args) =>
            {
                AddItem();
            };
            await Shell.Current.Navigation.PushAsync(itemSelectorPage2);
        }


        private async void Filter_Clicked(object sender, EventArgs e)
        {
            // Implement your filter logic here
            await Shell.Current.Navigation.PushModalAsync(new ColumnSelectionPage(this));
        }

        private async void ColumnSelection_Clicked(object sender, EventArgs e)
        {
            var columnSelectionPage = new ColumnSelectionPage(this);
            await Shell.Current.Navigation.PushModalAsync(new NavigationPage(columnSelectionPage));
        }


        internal async void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.BindingContext is ItemCount selectedItemCount)
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

                var saveButton = new Button
                {
                    Text = "Save",
                    BackgroundColor = Color.FromRgb(173, 216, 230),
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
                    BackgroundColor = new Color(0, 0, 0, 0.1f), // Semi-transparent black
                    Content = new Frame
                    {
                        BackgroundColor = Colors.White,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Padding = new Thickness(20),
                        WidthRequest = 300, // Set a fixed width for the frame
                        Content = new StackLayout
                        {
                            Spacing = 10,
                            Children =
                    {
                        new Label { Text = $"Editing\n{selectedItemCount.ItemCode}\t{selectedItemCount.ItemDescription}", FontAttributes = FontAttributes.Bold },
                        new Label { Text = "Batch & Lot" },
                        batchAndLotEntry,
                        new Label { Text = "Expiry (YYYY-MM-DD)" },
                        expiryEntry,
                        new Label { Text = "Quantity" },
                        quantityEntry,
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
                                await DisplayAlert("Error", "Quantity cannot be negative.", "OK");
                                return;
                            }

                            await _itemCountViewModel.EditItemCount(selectedItemCount.ItemKey, newBatchAndLot, newExpiry, newQuantity);
                            await DisplayAlert("Success", $"Updated {selectedItemCount.ItemDescription}", "OK");
                            LoadItemCountData();
                            tcs.SetResult(true);
                        }
                        else
                        {
                            await DisplayAlert("Error", "Invalid quantity entered", "OK");
                        }
                    }
                };

                await Navigation.PushModalAsync(page);
                bool result = await tcs.Task;
                await Navigation.PopModalAsync();
            }
        }

        private void ExpiryEntry_TextChanged(object sender, TextChangedEventArgs e)
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

            // Format the cleaned text to YYYY-MM-DD
            if (cleanedText.Length > 4)
            {
                cleanedText = cleanedText.Insert(4, "-");
            }
            if (cleanedText.Length > 7)
            {
                cleanedText = cleanedText.Insert(7, "-");
            }

            // Update the Entry text and cursor position
            entry.Text = cleanedText;
            entry.CursorPosition = cleanedText.Length;
        }
  
        internal async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.BindingContext is ItemCount selectedItemCount)
            {
                bool answer = await DisplayAlert("Delete", $"Are you sure you want to delete {selectedItemCount.ItemDescription}?", "Yes", "No");
                if (answer)
                {
                    try
                    {
                        await _itemCountViewModel.DeleteItemCount(selectedItemCount.ItemKey);
                        await DisplayAlert("Success", $"Deleted {selectedItemCount.ItemDescription}", "OK");
                        LoadItemCountData();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Failed to delete: {ex.Message}", "OK");
                    }
                }
            }
       
        }

        private async void AddItem()
        {
            var itemCodeEntry = new Entry
            {
                Text = ItemNumber,
                IsReadOnly = true,
                HorizontalOptions = LayoutOptions.Fill
            };
            var itemDescriptionEntry = new Entry
            {
                Text = ItemDescription,
                IsReadOnly = true,
                HorizontalOptions = LayoutOptions.Fill
            };
            var itemUomEntry = new Entry
            {
                Text = SellingUom,
                IsReadOnly = true,
                HorizontalOptions = LayoutOptions.Fill
            };
            var itemBatchLotNumberEntry = new Entry { Placeholder = "ItemBatchLotNumber", HorizontalOptions = LayoutOptions.Fill };
            var itemExpiryEntry = new Entry
            {
                Placeholder = "YYYY-MM-DD",
                HorizontalOptions = LayoutOptions.Fill,
                Keyboard = Keyboard.Numeric
            };
            itemExpiryEntry.TextChanged += ExpiryEntry_TextChanged;
            var itemQuantityEntry = new Entry { Placeholder = "ItemQuantity", HorizontalOptions = LayoutOptions.Fill, Keyboard = Keyboard.Numeric };

            var saveButton = new Button
            {
                Text = "Save",
                BackgroundColor = Color.FromRgb(173, 216, 230),
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
                BackgroundColor = new Color(0, 0, 0, 0.1f), // Semi-transparent black
                Content = new Frame
                {
                    BackgroundColor = Colors.White,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Padding = new Thickness(20),
                    WidthRequest = 300, // Set a fixed width for the frame
                    Content = new StackLayout
                    {
                        Spacing = 10,
                        Children =
                        {
                            new Label { Text = "Add New Item", FontAttributes = FontAttributes.Bold },
                            new Label { Text = "ItemCode" },
                            itemCodeEntry,
                            new Label { Text = "ItemDescription" },
                            itemDescriptionEntry,
                            new Label { Text = "ItemUom" },
                            itemUomEntry,
                            new Label { Text = "ItemQuantity" },
                            itemQuantityEntry,
                            new Label { Text = "ItemBatchLotNumber" },
                            itemBatchLotNumberEntry,
                            new Label { Text = "Expiry (YYYY-MM-DD)" },
                            itemExpiryEntry,
                            buttonStack
                        }
                    }
                }
            };

            var tcs = new TaskCompletionSource<bool>();

            saveButton.Clicked += async (s, args) =>
            {
                string itemCode = ItemNumber;
                string itemDescription = ItemDescription;
                string itemUom = SellingUom;
                string itemBatchLotNumber = itemBatchLotNumberEntry.Text;
                string itemExpiry = itemExpiryEntry.Text;
                string itemQuantityString = itemQuantityEntry.Text;

                if (!string.IsNullOrEmpty(itemCode) && !string.IsNullOrEmpty(itemDescription) && !string.IsNullOrEmpty(itemUom) && !string.IsNullOrEmpty(itemBatchLotNumber) && !string.IsNullOrEmpty(itemExpiry) && int.TryParse(itemQuantityString, out int itemQuantity))
                {
                    if (itemQuantity < 0)
                    {
                        await DisplayAlert("Error", "Quantity cannot be negative.", "OK");
                        return;
                    }

                    bool success = await _itemCountViewModel.AddItemCount(_countCode, itemCode, itemDescription, itemUom, itemBatchLotNumber, itemExpiry, itemQuantity);
                    if (success)
                    {
                        await DisplayAlert("Success", "Item added successfully", "OK");
                        tcs.SetResult(true);
                        LoadItemCountData();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to add item", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Please fill all fields correctly.", "OK");
                }
            };

            await Navigation.PushModalAsync(page);
            bool result = await tcs.Task;
            await Navigation.PopModalAsync();
        }

    }
}

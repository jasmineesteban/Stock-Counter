using MauiApp1.Pages;

namespace MauiApp1.Helpers
{
    public static class GridColumnVisibilityHelper
    {
        public static void UpdateColumnVisibility(Grid headerGrid, CollectionView dataGrid, bool showCtr, bool showItemNo, bool showDescription, bool showUom, bool showBatchLot, bool showExpiry, bool showQuantity, CountSheetsPage page)
        {
            var columnsToShow = new List<(string Name, bool IsVisible)>
            {
                ("Counter", showCtr),
                ("Item No.", showItemNo),
                ("Description", showDescription),
                ("UOM", showUom),
                ("Quantity", showQuantity),
                ("Batch&Lot", showBatchLot),
                ("Expiry", showExpiry)
            };

            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateHeaderGrid(headerGrid, columnsToShow);
                UpdateDataGrid(dataGrid, columnsToShow, page);
            });
        }

        private static void UpdateHeaderGrid(Grid headerGrid, List<(string Name, bool IsVisible)> columnsToShow)
        {
            headerGrid.Children.Clear();
            headerGrid.ColumnDefinitions.Clear();
            int columnIndex = 0;

            foreach (var (name, isVisible) in columnsToShow)
            {
                if (isVisible)
                {
                    AddColumnDefinitionAndLabel(headerGrid, isVisible, name, columnIndex);
                    columnIndex++;
                }
            }
        }

        private static void UpdateDataGrid(CollectionView dataGrid, List<(string Name, bool IsVisible)> columnsToShow, CountSheetsPage page)
        {
            dataGrid.ItemTemplate = new DataTemplate(() => CreateItemGrid(columnsToShow, page));
        }

        private static int AddColumnDefinitionAndLabel(Grid headerGrid, bool isVisible, string text, int columnIndex)
        {
            if (isVisible)
            {
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GetHeaderColumnWidth(text) });
                var label = new Label
                {
                    Text = text,
                    TextColor = Colors.White,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontAttributes = FontAttributes.Italic
                };
                headerGrid.Children.Add(label);
                Grid.SetColumn(label, columnIndex);
                columnIndex++;
            }
            return columnIndex;
        }

        private static SwipeView CreateItemGrid(List<(string Name, bool IsVisible)> columnsToShow, CountSheetsPage page)
        {
            var itemGrid = new Grid { ColumnSpacing = 0 };
            int columnIndex = 0;

            foreach (var (name, isVisible) in columnsToShow)
            {
                if (isVisible)
                {
                    AddItemColumn(itemGrid, isVisible, GetBindingPropertyName(name), name, columnIndex);
                    columnIndex++;
                }
            }

            var swipeView = new SwipeView { Content = itemGrid };

            var leftSwipeItems = new SwipeItems { new SwipeItem { Text = "Edit", BackgroundColor = Colors.LightBlue } };
            leftSwipeItems[0].Invoked += (s, e) => page.OnEditClicked(s, e);
            swipeView.LeftItems = leftSwipeItems;

            var rightSwipeItems = new SwipeItems { new SwipeItem { Text = "Delete", BackgroundColor = Colors.LightPink } };
            rightSwipeItems[0].Invoked += (s, e) => page.OnDeleteClicked(s, e);
            swipeView.RightItems = rightSwipeItems;

            return swipeView;
        }

        private static int AddItemColumn(Grid itemGrid, bool isVisible, string bindingPath, string name, int columnIndex)
        {
            if (isVisible)
            {
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GetDataColumnWidth(name) });
                var border = new Border
                {
                    Stroke = Colors.Gray,
                    StrokeThickness = 0.1,
                    Padding = 5
                };
                var label = new Label
                {
                    VerticalOptions = GetVerticalOptions(name),
                    HorizontalOptions = GetHorizontalOptions(name),
                    FontAttributes = GetFontAttributes(name)
                };
                label.SetBinding(Label.TextProperty, bindingPath);
                border.Content = label;
                itemGrid.Children.Add(border);
                Grid.SetColumn(border, columnIndex);
                columnIndex++;
            }
            return columnIndex;
        }

        private static string GetBindingPropertyName(string columnName)
        {
            return columnName switch
            {
                "Counter" => "ItemCounter",
                "Item No." => "ItemCode",
                "Description" => "ItemDescription",
                "UOM" => "ItemUom",
                "Quantity" => "ItemQuantity",
                "Batch&Lot" => "ItemBatchLotNumber",
                "Expiry" => "ItemExpiry",
                _ => string.Empty
            };
        }

        private static LayoutOptions GetHorizontalOptions(string columnName)
        {
            return columnName switch
            {
                "Counter" => LayoutOptions.Start,
                "Item No." => LayoutOptions.Start,
                "Description" => LayoutOptions.Start,
                "UOM" => LayoutOptions.Center,
                "Quantity" => LayoutOptions.End,
                "Batch&Lot" => LayoutOptions.Start,
                "Expiry" => LayoutOptions.Start,
                _ => LayoutOptions.Center
            };
        }

        private static LayoutOptions GetVerticalOptions(string columnName)
        {
            return columnName switch
            {
                "Counter" => LayoutOptions.Center,
                "Item No." => LayoutOptions.Center,
                "Description" => LayoutOptions.Center,
                "UOM" => LayoutOptions.Center,
                "Quantity" => LayoutOptions.Center,
                "Batch&Lot" => LayoutOptions.Center,
                "Expiry" => LayoutOptions.Center,
                _ => LayoutOptions.Center
            };
        }

        private static FontAttributes GetFontAttributes(string columnName)
        {
            return columnName switch
            {
                "Counter" => FontAttributes.None,
                "Item No." => FontAttributes.None,
                "Description" => FontAttributes.None,
                "UOM" => FontAttributes.None,
                "Quantity" => FontAttributes.Bold,
                "Batch&Lot" => FontAttributes.None,
                "Expiry" => FontAttributes.None,
                _ => FontAttributes.None
            };
        }

        private static GridLength GetHeaderColumnWidth(string columnName)
        {
            return columnName switch
            {
                "Counter" => new GridLength(50),
                "Item No." => new GridLength(60),
                "Description" => new GridLength(250),
                "UOM" => new GridLength(80),
                "Quantity" => new GridLength(75),
                "Batch&Lot" => new GridLength(100),
                "Expiry" => new GridLength(100),
                _ => new GridLength(100)
            };
        }

        private static GridLength GetDataColumnWidth(string columnName)
        {
            return columnName switch
            {
                "Counter" => new GridLength(50),
                "Item No." => new GridLength(60),
                "Description" => new GridLength(250),
                "UOM" => new GridLength(80),
                "Quantity" => new GridLength(75),
                "Batch&Lot" => new GridLength(100),
                "Expiry" => new GridLength(100),
                _ => new GridLength(100)
            };
        }
    }
}

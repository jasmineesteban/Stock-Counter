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
                ("Batch&Lot", showBatchLot),
                ("Expiry", showExpiry),
                ("Quantity", showQuantity)
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
                    AddColumnDefinitionAndLabel(headerGrid, true, name, columnIndex);
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
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 200 });
                var label = new Label
                {
                    Text = text,
                    TextColor = Colors.White,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                headerGrid.Children.Add(label);
                Grid.SetColumn(label, columnIndex);
                columnIndex++;
            }
            return columnIndex;
        }

        private static SwipeView CreateItemGrid(List<(string Name, bool IsVisible)> columnsToShow, CountSheetsPage page)
        {
            var itemGrid = new Grid { ColumnSpacing = 1 };
            int columnIndex = 0;

            foreach (var (name, isVisible) in columnsToShow)
            {
                if (isVisible)
                {
                    AddItemColumn(itemGrid, true, GetBindingPropertyName(name), columnIndex);
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

        private static int AddItemColumn(Grid itemGrid, bool isVisible, string bindingPath, int columnIndex)
        {
            if (isVisible)
            {
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 200 });
                var border = new Border
                {
                    Stroke = Colors.Gray,
                    StrokeThickness = 0.1,
                    Padding = 5
                };
                var label = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
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
            switch (columnName)
            {
                case "Counter": return "ItemCounter";
                case "Item No.": return "ItemCode";
                case "Description": return "ItemDescription";
                case "UOM": return "ItemUom";
                case "Batch&Lot": return "ItemBatchLotNumber";
                case "Expiry": return "ItemExpiry";
                case "Quantity": return "ItemQuantity";
                default: return string.Empty;
            }
        }
    }
}
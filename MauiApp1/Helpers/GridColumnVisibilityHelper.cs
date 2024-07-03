using MauiApp1.Pages;

namespace MauiApp1.Helpers
{
    public static class GridColumnVisibilityHelper
    {
        public static void UpdateColumnVisibility(Grid headerGrid, CollectionView dataGrid, bool showCtr, bool showItemNo, bool showDescription, bool showUom, bool showBatchLot, bool showExpiry, bool showQuantity, CountSheetsPage page)
        {
            headerGrid.Children.Clear();
            headerGrid.ColumnDefinitions.Clear();
            int columnIndex = 0;

            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showCtr, "Counter", columnIndex);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showItemNo, "Item No.", columnIndex);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showDescription, "Description", columnIndex);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showUom, "UOM", columnIndex);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showBatchLot, "Batch&Lot", columnIndex);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showExpiry, "Expiry", columnIndex);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showQuantity, "Quantity", columnIndex);

            dataGrid.ItemTemplate = new DataTemplate(() => CreateItemGrid(showCtr, showItemNo, showDescription, showUom, showBatchLot, showExpiry, showQuantity, page));
        }

        private static int AddColumnDefinitionAndLabel(Grid headerGrid, bool isVisible, string text, int columnIndex)
        {
            if (isVisible)
            {
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
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

        private static SwipeView CreateItemGrid(bool showCtr, bool showItemNo, bool showDescription, bool showUom, bool showBatchLot, bool showExpiry, bool showQuantity, CountSheetsPage page)
        {
            var itemGrid = new Grid { ColumnSpacing = 1 };
            int columnIndex = 0;

            columnIndex = AddItemColumn(itemGrid, showCtr, "ItemCounter", columnIndex);
            columnIndex = AddItemColumn(itemGrid, showItemNo, "ItemCode", columnIndex);
            columnIndex = AddItemColumn(itemGrid, showDescription, "ItemDescription", columnIndex);
            columnIndex = AddItemColumn(itemGrid, showUom, "ItemUom", columnIndex);
            columnIndex = AddItemColumn(itemGrid, showBatchLot, "ItemBatchLotNumber", columnIndex);
            columnIndex = AddItemColumn(itemGrid, showExpiry, "ItemExpiry", columnIndex);
            columnIndex = AddItemColumn(itemGrid, showQuantity, "ItemQuantity", columnIndex);


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
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
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
    }
}

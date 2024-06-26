﻿namespace MauiApp1.Helpers
{
    public static class GridColumnVisibilityHelper
    {
        public static void UpdateColumnVisibility(Grid headerGrid, CollectionView dataGrid, bool showCtr, bool showItemNo, bool showDescription, bool showUom, bool showBatchLot, bool showExpiry, bool showQuantity)
        {
            headerGrid.Children.Clear();
            headerGrid.ColumnDefinitions.Clear();
            int columnIndex = 0;

            AddColumnDefinitionAndLabel(headerGrid, showCtr, "Counter", columnIndex++);
            AddColumnDefinitionAndLabel(headerGrid, showItemNo, "Item No.", columnIndex++);
            AddColumnDefinitionAndLabel(headerGrid, showDescription, "Description", columnIndex++);
            AddColumnDefinitionAndLabel(headerGrid, showUom, "UOM", columnIndex++);
            AddColumnDefinitionAndLabel(headerGrid, showBatchLot, "Batch&Lot", columnIndex++);
            AddColumnDefinitionAndLabel(headerGrid, showExpiry, "Expiry", columnIndex++);
            AddColumnDefinitionAndLabel(headerGrid, showQuantity, "Quantity", columnIndex);

            dataGrid.ItemTemplate = new DataTemplate(() => CreateItemGrid(showCtr, showItemNo, showDescription, showUom, showBatchLot, showExpiry, showQuantity));
        }

        private static void AddColumnDefinitionAndLabel(Grid headerGrid, bool isVisible, string text, int column)
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
                Grid.SetColumn(label, column);
            }
        }

        private static Grid CreateItemGrid(bool showCtr, bool showItemNo, bool showDescription, bool showUom, bool showBatchLot, bool showExpiry, bool showQuantity)
        {
            var itemGrid = new Grid { ColumnSpacing = 5 };
            int columnIndex = 0;

            AddItemColumn(itemGrid, showCtr, "ItemCounter", columnIndex++);
            AddItemColumn(itemGrid, showItemNo, "ItemCode", columnIndex++);
            AddItemColumn(itemGrid, showDescription, "ItemDescription", columnIndex++);
            AddItemColumn(itemGrid, showUom, "ItemUom", columnIndex++);
            AddItemColumn(itemGrid, showBatchLot, "ItemBatchLotNumber", columnIndex++);
            AddItemColumn(itemGrid, showExpiry, "ItemExpiry", columnIndex++);
            AddItemColumn(itemGrid, showQuantity, "ItemQuantity", columnIndex);

            return itemGrid;
        }

        private static void AddItemColumn(Grid itemGrid, bool isVisible, string bindingPath, int column)
        {
            if (isVisible)
            {
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                var border = new Border
                {
                    Stroke = Colors.Gray,
                    StrokeThickness = 0.3,
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
                Grid.SetColumn(border, column);
            }
        }
    }
}

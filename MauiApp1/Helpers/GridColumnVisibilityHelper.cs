﻿namespace MauiApp1.Helpers
{
    public static class GridColumnVisibilityHelper
    {
        public static void UpdateColumnVisibility(Grid headerGrid, CollectionView dataGrid, bool showName, bool showQuantity, bool showUOM)
        {
            // Update header visibility
            headerGrid.Children.Clear();
            headerGrid.ColumnDefinitions.Clear();
            AddColumnDefinitionAndLabel(headerGrid, showName, "Name", 0);
            AddColumnDefinitionAndLabel(headerGrid, showQuantity, "Quantity", showName ? 1 : 0);
            AddColumnDefinitionAndLabel(headerGrid, showUOM, "UOM", showName && showQuantity ? 2 : (showName || showQuantity) ? 1 : 0);

            // Update data item visibility
            dataGrid.ItemTemplate = new DataTemplate(() => CreateItemGrid(showName, showQuantity, showUOM));
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

        private static Grid CreateItemGrid(bool showName, bool showQuantity, bool showUOM)
        {
            var itemGrid = new Grid { ColumnSpacing = 5 };

            AddItemColumn(itemGrid, showName, "Name", 0);
            AddItemColumn(itemGrid, showQuantity, "Quantity", showName ? 1 : 0);
            AddItemColumn(itemGrid, showUOM, "UOM", showName && showQuantity ? 2 : (showName || showQuantity) ? 1 : 0);

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
﻿using MauiApp1.Pages;

namespace MauiApp1.Helpers
{
    public static class GridColumnVisibilityHelper
    {
        public static void UpdateColumnVisibility(Grid headerGrid, CollectionView dataGrid, bool showCtr, bool showItemNo, bool showDescription, bool showUom, bool showBatchLot, bool showExpiry, bool showQuantity, CountSheetsPage page)
        {
            headerGrid.Children.Clear();
            headerGrid.ColumnDefinitions.Clear();
            int columnIndex = 0;

            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showCtr, "Counter", columnIndex, 30);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showItemNo, "Item No.", columnIndex, 50);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showDescription, "Description", columnIndex, 200);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showUom, "UOM", columnIndex, 80);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showQuantity, "Quantity", columnIndex, 80);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showBatchLot, "Batch&Lot", columnIndex, 120);
            columnIndex = AddColumnDefinitionAndLabel(headerGrid, showExpiry, "Expiry", columnIndex, 120);

            dataGrid.ItemTemplate = new DataTemplate(() => CreateItemGrid(showCtr, showItemNo, showDescription, showUom, showBatchLot, showExpiry, showQuantity, page));
        }

        private static int AddColumnDefinitionAndLabel(Grid headerGrid, bool isVisible, string text, int columnIndex, double width)
        {
            if (isVisible)
            {
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width) });
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
            var itemGrid = new Grid { ColumnSpacing = 0 };
            int columnIndex = 0;

            columnIndex = AddItemColumn(itemGrid, showCtr, "ItemCounter", columnIndex, 30, LayoutOptions.Center, false);
            columnIndex = AddItemColumn(itemGrid, showItemNo, "ItemCode", columnIndex, 50, LayoutOptions.Start, false);
            columnIndex = AddItemColumn(itemGrid, showDescription, "ItemDescription", columnIndex, 200, LayoutOptions.Start, false);
            columnIndex = AddItemColumn(itemGrid, showUom, "ItemUom", columnIndex, 80, LayoutOptions.Center, false);
            columnIndex = AddItemColumn(itemGrid, showQuantity, "ItemQuantity", columnIndex, 80, LayoutOptions.End, true);
            columnIndex = AddItemColumn(itemGrid, showBatchLot, "ItemBatchLotNumber", columnIndex, 120, LayoutOptions.Start, false);
            columnIndex = AddItemColumn(itemGrid, showExpiry, "ItemExpiry", columnIndex, 120, LayoutOptions.Start, false);

            var swipeView = new SwipeView { Content = itemGrid };

            var leftSwipeItems = new SwipeItems { new SwipeItem { Text = "Edit", BackgroundColor = Colors.LightBlue } };
            leftSwipeItems[0].Invoked += (s, e) => page.OnEditClicked(s, e);
            swipeView.LeftItems = leftSwipeItems;

            var rightSwipeItems = new SwipeItems { new SwipeItem { Text = "Delete", BackgroundColor = Colors.LightPink } };
            rightSwipeItems[0].Invoked += (s, e) => page.OnDeleteClicked(s, e);
            swipeView.RightItems = rightSwipeItems;

            return swipeView;
        }

        private static int AddItemColumn(Grid itemGrid, bool isVisible, string bindingPath, int columnIndex, double width, LayoutOptions horizontalOptions, bool isBold)
        {
            if (isVisible)
            {
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width) });
                var border = new Border
                {
                    Stroke = Colors.Gray,
                    StrokeThickness = 0.1,
                    Padding = 5
                };
                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = horizontalOptions,
                    FontAttributes = isBold ? FontAttributes.Bold : FontAttributes.None

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

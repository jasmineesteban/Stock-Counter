using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiApp1.Models;

namespace MauiApp1.Helpers
{
    public static class SortingHelper
    {
        private static int _tapCount = 0;
        private static int _sort = 0;

        public static async Task HandleHeaderGridTap(Grid headerGrid, Func<int, Task<IEnumerable<ItemCount>>> loadItems)
        {
            _tapCount++;

            switch (_tapCount)
            {
                case 1:
                    _sort = 1;
                    UpdateSortIndicator(headerGrid, "▼");
                    var toast1 = Toast.Make("Sorted 0 - Z", ToastDuration.Short);
                    await toast1.Show();
                    break;

                case 2:
                    _sort = 2;
                    UpdateSortIndicator(headerGrid, "▲");
                    var toast2 = Toast.Make("Sorted Z - 0", ToastDuration.Short);
                    await toast2.Show();
                    break;

                default:
                    _tapCount = 0;
                    _sort = 0;
                    UpdateSortIndicator(headerGrid, "");
                    break;
            }

            await loadItems(_sort);
        }

        private static void UpdateSortIndicator(Grid headerGrid, string text)
        {
            var sortIndicator = headerGrid.FindByName<Label>("sortIndicator");
            if (sortIndicator != null)
            {
                sortIndicator.Text = text;
            }
        }
    }
}

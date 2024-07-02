using Microsoft.Maui.Controls;

namespace MauiApp1.Controls
{
    public class SelectAllEntry : Entry
    {
        public SelectAllEntry()
        {
            this.Focused += OnEntryFocused;
        }

        private void OnEntryFocused(object sender, FocusEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                this.CursorPosition = 0;
                this.SelectionLength = this.Text?.Length ?? 0;
            });
        }
    }
}
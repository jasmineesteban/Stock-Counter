using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp1.Extensions
{
    public static class EntryExtensions
    {
        public static Task<string> GetInputAsync(this Entry entry, string title, string saveText, string cancelText)
        {
            var tcs = new TaskCompletionSource<string>();
            var saveButton = new Button
            {
                Text = saveText,
                WidthRequest = 100,
                HeightRequest = 50,
                BackgroundColor = Color.FromRgb(173, 216, 230), // Light blue
                TextColor = Colors.White
            };
            var cancelButton = new Button
            {
                Text = cancelText,
                WidthRequest = 100,
                HeightRequest = 50,
                BackgroundColor = Color.FromRgb(255, 127, 127), // Light red
                TextColor = Colors.White
            };
            saveButton.Clicked += (s, e) => tcs.TrySetResult(entry.Text);
            cancelButton.Clicked += (s, e) => tcs.TrySetResult(null);
            var buttonStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,
                Children = { saveButton, cancelButton }
            };
            var container = (StackLayout)entry.Parent;
            container.Children.Add(buttonStack);
            MainThread.BeginInvokeOnMainThread(() => entry.Focus());
            return tcs.Task;
        }
    }
}
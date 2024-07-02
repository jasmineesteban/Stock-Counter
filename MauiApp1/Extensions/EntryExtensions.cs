using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp1.Extensions
{
    public static class EntryExtensions
    {
        public static Task<string> GetInputAsync(this Entry entry, string title, string okText, string cancelText)
        {
            var tcs = new TaskCompletionSource<string>();

            var okButton = new Button
            {
                Text = okText,
                WidthRequest = 100,
                HeightRequest = 50,
                BackgroundColor = Colors.Green,
                TextColor = Colors.White
            };
            var cancelButton = new Button
            {
                Text = cancelText,
                WidthRequest = 100,
                HeightRequest = 50,
                BackgroundColor = Colors.Red,
                TextColor = Colors.White
            };

            okButton.Clicked += (s, e) => tcs.TrySetResult(entry.Text);
            cancelButton.Clicked += (s, e) => tcs.TrySetResult(null);

            var buttonStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,
                Children = { okButton, cancelButton }
            };

            var container = (StackLayout)entry.Parent;
            container.Children.Add(buttonStack);

            MainThread.BeginInvokeOnMainThread(() => entry.Focus());

            return tcs.Task;
        }
    }
}
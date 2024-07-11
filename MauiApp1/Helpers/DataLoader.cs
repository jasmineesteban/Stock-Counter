using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

public static class DataLoader
{
    public static async Task LoadDataAsync<T>(ObservableCollection<T> collection, Func<Task<IEnumerable<T>>> loadDataFunc, ActivityIndicator loadingIndicator = null)
    {
        try
        {
            if (loadingIndicator != null)
            {
                loadingIndicator.IsRunning = true;
                loadingIndicator.IsVisible = true;
            }

            var items = await loadDataFunc();
            collection.Clear();
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load items: {ex.Message}", "OK");
        }
        finally
        {
            if (loadingIndicator != null)
            {
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
            }
        }
    }
}
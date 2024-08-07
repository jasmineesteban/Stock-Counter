using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiApp1.ViewModels;

namespace MauiApp1.Pages
{
    [QueryProperty(nameof(EmployeeDetails), "EmployeeDetails")]
    public partial class ModalPage : ContentPage
    {
        private readonly CountSheetViewModel _countSheetViewModel;
        private string employeeId;
        private string employeeName;

        public string EmployeeDetails
        {
            set
            {
                var details = value.Split(new[] { " - " }, StringSplitOptions.None);
                employeeId = details[0];
                employeeName = details.Length > 1 ? details[1] : string.Empty;
                EmployeeEntry.Text = employeeName;
            }
        }

        public ModalPage(CountSheetViewModel countSheetViewModel)
        {
            InitializeComponent();
            _countSheetViewModel = countSheetViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await FadeInModalFrame();
            Device.BeginInvokeOnMainThread(() =>
            {
                CountSheetEntry.Focus();
            });

            CountSheetEntry.Completed += (s, e) => { CountSheetEntry.Unfocus(); };

        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                var description = CountSheetEntry.Text;
                var date = DateEntry.Date;

                if (string.IsNullOrEmpty(description))
                {
                    await DisplayAlert("Oops", "The description cannot be empty.", "OK");
                    return;
                }

                await _countSheetViewModel.AddCountSheet(employeeId, description, date);

                var toast = Toast.Make("Count sheet added !", ToastDuration.Short);
                await toast.Show();
                CountSheetEntry.Unfocus();

                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }

            await FadeOutModalFrame();
        }

        private async Task FadeInModalFrame()
        {
            this.Opacity = 0;
            await this.FadeTo(1, 500, Easing.CubicInOut);
        }

        private async Task FadeOutModalFrame()
        {
            await this.FadeTo(0, 500, Easing.CubicInOut);
        }

    }
}

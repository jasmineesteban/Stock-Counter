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
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            var description = CountSheetEntry.Text;
            var date = DateEntry.Date;

            await _countSheetViewModel.AddCountSheet(employeeId, description, date);

            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task FadeInModalFrame()
        {
            this.Opacity = 0;
            await this.FadeTo(1, 250, Easing.Linear);
        }
    }
}

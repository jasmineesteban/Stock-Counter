using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp1.Pages
{
    public partial class OnboardingPage : ContentPage
    {
        public OnboardingPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(3000); //3 seconds
            await Shell.Current.GoToAsync(nameof(SignInPage));
        }
    }
}

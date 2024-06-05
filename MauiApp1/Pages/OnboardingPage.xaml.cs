namespace MauiApp1.Pages;

public partial class OnboardingPage : ContentPage
{
	public OnboardingPage()
	{
		InitializeComponent();
	}

  //  private async void Button_Clicked(object sender, EventArgs e)
  //  {
		//await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
  //  }

    private async void SignIn_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SignInPage));
    }
}
namespace MauiApp1;

public class Main : ContentPage
{
	public Main()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome!"
				}
			}
		};
	}
}
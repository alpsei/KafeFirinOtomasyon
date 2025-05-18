namespace KafeFirinMaui.Views;

public partial class ForgotYourPassword : ContentPage
{
	public ForgotYourPassword()
	{
		InitializeComponent();
	}
	private void OnChangePasswordClicked(object sender, EventArgs e)
	{ }
    private void showPasswordCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            passwordEntry.IsPassword = false;
        }
        else
        {
            passwordEntry.IsPassword = true;
        }
    }
}

using KafeFirinMaui.Services;

namespace KafeFirinMaui.Views;

public partial class ForgotYourPassword : ContentPage
{
    private readonly UserService _userService;

    public ForgotYourPassword(UserService userService)
    {
        InitializeComponent();
        _userService = userService;

    }

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text.Trim();
        string newPassword = passwordEntry.Text;
        string selectedQuestion = secQuestionPicker.SelectedItem?.ToString();
        string secAnswer = secAnswerEntry.Text.Trim();

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(newPassword) ||
            string.IsNullOrWhiteSpace(selectedQuestion) ||
            string.IsNullOrWhiteSpace(secAnswer))
        {
            await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurun.", "Tamam");
            return;
        }

        var user = await _userService.GetUserByUsernameAndSecurity(username, selectedQuestion, secAnswer);

        if (user == null)
        {
            await DisplayAlert("Hata", "Kullan�c� ad�, g�venlik sorusu veya cevap hatal�.", "Tamam");
            return;
        }
        bool updated = await _userService.UpdatePasswordAsync(username, selectedQuestion, secAnswer, newPassword);
        if (updated)
        {
            await DisplayAlert("Ba�ar�l�", "�ifreniz ba�ar�yla de�i�tirildi.", "Tamam");

            usernameEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;
            secQuestionPicker.SelectedIndex = -1;
            secAnswerEntry.Text = string.Empty;

            await Shell.Current.GoToAsync("///UserLogin");
        }
        else
        {
            await DisplayAlert("Hata", "�ifre g�ncellenemedi.", "Tamam");
        }
    }


    private void showPasswordCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        passwordEntry.IsPassword = !e.Value;
    }
}

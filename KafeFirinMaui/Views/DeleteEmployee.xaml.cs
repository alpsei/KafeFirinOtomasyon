using KafeFirinMaui.Services.Interfaces;

namespace KafeFirinMaui.Views;

public partial class DeleteEmployee : ContentPage
{
    private readonly IUserService _userService;
    public DeleteEmployee(IUserService userService)
	{
		InitializeComponent();
        _userService = userService;
    }
    private async void EmployeeDeleteOnClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        if (string.IsNullOrWhiteSpace(username))
        {
            await DisplayAlert("Hata", "Kullan�c� ad� bo� olamaz.", "Tamam");
            return;
        }
        bool result = await _userService.DeleteUsersAsync(username);
        if (result)
        {
            await DisplayAlert("Ba�ar�l�", "Kullan�c� ba�ar�yla silindi!", "Tamam");
        }
        else
        {
            await DisplayAlert("Hata", "Kullan�c� silinemedi.", "Tamam");
        }
    }
    private async void SearchEmployeeOnClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        var user = await _userService.GetUserByUsernameAsync(username);
        if (string.IsNullOrWhiteSpace(username))
        {
            await DisplayAlert("Hata", "Kullan�c� ad� bo� olamaz.", "Tamam");
            return;
        }
        else if(user.RoleID != 2)
        {
            await DisplayAlert("Hata", "L�tfen do�ru bir kullan�c� ad� giriniz.", "Tamam");
            return;
        }
        else if (user != null)
        {
            nameEntry.Text = user.FirstName;
            surnameEntry.Text = user.LastName;
            emailEntry.Text = user.Email;
            passwordEntry.Text = user.Password;
        }
        else
        {
            await DisplayAlert("Hata", "Kullan�c� bulunamad�.", "Tamam");
        }
    }
}
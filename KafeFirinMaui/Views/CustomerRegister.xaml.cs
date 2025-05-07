using KafeFirinMaui.Services;
using Microsoft.Extensions.Logging;
using SharedClasses;

namespace KafeFirinMaui.Views;

public partial class CustomerRegister : ContentPage
{
    private readonly UserService _userService;
    public CustomerRegister(UserService userService)
    {
        InitializeComponent();
        _userService = userService;
    }

    private async void RegisterOnClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string fullName = nameEntry.Text + " " + surnameEntry.Text;
        string password = passwordEntry.Text;
        string email = emailEntry.Text;
        string? secQuestion = secQuestionPicker.SelectedItem.ToString();
        string secAnswer = secAnswerEntry.Text;

        Users newUser = new Users
        {
            Username = username,
            FullName = fullName,
            Password = password,
            Email = email,
            SecQuestion = secQuestion,
            SecAnswer = secAnswer,
            RoleID = 2
        };

        bool result = await _userService.CreateUsersAsync(newUser);

        if (result)
        {
            await DisplayAlert("Baþarýlý", "Kullanýcý baþarýyla oluþturuldu!", "Tamam");
        }
        else
        {
            await DisplayAlert("Hata", "Kullanýcý oluþturulamadý.", "Tamam");
        }
    }
}
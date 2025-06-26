using KafeFirinMaui.Services.Interfaces;
using Microsoft.Extensions.Logging;
using SharedClass.Classes;

namespace KafeFirinMaui.Views;

public partial class CustomerRegister : ContentPage
{
    private readonly IUserService _userService;
    public CustomerRegister(IUserService userService)
    {
        InitializeComponent();
        _userService = userService;
    }

    private async void RegisterOnClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string firstName = nameEntry.Text;
        string lastName = surnameEntry.Text;
        string password = passwordEntry.Text;
        string email = emailEntry.Text;
        string? secQuestion = secQuestionPicker.SelectedItem.ToString();
        string secAnswer = secAnswerEntry.Text;

        Users newUser = new Users
        {
            Username = username,
            FirstName = firstName,
            LastName = lastName,
            Password = password,
            Email = email,
            SecQuestion = secQuestion,
            SecAnswer = secAnswer,
            RoleID = 1
        };

        bool result = await _userService.CreateUsersAsync(newUser);

        if (result)
        {
            await DisplayAlert("Baþarýlý", "Kullanýcý baþarýyla oluþturuldu!", "Tamam");
            await Shell.Current.GoToAsync("///UserLogin");
            usernameEntry.Text = string.Empty;
            nameEntry.Text = string.Empty;
            surnameEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;
            emailEntry.Text = string.Empty;
            secQuestionPicker.SelectedItem = null;
            secAnswerEntry.Text = string.Empty;
        }
        else
        {
            await DisplayAlert("Hata", "Kullanýcý oluþturulamadý.", "Tamam");
        }
    }
}
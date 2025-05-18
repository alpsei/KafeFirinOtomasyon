using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services;
using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KafeFirinMaui.ViewModels
{
    public class UserSettingsViewModel : INotifyPropertyChanged
    {
        private readonly UserService _userService;
        private Users _user;
        public ICommand UpdateCommand => new Command(async () => await UpdateUserInfoAsync());

        public Users User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public UserSettingsViewModel(UserService userService)
        {
            _userService = userService;
            _ = LoadUserInfoAsync();
        }
        public async Task LoadUserInfoAsync()
        {
            try
            {
                var allUsers = await _userService.GetUsersAsync();
                var currentUserId = Session.LoggedInUser.UserID;
                var user = allUsers.FirstOrDefault(x => x.UserID == currentUserId);
                if(user != null)
                {
                    User = user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kullanıcı bilgileri yüklenirken hata: {ex.Message}");
            }
        }
        public async Task<bool> UpdateUserInfoAsync()
        {
            try
            {
                var result = await _userService.UpdateUsersAsync(_user);
                if (result)
                {
                    await App.Current.MainPage.DisplayAlert("Başarılı", "Kullanıcı bilgileri güncellendi.", "Tamam");
                    return true;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Hata", "Kullanıcı bilgileri güncellenemedi.", "Tamam");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kullanıcı bilgileri güncellenirken hata: {ex.Message}");
                return false;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

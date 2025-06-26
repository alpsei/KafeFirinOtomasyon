using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services.Interfaces;
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
        private readonly IUserService _userService;
        private Users _user;
        //public ICommand UpdateCommand => new Command(async () => await UpdateUserInfoAsync());

        public Users User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public UserSettingsViewModel(IUserService userService)
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
                var currentUserId = Session.LoggedInUser.UserID;
                var result = await _userService.UpdateUsersAsync(User,currentUserId);
                if (result)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("API güncelleme işlemi başarısız döndü.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kullanıcı bilgileri güncellenirken hata: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var result = await _userService.DeleteUsersAsync(userId);
                if (result)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("API silme işlemi başarısız döndü.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kullanıcı bilgileri silinirken hata: {ex.Message}");
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services;
using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace KafeFirinMaui.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly UserService _userService;
        private Users _user;
        public ICommand UpdateCommand => new Command(async () => await UpdateEmployeeInfoAsync());
        public ObservableCollection<Users> Employees { get; set; } = new();
        public EmployeeViewModel(UserService userService)
        {
            _userService = userService;
            _ = LoadEmployeeAsync();
        }
        public async Task LoadEmployeeAsync()
        {
            try
            {
                var allUsers = await _userService.GetUsersAsync();
                var employees = allUsers.Where(x => x.RoleID == 2).ToList();

                Employees.Clear();
                foreach (var employee in employees)
                {
                    Employees.Add(employee);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Personel yüklenirken hata: {ex.Message}");
            }
        }
        public async Task<bool> UpdateEmployeeInfoAsync()
        {
            try
            {
                var users = await _userService.GetUserByIdAsync(Session.LoggedInUser.UserID);
                var result = await _userService.UpdateUsersAsync(users);
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
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}

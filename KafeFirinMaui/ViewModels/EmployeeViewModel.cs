using KafeFirinMaui.Services;
using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly UserService _userService;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}

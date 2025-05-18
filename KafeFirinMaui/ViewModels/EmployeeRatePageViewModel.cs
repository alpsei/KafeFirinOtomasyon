using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.ViewModels
{
    public class EmployeeRatePageViewModel : INotifyPropertyChanged
    {
        public EmployeeViewModel EmployeeVM { get; }
        public RateViewModel RateVM { get; }
        private Users _selectedEmployee;
        public Users SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                if (_selectedEmployee != value)
                {
                    _selectedEmployee = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _employeeAvgRate;
        private double EmployeeAvgRate
        {
            get => _employeeAvgRate;
            set
            {
                if (_employeeAvgRate != value)
                {
                    _employeeAvgRate = value;
                    OnPropertyChanged();
                }
            }
        }

        public EmployeeRatePageViewModel(EmployeeViewModel employeeVM, RateViewModel rateVM)
        {
            EmployeeVM = employeeVM;
            RateVM = rateVM;
        }

        private void UpdateSelectedEmployeeAverageRate()
        {
            if (SelectedEmployee == null)
            {
                EmployeeAvgRate = 0;
                return;
            }

            var ratesForEmployee = RateVM.Rates.Where(r => r.StaffID == SelectedEmployee.UserID);
            if (ratesForEmployee.Any())
            {
                EmployeeAvgRate = ratesForEmployee.Average(r => r.Rate);
            }
            else
            {
                EmployeeAvgRate = 0;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}

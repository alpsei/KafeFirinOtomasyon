using KafeFirinMaui.Helpers;
using KafeFirinMaui.Services;
using SharedClass.Classes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.Gaming.Preview.GamesEnumeration;

public class RateViewModel : INotifyPropertyChanged
{
    private readonly RateService _rateService;

    private Rates _employeeRate;
    public Rates EmployeeRate
    {
        get => _employeeRate;
        set
        {
            _employeeRate = value;
            OnPropertyChanged();
        }
    }
    private List<Rates> _rates = new List<Rates>();
    public List<Rates> Rates
    {
        get => _rates;
        set
        {
            _rates = value;
            OnPropertyChanged();
        }
    }

    private int _currentRating;
    public int CurrentRating
    {
        get => _currentRating;
        set
        {
            if (_currentRating != value)
            {
                _currentRating = value;
                OnPropertyChanged();
                (SendRateCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    private int _selectedEmployeeId;
    public int SelectedEmployeeId
    {
        get => _selectedEmployeeId;
        set
        {
            if (_selectedEmployeeId != value)
            {
                _selectedEmployeeId = value;
                OnPropertyChanged();
                (SendRateCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    private int _currentCustomerId;
    public int CurrentCustomerId
    {
        get => _currentCustomerId;
        set
        {
            if (_currentCustomerId != value)
            {
                _currentCustomerId = value;
                OnPropertyChanged();
                (SendRateCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    public ICommand SendRateCommand { get; }

    public RateViewModel(RateService rateService)
    {
        _rateService = rateService;
        SendRateCommand = new Command(async () => await SendRateAsync(), CanSendRate);
        CurrentCustomerId = Session.LoggedInUser?.UserID ?? 0;
    }

    private bool CanSendRate()
    {
        return SelectedEmployeeId > 0 && CurrentCustomerId > 0 && CurrentRating > 0;
    }

    public async Task SendRateAsync()
    {
        var rateData = new Rates
        {
            StaffID = SelectedEmployeeId,
            CustomerID = CurrentCustomerId,
            Rate = CurrentRating
        };
        Console.WriteLine($"Gönderiliyor: StaffId={SelectedEmployeeId}, CustomerId={CurrentCustomerId}, Rate={CurrentRating}");
        bool success = await _rateService.SaveRateAsync(rateData);
        if (success)
        {
            Console.WriteLine("Puan başarıyla kaydedildi.");
            await LoadRatesAsync(SelectedEmployeeId);
        }
        else
        {
            Console.WriteLine("Puan kaydedilirken hata oluştu.");
        }
    }

    public async Task LoadRatesAsync(int employeeId)
    {
        var rateList = await _rateService.GetRatesByEmployeeIDAsync(employeeId);
        Rates = rateList ?? new List<Rates>();

        if (rateList != null && rateList.Any())
        {
            var specificRate = rateList.FirstOrDefault(r => r.CustomerID == CurrentCustomerId);
            if (specificRate != null) 
            {
                CurrentRating = specificRate.Rate;
                EmployeeRate = specificRate;
            }
            else
            {
                CurrentRating = 0;
                EmployeeRate = new Rates { StaffID = employeeId, CustomerID = CurrentCustomerId };
            }
            SelectedEmployeeId = employeeId;
        }
        OnPropertyChanged(nameof(CurrentRating));
        OnPropertyChanged(nameof(EmployeeRate));
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

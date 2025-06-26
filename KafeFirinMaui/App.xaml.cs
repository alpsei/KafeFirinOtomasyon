using KafeFirinMaui.Services.Interfaces;
using KafeFirinMaui.Views;
using System.Globalization;

namespace KafeFirinMaui
{
    public partial class App : Application
    {
        private readonly IUserService _userService;
        public static IServiceProvider Services { get; set; }
        public App(IUserService userService, IServiceProvider services)
        {
            InitializeComponent();
            _userService = userService;
            var cultureInfo = new CultureInfo("tr-TR");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            MainPage = new AppShell();
            Services = services;
        }
    }
}

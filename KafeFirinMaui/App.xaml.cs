using KafeFirinMaui.Services;
using KafeFirinMaui.Views;

namespace KafeFirinMaui
{
    public partial class App : Application
    {
        private readonly UserService _userService;
        public static IServiceProvider Services { get; set; }
        public App(UserService userService, IServiceProvider services)
        {
            InitializeComponent();
            _userService = userService;

            MainPage = new AppShell();
            Services = services;
        }
    }
}

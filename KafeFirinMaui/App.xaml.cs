using KafeFirinMaui.Services;
using KafeFirinMaui.Views;

namespace KafeFirinMaui
{
    public partial class App : Application
    {
        private readonly UserService _userService;

        public App(UserService userService)
        {
            InitializeComponent();
            _userService = userService;

            MainPage = new NavigationPage(new StaffMainMenu());
        }
    }
}

using KafeFirinMaui.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using KafeFirinMaui.Views;


namespace KafeFirinMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7210");
            });
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<UserLogin>();
            builder.Services.AddSingleton<CustomerRegister>();
            return builder.Build();
        }
    }
}

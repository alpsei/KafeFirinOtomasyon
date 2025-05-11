using KafeFirinMaui.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using KafeFirinMaui.Views;
using KafeFirinMaui;
using KafeFirinMaui.ViewModels;
using System.Text.Json;


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
            builder.Services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<UserLogin>();
            builder.Services.AddSingleton<CustomerRegister>();
            builder.Services.AddTransient<CustomerOrders>();
            builder.Services.AddTransient<CustomerOrdersViewModel>();
            builder.Services.AddTransient<CustomerOrders>();
            builder.Services.AddTransient<ProductServices>();
            builder.Services.AddSingleton<OrderService>();
            builder.Services.AddTransient<EmployeeViewModel>();
            builder.Services.AddTransient<RateEmployee>();
            builder.Services.AddTransient<OrderHistory>();
            builder.Services.AddTransient<OrderHistoryViewModel>();
            builder.Services.AddTransient<UserSettingsViewModel>();
            builder.Services.AddTransient<UserSettings>();
            var app = builder.Build();

            App.Services = app.Services;

            return app;

        }
    }
}

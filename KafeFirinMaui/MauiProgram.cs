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
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
            });
            builder.Services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            builder.Services.AddTransient<UserService>();
            builder.Services.AddTransient<UserLogin>();
            builder.Services.AddTransient<CustomerRegister>();
            builder.Services.AddTransient<CustomerOrders>();
            builder.Services.AddTransient<CustomerOrdersViewModel>();
            builder.Services.AddTransient<CustomerOrders>();
            builder.Services.AddTransient<ProductServices>();
            builder.Services.AddTransient<OrderService>();
            builder.Services.AddTransient<EmployeeViewModel>();
            builder.Services.AddTransient<RateEmployee>();
            builder.Services.AddTransient<OrderHistory>();
            builder.Services.AddTransient<OrderHistoryViewModel>();
            builder.Services.AddTransient<UserSettingsViewModel>();
            builder.Services.AddTransient<UserSettings>();
            builder.Services.AddTransient<ProductViewModel>();
            builder.Services.AddTransient<ProductStock>();
            builder.Services.AddTransient<EmployeeAddOrRemove>();
            builder.Services.AddTransient<ManagerSettings>();
            builder.Services.AddTransient<FeedbackService>();
            builder.Services.AddTransient<FeedbackViewModel>();
            builder.Services.AddTransient<CustomerFeedback>();
            builder.Services.AddTransient<SalaryManagement>();
            builder.Services.AddTransient<ForgotYourPassword>();
            builder.Services.AddTransient<RateService>();
            builder.Services.AddTransient<RateViewModel>();
            builder.Services.AddTransient<EmployeeRatePageViewModel>(sp =>
            new EmployeeRatePageViewModel(
            sp.GetRequiredService<EmployeeViewModel>(),
            sp.GetRequiredService<RateViewModel>()));
            builder.Services.AddTransient<EmployeeRateList>();
            builder.Services.AddTransient<ShowAvgRateOfEmployee>();
            builder.Services.AddTransient<AddProduct>();
            builder.Services.AddTransient<FeedbackHistory>();
            builder.Services.AddTransient<ShowFeedbacks>();
            builder.Services.AddTransient<DeleteEmployee>();
            builder.Services.AddTransient<StaffOrdersViewModel>();
            builder.Services.AddTransient<StaffOrders>();

            var app = builder.Build();

            App.Services = app.Services;

            return app;

        }
    }
}

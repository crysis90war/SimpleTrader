using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleTrader.API;
using SimpleTrader.API.Services;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.Domain.Services.AuthenticationServices;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.EF;
using SimpleTrader.EF.Services;
using SimpleTrader.WPF.State.Accounts;
using SimpleTrader.WPF.State.Assets;
using SimpleTrader.WPF.State.Authenticators;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System.Windows;

namespace SimpleTrader.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json");
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((builder, services) =>
                {
                    string api = builder.Configuration.GetValue<string>("FINANCE_API_KEY");
                    services.AddSingleton(new FinancialModelingPropHttpClientFactory(api));

                    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                    services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));
                    services.AddSingleton<ApplicationDbContextFactory>(new ApplicationDbContextFactory(connectionString));

                    services.AddSingleton<IDataService<Account>, AccountDataService>();
                    services.AddSingleton<IAccountService, AccountDataService>();
                    services.AddSingleton<IAuthenticationService, AuthenticationService>();
                    services.AddSingleton<IStockPriceService, StockPriceService>();
                    services.AddSingleton<IBuyStockService, BuyStockService>();
                    services.AddSingleton<IMajorIndexService, MajorIndexService>();
                    services.AddSingleton<IPasswordHasher, PasswordHasher>();
                    services.AddSingleton<ISimpleTraderViewModelFactory, SimpleTraderViewModelFactory>();

                    services.AddSingleton<AssetSummaryViewModel>();
                    services.AddSingleton<BuyViewModel>();
                    services.AddSingleton<PortfolioViewModel>();

                    services.AddSingleton<ViewModelDelegateRenavigator<HomeViewModel>>();
                    services.AddSingleton<ViewModelDelegateRenavigator<RegisterViewModel>>();
                    services.AddSingleton<ViewModelDelegateRenavigator<LoginViewModel>>();

                    services.AddSingleton<HomeViewModel>(services => new HomeViewModel(
                        services.GetRequiredService<AssetSummaryViewModel>(),
                        MajorIndexListingViewModel.LoadMajorIndexViewModel(
                            services.GetRequiredService<IMajorIndexService>())));

                    services.AddSingleton<CreateViewModel<HomeViewModel>>(services =>
                    {
                        return () => services.GetRequiredService<HomeViewModel>();
                    });

                    services.AddSingleton<CreateViewModel<BuyViewModel>>(services =>
                    {
                        return () => services.GetRequiredService<BuyViewModel>();
                    });

                    services.AddSingleton<CreateViewModel<PortfolioViewModel>>(services =>
                    {
                        return () => services.GetRequiredService<PortfolioViewModel>();
                    });

                    services.AddSingleton<CreateViewModel<LoginViewModel>>(services =>
                    {
                        return () => new LoginViewModel(
                            services.GetRequiredService<IAuthenticator>(),
                            services.GetRequiredService<ViewModelDelegateRenavigator<HomeViewModel>>(),
                            services.GetRequiredService<ViewModelDelegateRenavigator<RegisterViewModel>>()
                            );
                    });

                    services.AddSingleton<CreateViewModel<RegisterViewModel>>(services =>
                    {
                        return () => new RegisterViewModel(
                            services.GetRequiredService<IAuthenticator>(),
                            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
                            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>());
                    });

                    services.AddSingleton<INavigator, Navigator>();
                    services.AddSingleton<IAuthenticator, Authenticator>();
                    services.AddSingleton<IAccountStore, AccountStore>();
                    services.AddSingleton<AssetStore>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton(s => new MainWindow(s.GetRequiredService<MainViewModel>()));

                });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            Window window = _host.Services.GetRequiredService<MainWindow>();
            //IAuthenticationService authenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();
            //var account = await authenticationService.Register("samadh90@icloud.com", "crysis90war", "samadh90", "samadh90");
            window.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}

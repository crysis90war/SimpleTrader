using Microsoft.Extensions.DependencyInjection;
using SimpleTrader.API;
using SimpleTrader.API.Services;
using SimpleTrader.EF;
using SimpleTrader.EF.Services;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.Domain.Services.TransactionServices;
using System;
using System.Configuration;
using System.Windows;
using SimpleTrader.Domain.Services.AuthenticationServices;
using Microsoft.AspNet.Identity;

namespace SimpleTrader.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();

            Window window = serviceProvider.GetRequiredService<MainWindow>();

            window.Show();

            base.OnStartup(e);
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            string api = ConfigurationManager.AppSettings.Get("financeApiKey");

            services.AddSingleton(new FinancialModelingPropHttpClientFactory(api));

            services.AddSingleton<ApplicationDbContextFactory>();

            services.AddSingleton<IDataService<Account>, AccountDataService>();

            services.AddSingleton<IAccountService, AccountDataService>();

            services.AddSingleton<IAuthenticationService, AuthenticationService>();

            services.AddSingleton<IStockPriceService, StockPriceService>();

            services.AddSingleton<IBuyStockService, BuyStockService>();

            services.AddSingleton<IMajorIndexService, MajorIndexService>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddSingleton<IRootSimpleTraderViewModelFactory, RootSimpleTraderViewModelFactory>();

            services.AddSingleton<ISimpleTraderViewModelFactory<HomeViewModel>, HomeViewModelFactory>();

            services.AddSingleton<ISimpleTraderViewModelFactory<PortfolioViewModel>, PortfolioViewModelFactory>();

            services.AddSingleton<ISimpleTraderViewModelFactory<MajorIndexListingViewModel>, MajorIndexListingViewModelFactory>();

            services.AddScoped<INavigator, Navigator>();

            services.AddScoped<MainViewModel>();

            services.AddScoped<BuyViewModel>();

            services.AddScoped(s => new MainWindow(s.GetRequiredService<MainViewModel>()));

            return services.BuildServiceProvider();
        }
    }
}

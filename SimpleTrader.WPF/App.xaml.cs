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
using SimpleTrader.WPF.State.Authenticators;
using SimpleTrader.WPF.State.Accounts;

namespace SimpleTrader.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();

            Window window = serviceProvider.GetRequiredService<MainWindow>();


            IAuthenticationService authenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();

            //var account = await authenticationService.Register("samadh90@icloud.com", "crysis90war", "samadh90", "samadh90");

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

            services.AddSingleton<ISimpleTraderViewModelFactory, SimpleTraderViewModelFactory>();

            services.AddSingleton<HomeViewModel>(services => new HomeViewModel(
                MajorIndexListingViewModel.LoadMajorIndexViewModel(
                    services.GetRequiredService<IMajorIndexService>())));

            services.AddSingleton<CreateViewModel<HomeViewModel>>(services =>
            {
                return () => services.GetRequiredService<HomeViewModel>();
            });

            services.AddSingleton<BuyViewModel>();

            services.AddSingleton<CreateViewModel<BuyViewModel>>(services =>
            {
                return () => services.GetRequiredService<BuyViewModel>();
            });

            services.AddSingleton<PortfolioViewModel>();

            services.AddSingleton<CreateViewModel<PortfolioViewModel>>(services =>
            {
                return () => services.GetRequiredService<PortfolioViewModel>();
            });

            services.AddSingleton<ViewModelDelegateRenavigator<HomeViewModel>>();

            services.AddSingleton<CreateViewModel<LoginViewModel>>(services =>
            {
                return () => new LoginViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<HomeViewModel>>());
            });

            services.AddSingleton<INavigator, Navigator>();

            services.AddSingleton<IAuthenticator, Authenticator>();

            services.AddSingleton<IAccountStore, AccountStore>();

            services.AddSingleton<MainViewModel>();

            services.AddSingleton<BuyViewModel>();

            services.AddSingleton(s => new MainWindow(s.GetRequiredService<MainViewModel>()));

            return services.BuildServiceProvider();
        }
    }
}

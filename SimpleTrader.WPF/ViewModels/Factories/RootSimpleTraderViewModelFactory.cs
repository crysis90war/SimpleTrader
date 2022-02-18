using SimpleTrader.WPF.State.Navigators;
using System;

namespace SimpleTrader.WPF.ViewModels.Factories
{
    public class RootSimpleTraderViewModelFactory : IRootSimpleTraderViewModelFactory
    {
        private readonly ISimpleTraderViewModelFactory<HomeViewModel> _homeViewModelFactory;
        private readonly ISimpleTraderViewModelFactory<PortfolioViewModel> _portfolioViewModelFactory;
        private readonly ISimpleTraderViewModelFactory<LoginViewModel> _loginViewModelFactory;
        private readonly BuyViewModel _buyViewModel;

        public RootSimpleTraderViewModelFactory(
            ISimpleTraderViewModelFactory<HomeViewModel> homeViewModelFactory, 
            ISimpleTraderViewModelFactory<PortfolioViewModel> portfolioViewModelFactory,
            ISimpleTraderViewModelFactory<LoginViewModel> loginViewModelFactory,
            BuyViewModel buyViewModel)
        {
            _homeViewModelFactory = homeViewModelFactory;
            _portfolioViewModelFactory = portfolioViewModelFactory;
            _loginViewModelFactory = loginViewModelFactory;
            _buyViewModel = buyViewModel;
        }


        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Login:
                    return _loginViewModelFactory.CreateViewModel();
                case ViewType.Home:
                    return _homeViewModelFactory.CreateViewModel();
                case ViewType.Portfolio:
                    return _portfolioViewModelFactory.CreateViewModel();
                case ViewType.Buy:
                    return _buyViewModel;
                default:
                    throw new ArgumentException($"The {viewType} does not have a ViewModel.", nameof(viewType));
            }
        }
    }
}

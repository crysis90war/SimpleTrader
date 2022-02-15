namespace SimpleTrader.WPF.ViewModels.Factories
{
    public interface ISimpleTraderViewModelFactory<TViewModel> where TViewModel : ViewModelBase
    {
        TViewModel CreateViewModel();
    }
}

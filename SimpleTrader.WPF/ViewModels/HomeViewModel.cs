namespace SimpleTrader.WPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public AssetSummaryViewModel AssetSummaryViewModel { get; set; }
        public MajorIndexListingViewModel MajorIndexListingViewModel { get; set; }

        public HomeViewModel(AssetSummaryViewModel assetSummaryViewModel, MajorIndexListingViewModel majorIndexListingViewModel)
        {
            AssetSummaryViewModel = assetSummaryViewModel;
            MajorIndexListingViewModel = majorIndexListingViewModel;
        }
    }
}

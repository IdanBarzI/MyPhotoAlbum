using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MyPhotoAlbum.Services.Service;
using Windows.UI.Xaml.Input;

namespace MyPhotoAlbum.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {

        private readonly INavigationService _navigationService = FrameNavigationService.Init;

        public NavigationViewModel() {  }


        public void AddPhoto_Tapped(object sender, TappedRoutedEventArgs e) => _navigationService.NavigateTo("AddPhoto");

        public void Album_Tapped(object sender, TappedRoutedEventArgs e) => _navigationService.NavigateTo("Album");

        public void Exit_Tapped(object sender, TappedRoutedEventArgs e) => App.Current.Exit();
    }
}
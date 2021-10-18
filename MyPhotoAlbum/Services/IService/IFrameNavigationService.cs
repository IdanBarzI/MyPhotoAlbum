using GalaSoft.MvvmLight.Views;

namespace MyPhotoAlbum.Services.IService
{
    public interface IFrameNavigationService : INavigationService
    {
        string CurrentPage { get; }

        new void NavigateTo(string page);

        new void NavigateTo(string page, object parameter);

        new void GoBack();
    }
}

using MyPhotoAlbum.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MyPhotoAlbum.ViewModels
{
    public class AlbumViewModel
    {

        public ObservableCollection<Photo> photos;

        public ObservableCollection<Photo> Photos
        {
            get { return photos; }
            set
            {
                if (photos != value) 
                    photos = value; 
            }
        }

        public AlbumViewModel()
        {
            photos = new ObservableCollection<Photo>();
            GetPhotos();
        }

        private async void GetPhotos()
        {
            StorageFolder destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("Album",
                CreationCollisionOption.OpenIfExists);
            var files = await destinationFolder.GetFilesAsync();
            foreach (var file in files)
            {
                Photo photo = new Photo();
                photo.PhotoName = file.Name;
                photo.ImageSource = file.Path;
                photo.Date = file.DateCreated.Date.ToString();
                photos.Add(photo);
            }
        }

    }
}

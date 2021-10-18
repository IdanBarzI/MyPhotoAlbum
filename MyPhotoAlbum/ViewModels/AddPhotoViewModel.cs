using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;

namespace MyPhotoAlbum.ViewModels
{
    public class AddPhotoViewModel : INotifyPropertyChanged
    {
        private StorageFile photo;

        private ImageSource source;

        public ImageSource Source
        {
            get { return source; }
            set
            {
                if (source != value)
                    { source = value; onPropertyChanged("Source"); }
            }
        }

        private string picName;

        public string PicName
        {
            get { return picName; }
            set
            {
                if (picName != value)
                    { picName = value;  onPropertyChanged("PicName"); }
            }
        }

        public AddPhotoViewModel()
        {
            source = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        CameraCaptureUI captureUI = new CameraCaptureUI();

        private async Task CamAsync()
        {
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(600, 338);

            photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null) return;

            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);

            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

            Source = bitmapSource;
        }

        private async void SaveCameraPic()
        {
            if (photo == null) return;

            StorageFolder destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("Album",
                CreationCollisionOption.OpenIfExists);
            await photo.CopyAsync(destinationFolder, PicName + ".jpg", NameCollisionOption.GenerateUniqueName);
            Source = null;
            PicName = "";

        }

        public void StackPanel_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "Copy";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        public async void StackPanel_DropAsync(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var images = await e.DataView.GetStorageItemsAsync();
                if (images.Any())
                {
                    var storageFile = images[0] as StorageFile;
                    photo = storageFile;
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(await storageFile.OpenAsync(FileAccessMode.Read));
                    Source = bitmapImage;
                }
            }
        }

        public void StartCam_Click(object sender, RoutedEventArgs e) => _ = CamAsync();

        public void Save_Click(object sender, RoutedEventArgs e) =>  SaveCameraPic();
    }
}

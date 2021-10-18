using MyPhotoAlbum.Services.IService;
using MyPhotoAlbum.Views.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyPhotoAlbum.Services.Service
{
    public class FrameNavigationService : IFrameNavigationService
    {
        private readonly IDictionary<string, Type> pages_ = new Dictionary<string, Type>();

        private readonly List<string> _historic;

        public const string RootPage = "(Root)";

        public const string UnknownPage = "(Unknown)";

        private static Frame AppFrame => ((Window.Current.Content as Frame)?.Content as MainPage)?.AppFrame;

        private static FrameNavigationService init;

        public static FrameNavigationService Init
        {
            get
            {
                if (init == null)
                {
                    init = new FrameNavigationService();
                    SetupNavigation();
                }
                return init;
            }
        }

        private static void SetupNavigation()
        {
            init.Configure("AddPhoto", typeof(AddPhoto));
            init.Configure("Album", typeof(Album));
        }

        public void Configure(string page, Type type)
        {
            lock (pages_)
            {
                if (pages_.ContainsKey(page))
                    throw new ArgumentException("The specified page is already registered.");

                if (pages_.Values.Any(v => v == type))
                    throw new ArgumentException("The specified view has already been registered under another name.");

                pages_.Add(page, type);
            }
        }

        public string CurrentPage
        {
            get
            {
                var frame = AppFrame;
                if (frame.BackStackDepth == 0)
                    return RootPage;

                if (frame.Content == null)
                    return UnknownPage;

                var type = frame.Content.GetType();

                lock (pages_)
                {
                    if (pages_.Values.All(v => v != type))
                        return UnknownPage;

                    var item = pages_.Single(i => i.Value == type);

                    return item.Key;
                }
            }
        }

        public object Parameter => throw new NotImplementedException();

        public string CurrentPageKey => throw new NotImplementedException();

        public void NavigateTo(string page)
        {
            NavigateTo(page, null);
        }

        public void NavigateTo(string page, object parameter)
        {
            lock (pages_)
            {
                if (!pages_.ContainsKey(page))
                    throw new ArgumentException("Unable to find a page registered with the specified name.");

                System.Diagnostics.Debug.Assert(AppFrame != null);
                AppFrame.Navigate(pages_[page], parameter);
            }
        }

        public void GoBack()
        {
            System.Diagnostics.Debug.Assert(AppFrame != null);
            if (AppFrame.CanGoBack)
                AppFrame.GoBack();
        }
    }
}

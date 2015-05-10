using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;

namespace ProjectOxfordPrototype.ViewModel
{
    public class ThumbnailViewModel : ViewModelBase
    {
        private BitmapImage _sourceImage;

        /// <summary>
        /// Sets and gets the SourceImage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BitmapImage SourceImage
        {
            get { return _sourceImage; }
            set { Set(() => SourceImage, ref _sourceImage, value); }
        }

        private BitmapImage _thumbnail;

        /// <summary>
        /// Sets and gets the Thumbnail property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BitmapImage Thumbnail
        {
            get { return _thumbnail; }
            set { Set(() => Thumbnail, ref _thumbnail, value); }
        }
    }
}

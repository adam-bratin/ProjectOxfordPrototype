using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using Microsoft.ProjectOxford.Vision.Contract;

namespace ProjectOxfordPrototype.ViewModel
{
    public class ImageAnalysisViewModel : ViewModelBase
    {
        private BitmapImage _image;

        /// <summary>
        /// Sets and gets the Image property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BitmapImage Image
        {
            get { return _image; }
            set { Set(() => Image, ref _image, value); }
        }

        private AnalysisResult _analysisResults;

        /// <summary>
        /// Sets and gets the AnalysisResults property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AnalysisResult AnalysisResults
        {
            get { return _analysisResults; }
            set { Set(() => AnalysisResults, ref _analysisResults, value); }
        }

        private Visibility _visibility;

        /// <summary>
        /// Sets and gets the Visibility property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility Visibility
        {
            get { return _visibility; }
            set { Set(() => Visibility, ref _visibility, value); }
        }
    }
}

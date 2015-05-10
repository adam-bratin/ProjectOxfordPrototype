using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ProjectOxfordPrototype.ViewModel;

namespace ProjectOxfordPrototype
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly VisionService _visionService;

        private RelayCommand _analyzeImageCommand;

        public RelayCommand AnalyzeImageCommand
        {
            get
            {
                return this._analyzeImageCommand ?? new RelayCommand(() =>
                {
                    var task = this.AnalyzeImageAsync();
                });
            }
        }

        private ImageAnalysisViewModel _imageAnalysisViewModel;

        /// <summary>
        /// Sets and gets the ImageAnalysisViewModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ImageAnalysisViewModel ImageAnalysisViewModel
        {
            get { return _imageAnalysisViewModel; }
            set { Set(() => ImageAnalysisViewModel, ref _imageAnalysisViewModel, value); }
        }

        private async Task AnalyzeImageAsync()
        {
            this.ImageAnalysisViewModel = this.ImageAnalysisViewModel ?? new ImageAnalysisViewModel();
            FileOpenPicker op = new FileOpenPicker();
            var file = await op.PickSingleFileAsync();
            BitmapImage image = new BitmapImage(new Uri(file.Path));
            await this._visionService.AnalyzeImageAsync(file.Path);
        }

        public MainPageViewModel()
        {
            this._visionService = new VisionService();
        }
    }
}

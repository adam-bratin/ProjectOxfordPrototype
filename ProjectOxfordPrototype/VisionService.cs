using Microsoft.ProjectOxford.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.ProjectOxford.Vision.Contract;

namespace ProjectOxfordPrototype
{
    public class VisionService
    {
        private VisionServiceClient _serviceClient;

        public VisionServiceClient ServiceClient
        {
            get { return _serviceClient; }
            private set { _serviceClient = value; }
        }

        public VisionService()
        {
            this._serviceClient = new VisionServiceClient(Constants.Constants.VisionSubscriptionKey);
        }

        public async Task<AnalysisResult> AnalyzeImageAsync(string imagePathOrUrl)
        {
            if (string.IsNullOrEmpty(imagePathOrUrl))
            {
                throw new ArgumentNullException(paramName: GetPropertyName(() => imagePathOrUrl));
            }
            var options = new[] {"All"};
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(imagePathOrUrl);
                var stream = await file.OpenReadAsync();
                return await this._serviceClient.AnalyzeImageAsync(stream.AsStream(), options);

            }
            catch (FileNotFoundException)
            {
                if (Uri.IsWellFormedUriString(imagePathOrUrl, UriKind.Absolute))
                {
                    return await this._serviceClient.AnalyzeImageAsync(imagePathOrUrl, options);
                }
                else
                {
                    throw new FormatException(GetPropertyName(() => imagePathOrUrl) + "was not a valid URL");
                }
            }
            catch (ClientException e)
            {
                throw e;
            }
        }

        public async Task<BitmapImage> GetThumbnailAsync(string imagePathOrUrl, StorageFolder resultFolder,
            int height = 512, int width = 512, bool smartCropping = true)
        {
            if (resultFolder == null)
            {
                throw new ArgumentNullException(paramName: GetPropertyName(() => resultFolder));
            }
            if (string.IsNullOrEmpty(imagePathOrUrl))
            {
                throw new ArgumentNullException(paramName: GetPropertyName(() => imagePathOrUrl));
            }
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(imagePathOrUrl);
                var stream = await file.OpenStreamForReadAsync();
                var image = await this._serviceClient.GetThumbnailAsync(stream, width, height, smartCropping);
                this.SaveWritableBitmapImage(image, Path.GetFileName(imagePathOrUrl),(uint)width, (uint) height, resultFolder);
                return await this.GetBitmapImageAsync(image);
            }
            catch (FileNotFoundException)
            {
                if (Uri.IsWellFormedUriString(imagePathOrUrl, UriKind.Absolute))
                {
                    var uri = new Uri(imagePathOrUrl);
                    var image = await this._serviceClient.GetThumbnailAsync(imagePathOrUrl, width, height, smartCropping);
                    this.SaveWritableBitmapImage(image, uri.Segments.Last(), (uint)width, (uint)height, resultFolder);
                    return await this.GetBitmapImageAsync(image);
                }
                else
                {
                    throw new FormatException(GetPropertyName(() => imagePathOrUrl) + "was not a valid URL");
                }
            }
            catch (ClientException e)
            {
                throw e;
            }
        }

        private async Task<BitmapImage> GetBitmapImageAsync(byte[] data)
        {
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(data);
                    await writer.StoreAsync();
                }
                var image = new BitmapImage();
                await image.SetSourceAsync(stream);
                return image;
            }   
        }

        private async void SaveWritableBitmapImage(byte[] data, string filename, uint width, uint height,
            StorageFolder destinationFolder)
        {
            var outputFile = await destinationFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            using (var writeStream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, writeStream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied,
                   width, height,
                   96, 96, data);
                await encoder.FlushAsync();
            }
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            return memberExpression != null ? memberExpression.Member.Name : "";
        }
    }
}

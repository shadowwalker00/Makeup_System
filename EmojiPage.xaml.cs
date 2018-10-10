using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace BeautyFace
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EmojiPage : Page
    {
        private StorageFile curImage;
        private Brush foreColor;
        private int foreSize;
        public EmojiPage()
        {
            this.InitializeComponent();
        }
        private async void cameraButton_Click(object sender, RoutedEventArgs e)
        {
            //清理屏幕
            clearMyCanvas();
            //访问摄像头
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);
            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            //显示图片
            showImage(photo);
        }

        private async void galleryButton_Click(object sender, RoutedEventArgs e)
        {
            //清理屏幕
            clearMyCanvas();

            // 从本地图像文件夹选择照片方法
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            try
            {
                //本地获取一张照片
                var image_file = await picker.PickSingleFileAsync();
                if (image_file != null)
                {
                    //显示图片
                    showImage(image_file);
                }
            }
            catch
            {

            }
        }
        private void clearMyCanvas()
        {
            while (sourceEmoji.Children.Count > 0)
            {
                sourceEmoji.Children.RemoveAt(sourceEmoji.Children.Count - 1);
            }
        }
        private async void showImage(StorageFile image_file)
        {
            //保存到私有成员变量curImage
            curImage = image_file;
            //StorageFile->irandomAccessStream->Bitmap->SoftWareBitmap->SoftWareBitmapSource
            IRandomAccessStream fileStream = await image_file.OpenAsync(FileAccessMode.Read);

            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
            SoftwareBitmap imageBitmap = await decoder.GetSoftwareBitmapAsync();
            SoftwareBitmap displayableImage = SoftwareBitmap.Convert(imageBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore);
            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(displayableImage);

            //保存原图像的尺寸
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = bitmapSource;
            this.sourceEmoji.Background = brush;
        }
        private async void produceEmojiButton_Click(object sender, RoutedEventArgs e)
        {
            while (this.emogiCan1.Children.Count > 0)
            {
                this.emogiCan1.Children.RemoveAt(this.emogiCan1.Children.Count - 1);
            }
            while (this.emogiCan2.Children.Count > 0)
            {
                this.emogiCan2.Children.RemoveAt(this.emogiCan2.Children.Count - 1);
            }
            while (this.emogiCan1.Children.Count > 0)
            {
                this.emogiCan2.Children.RemoveAt(this.emogiCan2.Children.Count - 1);
            }
            while (this.emogiCan3.Children.Count > 0)
            {
                this.emogiCan3.Children.RemoveAt(this.emogiCan3.Children.Count - 1);
            }
            while (this.emogiCan4.Children.Count > 0)
            {
                this.emogiCan4.Children.RemoveAt(this.emogiCan4.Children.Count - 1);
            }
            while (this.emogiCan5.Children.Count > 0)
            {
                this.emogiCan5.Children.RemoveAt(this.emogiCan5.Children.Count - 1);
            }
            while (this.emogiCan6.Children.Count > 0)
            {
                this.emogiCan6.Children.RemoveAt(this.emogiCan6.Children.Count - 1);
            }
            while (this.emogiCan7.Children.Count > 0)
            {
                this.emogiCan7.Children.RemoveAt(this.emogiCan7.Children.Count - 1);
            }
            while (this.emogiCan8.Children.Count > 0)
            {
                this.emogiCan8.Children.RemoveAt(this.emogiCan8.Children.Count - 1);
            }
            while (this.emogiCan9.Children.Count > 0)
            {
                this.emogiCan9.Children.RemoveAt(this.emogiCan9.Children.Count - 1);
            }
            StorageFile image_file = curImage;
            IRandomAccessStream fileStream = await image_file.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
            SoftwareBitmap imageBitmap = await decoder.GetSoftwareBitmapAsync();
            SoftwareBitmap displayableImage = SoftwareBitmap.Convert(imageBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore);
            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(displayableImage);
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = bitmapSource;

            this.emogiCan1.Background = brush;
            TextBlock word1 = new TextBlock();
            Canvas.SetLeft(word1, 10);
            word1.Text = "请开始你的表演";
            word1.FontSize = foreSize;
            word1.Foreground = foreColor;
            Debug.WriteLine(word1.ActualHeight);
            Debug.WriteLine(word1.Height);
            Canvas.SetTop(word1, emogiCan1.ActualHeight-2*foreSize);
            
            this.emogiCan1.Children.Add(word1);

            this.emogiCan2.Background = brush;
            TextBlock word2 = new TextBlock();
            Canvas.SetLeft(word2, 10);
            Canvas.SetTop(word2, emogiCan2.ActualHeight - 2 * foreSize);
            word2.Text = "因吹斯汀~";
            word2.FontSize = foreSize;
            word2.Foreground = foreColor;
            this.emogiCan2.Children.Add(word2);

            this.emogiCan3.Background = brush;
            TextBlock word3 = new TextBlock();
            Canvas.SetLeft(word3, 10);
            Canvas.SetTop(word3, emogiCan3.ActualHeight - 2 * foreSize);
            word3.Text = "你瞅啥！";
            word3.FontSize = foreSize;
            word3.Foreground = foreColor;
            this.emogiCan3.Children.Add(word3);

            this.emogiCan4.Background = brush;
            TextBlock word4 = new TextBlock();
            Canvas.SetLeft(word4, 10);
            Canvas.SetTop(word4, emogiCan1.ActualHeight - 2 * foreSize);
            word4.Text = "哼！愚蠢的人类！";
            word4.FontSize = foreSize;
            word4.Foreground = foreColor;
            this.emogiCan4.Children.Add(word4);
            this.emogiCan5.Background = brush;
            TextBlock word5 = new TextBlock();
            Canvas.SetLeft(word5, 10);
            Canvas.SetTop(word5, emogiCan1.ActualHeight - 2 * foreSize);
            word5.Text = "你为啥还不撩我！";
            word5.FontSize = foreSize;
            word5.Foreground = foreColor;
            this.emogiCan5.Children.Add(word5);

            this.emogiCan6.Background = brush;
            TextBlock word6 = new TextBlock();
            Canvas.SetLeft(word6, 10);
            Canvas.SetTop(word6, emogiCan1.ActualHeight - 2 * foreSize);
            word6.Text = "总有刁民想害朕";
            word6.FontSize = foreSize;
            word6.Foreground = foreColor;
            this.emogiCan6.Children.Add(word6);

            this.emogiCan7.Background = brush;
            TextBlock word7 = new TextBlock();
            Canvas.SetLeft(word7, 10);
            Canvas.SetTop(word7, emogiCan1.ActualHeight - 2 * foreSize);
            word7.Text = "约不约？";
            word7.FontSize = foreSize;
            word7.Foreground = foreColor;
            this.emogiCan7.Children.Add(word7);

            this.emogiCan8.Background = brush;
            TextBlock word8 = new TextBlock();
            Canvas.SetLeft(word8, 10);
            Canvas.SetTop(word8, emogiCan1.ActualHeight - 2 * foreSize);
            word8.Text = "叫爸爸！";
            word8.FontSize = foreSize;
            word8.Foreground = foreColor;
            this.emogiCan8.Children.Add(word8);

            this.emogiCan9.Background = brush;
            TextBlock word9 = new TextBlock();
            Canvas.SetLeft(word9, 10);
            Canvas.SetTop(word9, emogiCan1.ActualHeight - 2 * foreSize);
            word9.Text = "我的内心毫无波动";
            word9.FontSize = foreSize;
            word9.Foreground = foreColor;
            this.emogiCan9.Children.Add(word9);
        }
        private void colorGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (redColor.IsSelected)
            {
                foreColor = new SolidColorBrush(Colors.Red);
            }
            else if (blueColor.IsSelected)
            {
                foreColor = new SolidColorBrush(Colors.Blue);
            }
            else if (pinkColor.IsSelected)
            {
                foreColor = new SolidColorBrush(Colors.Pink);
            }
        }
        private void sizeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Size2.IsSelected)
            {
                foreSize = 15;
            }
            else if (Size5.IsSelected)
            {
                foreSize = 20;
            }
            else if (Size8.IsSelected)
            {
                foreSize = 25;
            }
        }
        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(this.emogiCan1);
            StorageFolder folder;
            folder = ApplicationData.Current.LocalFolder;
            for(int i=0;i<9;i++)
            {
                string fileName = "emoji";
                fileName = fileName + i.ToString()+".png";
                StorageFile file = await folder.CreateFileAsync(fileName);
                if (file != null)
                {
                    var pixels = await renderTargetBitmap.GetPixelsAsync();
                    using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        var encoder = await
                            BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                        byte[] bytes = pixels.ToArray();
                        encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                             BitmapAlphaMode.Ignore,
                                             (uint)this.emogiCan1.ActualWidth, (uint)this.emogiCan1.ActualHeight,
                                             96, 96, bytes);
                        await encoder.FlushAsync();
                    }
                }
            }
        }
    }
}

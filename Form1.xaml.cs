using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.UI.Xaml.Shapes;
using BeautyFace.Models;
using BeautyFace.Cores;
using WinRTXamlToolkit.Imaging;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace BeautyFace
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Form1 : Page
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        #region 私有成员变量
        //private StorageFile curImage;
        private RootObject curFace;
        public static string curImgPath;
        int preHeight;
        int preWidth;
        private List<WallPaper> wallpapers;
        private List<FilterClass> filters;
        private List<DealFaceClass> tools;
        private pictureProcess dealer;
        private ImageBrush tempBrush;
        private byte[] tempImage;
        public StorageFile curImage { get; set; }
        #endregion
        public Form1()
        {
            this.InitializeComponent();
            wallpapers = WallPaperManager.getWallPapers();
            filters = FilterClassManager.getFilters();
            tools = DealFaceClassManager.getDealFaceClasses();
            dealer = new pictureProcess();
        }
        private void bottomList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ManipulatePanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RelativePanel_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
        private void Tab_Tapped(object sender, TappedRoutedEventArgs e)
        {
            List<RelativePanel> tabList = new List<RelativePanel> { Tab1, Tab2, Tab3,Tab4 };
            int oldIndex = ManipulatePanel.SelectedIndex;
            int index= tabList.IndexOf(sender as RelativePanel);
            ManipulatePanel.SelectedIndex=index;

            //修改之前点击的relatiePanel
            (tabList[oldIndex].Children[0] as TextBlock).Foreground = new SolidColorBrush(Colors.Gray);
            (tabList[oldIndex].Children[1] as Rectangle).Fill = new SolidColorBrush(Colors.Gray);

            //修改当前点击的relativePanel
            (tabList[index].Children[0] as TextBlock).Foreground = new SolidColorBrush(Colors.DarkOrange);
            (tabList[index].Children[1] as Rectangle).Fill = new SolidColorBrush(Colors.DarkOrange);
        }
       
        private async void showImage(StorageFile image_file)
        {
            //保存到私有成员变量curImage
            curImage = image_file;
            //progress ring
            WaitLoading.IsActive = true;
            //StorageFile->irandomAccessStream->Bitmap->SoftWareBitmap->SoftWareBitmapSource
            IRandomAccessStream fileStream = await image_file.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
            SoftwareBitmap imageBitmap = await decoder.GetSoftwareBitmapAsync();
            SoftwareBitmap displayableImage = SoftwareBitmap.Convert(imageBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore);
            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(displayableImage);
            
            //保存原图像的尺寸
            preHeight = imageBitmap.PixelHeight;
            preWidth = imageBitmap.PixelWidth;
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = bitmapSource;
            this.MyCanvas.Background = brush;
            WaitLoading.IsActive = false;
        }
        //识别当前人脸方法
        private async void recogBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WaitLoading.IsActive = true;
                RootObject myFace = await FaceProxy.GetFace(curImage);
                res.Text = "性别=" + myFace.faceAttributes.gender +
                    "年龄=" + myFace.faceAttributes.age.ToString() +
                    "高兴程度=" + myFace.faceAttributes.emotion.happiness.ToString();
                curFace = myFace;
                WaitLoading.IsActive = false;
            }
            catch
            {
                //异常处理
            }
        }
        private void markMyface(RootObject curFace)
        {
            //清理画布
            clearMyCanvas();
            //缩放尺寸
            
            double scaleHeight =MyCanvas.ActualHeight/ preHeight;
            double scaleWidth = MyCanvas.ActualWidth / preWidth;


            //标出人脸
            Rectangle faceBox = new Rectangle();
            faceBox.Width = curFace.faceRectangle.width*scaleWidth;
            faceBox.Height = curFace.faceRectangle.height * scaleHeight;
            faceBox.Fill = new SolidColorBrush(Windows.UI.Colors.Transparent);
            faceBox.Stroke = new SolidColorBrush(Windows.UI.Colors.Red) ;
            faceBox.StrokeThickness = 3;
            Canvas.SetLeft(faceBox,curFace.faceRectangle.left*scaleWidth);
            Canvas.SetTop(faceBox, curFace.faceRectangle.top*scaleHeight);
        }
        //清理画布方法
        private void clearMyCanvas()
        {
            while (MyCanvas.Children.Count > 0)
            {
                MyCanvas.Children.RemoveAt(MyCanvas.Children.Count - 1);
            }                
        }
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
        private void wallpaperItems_ItemClick(object sender, ItemClickEventArgs e)
        {
              
                
        }
        public void saveMyCanvas()
        {
            //保存当前画布内容到本地图片
        }
        private async void filterItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] getWidthHeight = await dealer.getWidthandHeight(curImage);
            var bytes = await dealer.StorageFileToBytes(curImage);
            tempImage = bytes;
            var filterObj = (FilterClass)e.ClickedItem;
            SoftwareBitmapSource showImage;
            byte[] newBytes;
            ImageBrush brush = new ImageBrush();

            if (filterObj.Name=="原图")
            {
                showImage = await dealer.bytesArrayToshowImage(bytes, getWidthHeight[0], getWidthHeight[1]);
                brush.ImageSource = showImage;
                this.MyCanvas.Background = brush;
            }
            else if(filterObj.Name=="灰白")
            {
                newBytes=dealer.blackEffect(bytes, getWidthHeight[0], getWidthHeight[1]);
                showImage = await dealer.bytesArrayToshowImage(newBytes, getWidthHeight[0], getWidthHeight[1]);
                brush.ImageSource = showImage;
                this.MyCanvas.Background = brush;
            }
            else if(filterObj.Name=="曝光")
            {
                newBytes = dealer.filmEffect(bytes, getWidthHeight[0], getWidthHeight[1]);
                showImage = await dealer.bytesArrayToshowImage(newBytes, getWidthHeight[0], getWidthHeight[1]);
                brush.ImageSource = showImage;
                this.MyCanvas.Background = brush;
            }
            else if (filterObj.Name == "美食")
            {

            }
            else if (filterObj.Name == "暖色")
            {
                newBytes = dealer.warmEffect(bytes, getWidthHeight[0], getWidthHeight[1]);
                showImage = await dealer.bytesArrayToshowImage(newBytes, getWidthHeight[0], getWidthHeight[1]);
                brush.ImageSource = showImage;
                this.MyCanvas.Background = brush;
            }
            else if (filterObj.Name == "冷色")
            {
                newBytes = dealer.coldEffect(bytes, getWidthHeight[0], getWidthHeight[1]);
                showImage = await dealer.bytesArrayToshowImage(newBytes, getWidthHeight[0], getWidthHeight[1]);
                brush.ImageSource = showImage;
                this.MyCanvas.Background = brush;
            }
            else if (filterObj.Name == "版画")
            {
                newBytes = dealer.engraveEffect(bytes, getWidthHeight[0], getWidthHeight[1]);
                showImage = await dealer.bytesArrayToshowImage(newBytes, getWidthHeight[0], getWidthHeight[1]);
                brush.ImageSource = showImage;
                this.MyCanvas.Background = brush;
            }
        }
        private async void toolItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] getWidthHeight = await dealer.getWidthandHeight(curImage);
            var bytes = await dealer.StorageFileToBytes(curImage);
            SoftwareBitmapSource showImage;
            byte[] newBytes;
            ImageBrush brush = new ImageBrush();
            var toolObj = (DealFaceClass)e.ClickedItem;
            if(toolObj.Name=="亮眼")
            {
                newBytes = dealer.brightEyes(bytes, getWidthHeight[0], getWidthHeight[1],
                    (int)curFace.faceLandmarks.eyeLeftOuter.x, (int)curFace.faceLandmarks.eyeLeftTop.y, (int)curFace.faceLandmarks.eyeLeftInner.x, (int)curFace.faceLandmarks.eyeLeftBottom.y,
                    (int)curFace.faceLandmarks.eyeRightInner.x, (int)curFace.faceLandmarks.eyeRightTop.y, (int)curFace.faceLandmarks.eyeRightOuter.x, (int)curFace.faceLandmarks.eyeRightBottom.y);
                showImage = await dealer.bytesArrayToshowImage(newBytes, getWidthHeight[0], getWidthHeight[1]);
                brush.ImageSource = showImage;
                this.MyCanvas.Background = brush;
            }
            else if(toolObj.Name=="美白")
            {
                newBytes = dealer.brightFace(bytes, getWidthHeight[0], getWidthHeight[1],
                    curFace.faceRectangle.left, curFace.faceRectangle.top,
                    curFace.faceRectangle.width + curFace.faceRectangle.left,
                    curFace.faceRectangle.height + curFace.faceRectangle.top);
                showImage = await dealer.bytesArrayToshowImage(newBytes, getWidthHeight[0], getWidthHeight[1]);
                brush.ImageSource = showImage;
                this.MyCanvas.Background = brush;
            }
        }
        private void wallperItems_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
        private async void brightSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if(curImage!=null)
            {
                int[] getWidthHeight = await dealer.getWidthandHeight(curImage);
            var bytes = await dealer.StorageFileToBytes(curImage);
            SoftwareBitmapSource showImage;
            byte[] newBytes;
            ImageBrush brush = new ImageBrush();
            newBytes = dealer.increBright(bytes, getWidthHeight[0], getWidthHeight[1], brightSlider.Value);
            showImage = await dealer.bytesArrayToshowImage(newBytes, getWidthHeight[0], getWidthHeight[1]);
            brush.ImageSource = showImage;
            this.MyCanvas.Background = brush;
            }
        }
        private async void blurSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if(curImage!=null)
            {
                int[] getWidthHeight = await dealer.getWidthandHeight(curImage);
                var bytes = await dealer.StorageFileToBytes(curImage);
                SoftwareBitmapSource showImage;
                byte[] newBytes;
                ImageBrush brush = new ImageBrush();
                newBytes = dealer.gaussBlur(bytes, getWidthHeight[1], getWidthHeight[0], (int)blurSlider.Value*2+5);
                showImage = await dealer.bytesArrayToshowImage(newBytes, getWidthHeight[0], getWidthHeight[1]);
                brush.ImageSource = showImage;
                this.MyCanvas.Background = brush;
            }
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
        private async  void Filebut_Click(object sender, RoutedEventArgs e)
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
        private async void savebut_Click(object sender, RoutedEventArgs e)
        {
            if (this.MyCanvas != null)
            {
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
                await renderTargetBitmap.RenderAsync(this.MyCanvas);
                var picker = new FileSavePicker();
                picker.FileTypeChoices.Add("JPEG Image", new string[] { ".jpg" });
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                StorageFile file = await picker.PickSaveFileAsync();
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
                                             (uint)this.MyCanvas.ActualWidth,(uint)this.MyCanvas.ActualHeight,
                                             96, 96, bytes);
                        await encoder.FlushAsync();
                    }
                }
            }
        }
    }
}

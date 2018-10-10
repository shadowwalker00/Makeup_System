using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace BeautyFace
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public class Emo
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
    public sealed partial class RecogPage : Page
    {
        private StorageFile curImage;
        private RootObject curFace;
        public void setCurImage(StorageFile input)
        {
            curImage = input;
        }
        int preHeight;
        int preWidth;
        public RecogPage()
        {
            this.InitializeComponent();

        }
        private async void recogBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WaitLoading.IsActive = true;
                RootObject myFace = await FaceProxy.GetFace(curImage);
                ID_text.Text = myFace.faceId.ToString();
                if(myFace.faceAttributes.gender.ToString()=="male")
                {
                    gender_text.Text = "男";
                }
                else
                {
                    gender_text.Text = "女";
                }
                age_text.Text = ((int)myFace.faceAttributes.age).ToString()+"岁";
                markMyface(myFace);
                List<Emo> emotions = new List<Emo>();
                emotions.Add(new Emo { Name = "生气", Value = myFace.faceAttributes.emotion.anger });
                emotions.Add(new Emo { Name = "恐惧", Value = myFace.faceAttributes.emotion.fear });
                emotions.Add(new Emo { Name = "悲伤", Value = myFace.faceAttributes.emotion.sadness });
                emotions.Add(new Emo { Name = "惊讶", Value = myFace.faceAttributes.emotion.surprise });
                emotions.Add(new Emo { Name = "高兴", Value = myFace.faceAttributes.emotion.happiness });
                emotions.Add(new Emo { Name = "中性", Value = myFace.faceAttributes.emotion.neutral });
                (pieChart.Series[0] as PieSeries).ItemsSource = emotions;
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
            //缩放尺寸

            double scaleHeight = First.ActualHeight / preHeight;
            double scaleWidth = First.ActualWidth / preWidth;


            //标出人脸
            Rectangle faceBox = new Rectangle();
            faceBox.Width = curFace.faceRectangle.width * scaleWidth;
            faceBox.Height = curFace.faceRectangle.height * scaleHeight;
            faceBox.Fill = new SolidColorBrush(Windows.UI.Colors.Transparent);
            faceBox.Stroke = new SolidColorBrush(Windows.UI.Colors.Red);
            faceBox.StrokeThickness = 3;
            Canvas.SetLeft(faceBox, curFace.faceRectangle.left * scaleWidth);
            Canvas.SetTop(faceBox, curFace.faceRectangle.top * scaleHeight);


            //标定特征点
            List<Ellipse> points = new List<Ellipse>();
            for (int i = 0; i < 28; i++)
            {
                Ellipse temp = new Ellipse();
                temp.Height = 8;
                temp.Width = 8;
                temp.Fill = new SolidColorBrush(Colors.Blue);
                points.Add(temp);
            }

            List<Line> lines = new List<Line>();
            for (int i = 0; i < 30; i++)
            {
                Line temp = new Line();
                temp.StrokeThickness = 2;
                temp.Stroke = new SolidColorBrush(Colors.White);
                lines.Add(temp);
            }
            Canvas.SetLeft(points[3], curFace.faceLandmarks.eyeLeftInner.x * scaleWidth);
            Canvas.SetTop(points[3], curFace.faceLandmarks.eyeLeftInner.y * scaleHeight);
            Canvas.SetLeft(points[4], curFace.faceLandmarks.eyeLeftTop.x * scaleWidth);
            Canvas.SetTop(points[4], curFace.faceLandmarks.eyeLeftTop.y * scaleHeight);
            Canvas.SetLeft(points[5], curFace.faceLandmarks.eyeLeftBottom.x * scaleWidth);
            Canvas.SetTop(points[5], curFace.faceLandmarks.eyeLeftBottom.y * scaleHeight);
            Canvas.SetLeft(points[6], curFace.faceLandmarks.eyeLeftOuter.x * scaleWidth);
            Canvas.SetTop(points[6], curFace.faceLandmarks.eyeLeftOuter.y * scaleHeight);
            Canvas.SetLeft(points[7], curFace.faceLandmarks.eyeRightInner.x * scaleWidth);
            Canvas.SetTop(points[7], curFace.faceLandmarks.eyeRightInner.y * scaleHeight);
            Canvas.SetLeft(points[8], curFace.faceLandmarks.eyeRightTop.x * scaleWidth);
            Canvas.SetTop(points[8], curFace.faceLandmarks.eyeRightTop.y * scaleHeight);
            Canvas.SetLeft(points[9], curFace.faceLandmarks.eyeRightBottom.x * scaleWidth);
            Canvas.SetTop(points[9], curFace.faceLandmarks.eyeRightBottom.y * scaleHeight);
            Canvas.SetLeft(points[10], curFace.faceLandmarks.eyeRightOuter.x * scaleWidth);
            Canvas.SetTop(points[10], curFace.faceLandmarks.eyeRightOuter.y * scaleHeight);

            Canvas.SetLeft(points[0], curFace.faceLandmarks.noseTip.x * scaleWidth);
            Canvas.SetTop(points[0], curFace.faceLandmarks.noseTip.y * scaleHeight);
            Canvas.SetLeft(points[1], curFace.faceLandmarks.mouthLeft.x * scaleWidth);
            Canvas.SetTop(points[1], curFace.faceLandmarks.mouthLeft.y * scaleHeight);
            Canvas.SetLeft(points[2], curFace.faceLandmarks.mouthRight.x * scaleWidth);
            Canvas.SetTop(points[2], curFace.faceLandmarks.mouthRight.y * scaleHeight);

            Canvas.SetLeft(points[11], curFace.faceLandmarks.eyebrowLeftInner.x * scaleWidth);
            Canvas.SetTop(points[11], curFace.faceLandmarks.eyebrowLeftInner.y * scaleHeight);
            Canvas.SetLeft(points[12], curFace.faceLandmarks.eyebrowLeftOuter.x * scaleWidth);
            Canvas.SetTop(points[12], curFace.faceLandmarks.eyebrowLeftOuter.y * scaleHeight);
            Canvas.SetLeft(points[13], curFace.faceLandmarks.eyebrowRightInner.x * scaleWidth);
            Canvas.SetTop(points[13], curFace.faceLandmarks.eyebrowRightInner.y * scaleHeight);
            Canvas.SetLeft(points[14], curFace.faceLandmarks.eyebrowRightOuter.x * scaleWidth);
            Canvas.SetTop(points[14], curFace.faceLandmarks.eyebrowRightOuter.y * scaleHeight);

            Canvas.SetLeft(points[15], curFace.faceLandmarks.noseLeftAlarOutTip.x * scaleWidth);
            Canvas.SetTop(points[15], curFace.faceLandmarks.noseLeftAlarOutTip.y * scaleHeight);
            Canvas.SetLeft(points[16], curFace.faceLandmarks.noseLeftAlarTop.x * scaleWidth);
            Canvas.SetTop(points[16], curFace.faceLandmarks.noseLeftAlarTop.y * scaleHeight);
            Canvas.SetLeft(points[17], curFace.faceLandmarks.noseRightAlarOutTip.x * scaleWidth);
            Canvas.SetTop(points[17], curFace.faceLandmarks.noseRightAlarOutTip.y * scaleHeight);
            Canvas.SetLeft(points[18], curFace.faceLandmarks.noseRightAlarTop.x * scaleWidth);
            Canvas.SetTop(points[18], curFace.faceLandmarks.noseRightAlarTop.y * scaleHeight);
            Canvas.SetLeft(points[19], curFace.faceLandmarks.noseRootLeft.x * scaleWidth);
            Canvas.SetTop(points[19], curFace.faceLandmarks.noseRootLeft.y * scaleHeight);
            Canvas.SetLeft(points[20], curFace.faceLandmarks.noseRootRight.x * scaleWidth);
            Canvas.SetTop(points[20], curFace.faceLandmarks.noseRootRight.y * scaleHeight);
            Canvas.SetLeft(points[21], curFace.faceLandmarks.noseTip.x * scaleWidth);
            Canvas.SetTop(points[21], curFace.faceLandmarks.noseTip.y * scaleHeight);
            Canvas.SetLeft(points[22], curFace.faceLandmarks.underLipBottom.x * scaleWidth);
            Canvas.SetTop(points[22], curFace.faceLandmarks.underLipBottom.y * scaleHeight);
            Canvas.SetLeft(points[23], curFace.faceLandmarks.underLipTop.x * scaleWidth);
            Canvas.SetTop(points[23], curFace.faceLandmarks.underLipTop.y * scaleHeight);
            Canvas.SetLeft(points[24], curFace.faceLandmarks.upperLipBottom.x * scaleWidth);
            Canvas.SetTop(points[24], curFace.faceLandmarks.upperLipBottom.y * scaleHeight);
            Canvas.SetLeft(points[25], curFace.faceLandmarks.upperLipTop.x * scaleWidth);
            Canvas.SetTop(points[25], curFace.faceLandmarks.upperLipTop.y * scaleHeight);
            Canvas.SetLeft(points[26], curFace.faceLandmarks.pupilLeft.x * scaleWidth);
            Canvas.SetTop(points[26], curFace.faceLandmarks.pupilLeft.y * scaleHeight);
            Canvas.SetLeft(points[27], curFace.faceLandmarks.pupilRight.x * scaleWidth);
            Canvas.SetTop(points[27], curFace.faceLandmarks.pupilRight.y * scaleHeight);

            lines[0].X1 = curFace.faceLandmarks.eyebrowLeftInner.x * scaleWidth;
            lines[0].Y1 = curFace.faceLandmarks.eyebrowLeftInner.y * scaleHeight;
            lines[0].X2 = curFace.faceLandmarks.eyebrowLeftOuter.x * scaleWidth;
            lines[0].Y2 = curFace.faceLandmarks.eyebrowLeftOuter.y * scaleHeight;

            lines[1].X1 = curFace.faceLandmarks.eyebrowRightInner.x * scaleWidth;
            lines[1].Y1 = curFace.faceLandmarks.eyebrowRightInner.y * scaleHeight;
            lines[1].X2 = curFace.faceLandmarks.eyebrowRightOuter.x * scaleWidth;
            lines[1].Y2 = curFace.faceLandmarks.eyebrowRightOuter.y * scaleHeight;

            lines[2].X1 = curFace.faceLandmarks.eyebrowLeftInner.x * scaleWidth;
            lines[2].Y1 = curFace.faceLandmarks.eyebrowLeftInner.y * scaleHeight;
            lines[2].X2 = curFace.faceLandmarks.eyebrowRightInner.x * scaleWidth;
            lines[2].Y2 = curFace.faceLandmarks.eyebrowRightInner.y * scaleHeight;

            lines[3].X1 = curFace.faceLandmarks.mouthLeft.x * scaleWidth;
            lines[3].Y1 = curFace.faceLandmarks.mouthLeft.y * scaleHeight;
            lines[3].X2 = curFace.faceLandmarks.upperLipTop.x * scaleWidth;
            lines[3].Y2 = curFace.faceLandmarks.upperLipTop.y * scaleHeight;

            lines[4].X1 = curFace.faceLandmarks.upperLipTop.x * scaleWidth;
            lines[4].Y1 = curFace.faceLandmarks.upperLipTop.y * scaleHeight;
            lines[4].X2 = curFace.faceLandmarks.mouthRight.x * scaleWidth;
            lines[4].Y2 = curFace.faceLandmarks.mouthRight.y * scaleHeight;

            lines[5].X1 = curFace.faceLandmarks.mouthLeft.x * scaleWidth;
            lines[5].Y1 = curFace.faceLandmarks.mouthLeft.y * scaleHeight;
            lines[5].X2 = curFace.faceLandmarks.underLipBottom.x * scaleWidth;
            lines[5].Y2 = curFace.faceLandmarks.underLipBottom.y * scaleHeight;

            lines[6].X1 = curFace.faceLandmarks.underLipBottom.x * scaleWidth;
            lines[6].Y1 = curFace.faceLandmarks.underLipBottom.y * scaleHeight;
            lines[6].X2 = curFace.faceLandmarks.mouthRight.x * scaleWidth;
            lines[6].Y2 = curFace.faceLandmarks.mouthRight.y * scaleHeight;

            lines[7].X1 = curFace.faceLandmarks.underLipBottom.x * scaleWidth;
            lines[7].Y1 = curFace.faceLandmarks.underLipBottom.y * scaleHeight;
            lines[7].X2 = curFace.faceLandmarks.mouthRight.x * scaleWidth;
            lines[7].Y2 = curFace.faceLandmarks.mouthRight.y * scaleHeight;

            lines[8].X1 = curFace.faceLandmarks.eyebrowLeftOuter.x * scaleWidth;
            lines[8].Y1 = curFace.faceLandmarks.eyebrowLeftOuter.y * scaleHeight;
            lines[8].X2 = curFace.faceLandmarks.mouthLeft.x * scaleWidth;
            lines[8].Y2 = curFace.faceLandmarks.mouthLeft.y * scaleHeight;

            lines[9].X1 = curFace.faceLandmarks.eyebrowRightOuter.x * scaleWidth;
            lines[9].Y1 = curFace.faceLandmarks.eyebrowRightOuter.y * scaleHeight;
            lines[9].X2 = curFace.faceLandmarks.mouthRight.x * scaleWidth;
            lines[9].Y2 = curFace.faceLandmarks.mouthRight.y * scaleHeight;

            lines[10].X1 = curFace.faceLandmarks.eyebrowRightOuter.x * scaleWidth;
            lines[10].Y1 = curFace.faceLandmarks.eyebrowRightOuter.y * scaleHeight;
            lines[10].X2 = curFace.faceLandmarks.mouthRight.x * scaleWidth;
            lines[10].Y2 = curFace.faceLandmarks.mouthRight.y * scaleHeight;

            lines[11].X1 = curFace.faceLandmarks.eyebrowLeftInner.x * scaleWidth;
            lines[11].Y1 = curFace.faceLandmarks.eyebrowLeftInner.y * scaleHeight;
            lines[11].X2 = curFace.faceLandmarks.noseRootLeft.x * scaleWidth;
            lines[11].Y2 = curFace.faceLandmarks.noseRootLeft.y * scaleHeight;
            lines[12].X1 = curFace.faceLandmarks.eyebrowRightInner.x * scaleWidth;
            lines[12].Y1 = curFace.faceLandmarks.eyebrowRightInner.y * scaleHeight;
            lines[12].X2 = curFace.faceLandmarks.noseRootRight.x * scaleWidth;
            lines[12].Y2 = curFace.faceLandmarks.noseRootRight.y * scaleHeight;
            lines[13].X1 = curFace.faceLandmarks.noseLeftAlarTop.x * scaleWidth;
            lines[13].Y1 = curFace.faceLandmarks.noseLeftAlarTop.y * scaleHeight;
            lines[13].X2 = curFace.faceLandmarks.noseRootLeft.x * scaleWidth;
            lines[13].Y2 = curFace.faceLandmarks.noseRootLeft.y * scaleHeight;
            lines[14].X1 = curFace.faceLandmarks.noseLeftAlarTop.x * scaleWidth;
            lines[14].Y1 = curFace.faceLandmarks.noseLeftAlarTop.y * scaleHeight;
            lines[14].X2 = curFace.faceLandmarks.noseLeftAlarOutTip.x * scaleWidth;
            lines[14].Y2 = curFace.faceLandmarks.noseLeftAlarOutTip.y * scaleHeight;
            lines[15].X1 = curFace.faceLandmarks.noseRightAlarTop.x * scaleWidth;
            lines[15].Y1 = curFace.faceLandmarks.noseRightAlarTop.y * scaleHeight;
            lines[15].X2 = curFace.faceLandmarks.noseRightAlarOutTip.x * scaleWidth;
            lines[15].Y2 = curFace.faceLandmarks.noseRightAlarOutTip.y * scaleHeight;
            lines[16].X1 = curFace.faceLandmarks.noseRightAlarTop.x * scaleWidth;
            lines[16].Y1 = curFace.faceLandmarks.noseRightAlarTop.y * scaleHeight;
            lines[16].X2 = curFace.faceLandmarks.noseRootRight.x * scaleWidth;
            lines[16].Y2 = curFace.faceLandmarks.noseRootRight.y * scaleHeight;
            lines[17].X1 = curFace.faceLandmarks.mouthLeft.x * scaleWidth;
            lines[17].Y1 = curFace.faceLandmarks.mouthLeft.y * scaleHeight;
            lines[17].X2 = curFace.faceLandmarks.noseLeftAlarOutTip.x * scaleWidth;
            lines[17].Y2 = curFace.faceLandmarks.noseLeftAlarOutTip.y * scaleHeight;
            lines[18].X1 = curFace.faceLandmarks.mouthRight.x * scaleWidth;
            lines[18].Y1 = curFace.faceLandmarks.mouthRight.y * scaleHeight;
            lines[18].X2 = curFace.faceLandmarks.noseRightAlarOutTip.x * scaleWidth;
            lines[18].Y2 = curFace.faceLandmarks.noseRightAlarOutTip.y * scaleHeight;
            lines[19].X1 = curFace.faceLandmarks.noseRootLeft.x * scaleWidth;
            lines[19].Y1 = curFace.faceLandmarks.noseRootLeft.y * scaleHeight;
            lines[19].X2 = curFace.faceLandmarks.noseTip.x * scaleWidth;
            lines[19].Y2 = curFace.faceLandmarks.noseTip.y * scaleHeight;
            lines[20].X1 = curFace.faceLandmarks.noseTip.x * scaleWidth;
            lines[20].Y1 = curFace.faceLandmarks.noseTip.y * scaleHeight;
            lines[20].X2 = curFace.faceLandmarks.noseRootRight.x * scaleWidth;
            lines[20].Y2 = curFace.faceLandmarks.noseRootRight.y * scaleHeight;
            lines[21].X1 = curFace.faceLandmarks.noseRootLeft.x * scaleWidth;
            lines[21].Y1 = curFace.faceLandmarks.noseRootLeft.y * scaleHeight;
            lines[21].X2 = curFace.faceLandmarks.noseRootRight.x * scaleWidth;
            lines[21].Y2 = curFace.faceLandmarks.noseRootRight.y * scaleHeight;
            lines[22].X1 = curFace.faceLandmarks.noseLeftAlarTop.x * scaleWidth;
            lines[22].Y1 = curFace.faceLandmarks.noseLeftAlarTop.y * scaleHeight;
            lines[22].X2 = curFace.faceLandmarks.noseTip.x * scaleWidth;
            lines[22].Y2 = curFace.faceLandmarks.noseTip.y * scaleHeight;
            lines[23].X1 = curFace.faceLandmarks.noseRightAlarTop.x * scaleWidth;
            lines[23].Y1 = curFace.faceLandmarks.noseRightAlarTop.y * scaleHeight;
            lines[23].X2 = curFace.faceLandmarks.noseTip.x * scaleWidth;
            lines[23].Y2 = curFace.faceLandmarks.noseTip.y * scaleHeight;
            lines[24].X1 = curFace.faceLandmarks.noseLeftAlarOutTip.x * scaleWidth;
            lines[24].Y1 = curFace.faceLandmarks.noseLeftAlarOutTip.y * scaleHeight;
            lines[24].X2 = curFace.faceLandmarks.noseTip.x * scaleWidth;
            lines[24].Y2 = curFace.faceLandmarks.noseTip.y * scaleHeight;
            lines[25].X1 = curFace.faceLandmarks.noseRightAlarOutTip.x * scaleWidth;
            lines[25].Y1 = curFace.faceLandmarks.noseRightAlarOutTip.y * scaleHeight;
            lines[25].X2 = curFace.faceLandmarks.noseTip.x * scaleWidth;
            lines[25].Y2 = curFace.faceLandmarks.noseTip.y * scaleHeight;
            lines[26].X1 = curFace.faceLandmarks.noseRightAlarOutTip.x * scaleWidth;
            lines[26].Y1 = curFace.faceLandmarks.noseRightAlarOutTip.y * scaleHeight;
            lines[26].X2 = curFace.faceLandmarks.upperLipTop.x * scaleWidth;
            lines[26].Y2 = curFace.faceLandmarks.upperLipTop.y * scaleHeight;
            lines[27].X1 = curFace.faceLandmarks.noseLeftAlarOutTip.x * scaleWidth;
            lines[27].Y1 = curFace.faceLandmarks.noseLeftAlarOutTip.y * scaleHeight;
            lines[27].X2 = curFace.faceLandmarks.upperLipTop.x * scaleWidth;
            lines[27].Y2 = curFace.faceLandmarks.upperLipTop.y * scaleHeight;

            this.First.Children.Add(faceBox);
            for (int i = 0; i < 28; i++)
            {
                this.First.Children.Add(points[i]);
            }
            for (int i = 0; i < 30; i++)
            {
                this.First.Children.Add(lines[i]);
            }
        }
        private void clearFirst()
        {
            while (First.Children.Count > 0)
                First.Children.RemoveAt(First.Children.Count - 1);
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
            this.First.Background = brush;
            WaitLoading.IsActive = false;
        }
        private async void File_Click(object sender, RoutedEventArgs e)
        {
            //清理屏幕
            clearFirst();
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

        private async void cameraButton_Click(object sender, RoutedEventArgs e)
        {
            //清理屏幕
            clearFirst();

            //访问摄像头
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);
            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            //显示图片
            showImage(photo);
        }
    }
}

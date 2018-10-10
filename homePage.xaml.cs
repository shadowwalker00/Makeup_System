using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Shapes;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace BeautyFace
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>

    public sealed partial class homePage : Page
    {
        private GaussianBlurEffect blurEffect;
        private GrayscaleEffect effect;
        private double blurAmount;

        public  homePage()
        {
            this.InitializeComponent();
            

        }
        private async void myCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            effect = new GrayscaleEffect();
            CanvasBitmap mtg = await CanvasBitmap.LoadAsync(sender, "Assets/Faces/baby.jpg");
            effect.Source = mtg;
            blurEffect = new GaussianBlurEffect();
            blurEffect.BlurAmount = 0;
            blurEffect.Source = effect;
            args.DrawingSession.DrawImage(blurEffect);
        }

        private void Tab_Tapped(object sender, TappedRoutedEventArgs e)
        {
            List<RelativePanel> tabList = new List<RelativePanel> { Tab1, Tab2, Tab3, Tab4 };
            int oldIndex = ManipulatePanel.SelectedIndex;
            int index = tabList.IndexOf(sender as RelativePanel);
            ManipulatePanel.SelectedIndex = index;

            //修改之前点击的relatiePanel
            (tabList[oldIndex].Children[0] as TextBlock).Foreground = new SolidColorBrush(Colors.Gray);
            (tabList[oldIndex].Children[1] as Rectangle).Fill = new SolidColorBrush(Colors.Gray);

            //修改当前点击的relativePanel
            (tabList[index].Children[0] as TextBlock).Foreground = new SolidColorBrush(Colors.DarkOrange);
            (tabList[index].Children[1] as Rectangle).Fill = new SolidColorBrush(Colors.DarkOrange);
        }
        private void Lumi_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            blurAmount = Lumi.Value;
        }
    }
}

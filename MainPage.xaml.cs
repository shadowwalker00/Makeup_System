using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BeautyFace;
using Windows.Storage.Pickers;
using Windows.UI;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace BeautyFace
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private StorageFile transFile;
        public MainPage()
        {
            this.InitializeComponent();
            titleBlock.Text = "识别";
            MyFrame.Navigate(typeof(RecogPage));
        } 
        private void HumButton_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        private void myListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Recog.IsSelected==true)
            {
                titleBlock.Text = "识别";
                rec1.Fill = new SolidColorBrush(Colors.Orange);
                rec2.Fill = new SolidColorBrush(Colors.Gray);
                rec3.Fill = new SolidColorBrush(Colors.Gray);
                MyFrame.Navigate(typeof(RecogPage));
            }
            if (Assembly.IsSelected == true)
            {
                titleBlock.Text = "美颜";
                rec1.Fill = new SolidColorBrush(Colors.Gray);
                rec2.Fill = new SolidColorBrush(Colors.Orange);
                rec3.Fill = new SolidColorBrush(Colors.Gray);
                MyFrame.Navigate(typeof(Form1));
            }
            else if (Emoji.IsSelected == true)
            {
                titleBlock.Text = "表情包";
                rec1.Fill = new SolidColorBrush(Colors.Gray);
                rec2.Fill = new SolidColorBrush(Colors.Gray);
                rec3.Fill = new SolidColorBrush(Colors.Orange);
                MyFrame.Navigate(typeof(EmojiPage));
            }
        }

        private async void botListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(settingItem.IsSelected)
            {
                mySplitView.IsPaneOpen = false;
                settingDialog st = new settingDialog();
                await st.ShowAsync();
            }
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cameraItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void gallerayItem_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            try
            {
                var image_file = await picker.PickSingleFileAsync();
                (App.Current as App).curImage = image_file;
            }
            catch
            {

            }
        }
    }
}

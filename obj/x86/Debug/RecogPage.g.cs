﻿#pragma checksum "C:\Users\steven\Documents\Visual Studio 2015\Projects\BeautyFace\BeautyFace\RecogPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8EA625B56A2E6E79C4B35B7C55E513F3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BeautyFace
{
    partial class RecogPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.VisualStateGroup = (global::Windows.UI.Xaml.VisualStateGroup)(target);
                }
                break;
            case 2:
                {
                    this.Wide = (global::Windows.UI.Xaml.VisualState)(target);
                }
                break;
            case 3:
                {
                    this.Narrow = (global::Windows.UI.Xaml.VisualState)(target);
                }
                break;
            case 4:
                {
                    this.First = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 5:
                {
                    this.WaitLoading = (global::Windows.UI.Xaml.Controls.ProgressRing)(target);
                }
                break;
            case 6:
                {
                    this.Second = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 7:
                {
                    this.pieChart = (global::WinRTXamlToolkit.Controls.DataVisualization.Charting.Chart)(target);
                }
                break;
            case 8:
                {
                    this.cameraButton = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    #line 107 "..\..\..\RecogPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)this.cameraButton).Click += this.cameraButton_Click;
                    #line default
                }
                break;
            case 9:
                {
                    this.Filebut = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    #line 110 "..\..\..\RecogPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)this.Filebut).Click += this.File_Click;
                    #line default
                }
                break;
            case 10:
                {
                    this.recogbut = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    #line 113 "..\..\..\RecogPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)this.recogbut).Click += this.recogBut_Click;
                    #line default
                }
                break;
            case 11:
                {
                    this.gender_text = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 12:
                {
                    this.age_text = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13:
                {
                    this.ID_text = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}


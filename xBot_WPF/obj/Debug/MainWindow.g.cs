﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DF7A6F8DF791BF9E699EF0FCD655CCCA9D772DDDDE3E56EC43D66940229E13A6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using xBot_WPF;


namespace xBot_WPF {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal xBot_WPF.MainWindow xBot;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RichTextBox logViewRTB;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label miniMizeLBL;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image upperLogo;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridMenu;
        
        #line default
        #line hidden
        
        
        #line 118 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCloseMenu;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOpenMenu;
        
        #line default
        #line hidden
        
        
        #line 162 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button startBotBTN;
        
        #line default
        #line hidden
        
        
        #line 163 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image statIMG;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/xBot_WPF;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.xBot = ((xBot_WPF.MainWindow)(target));
            
            #line 10 "..\..\MainWindow.xaml"
            this.xBot.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            
            #line 10 "..\..\MainWindow.xaml"
            this.xBot.Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.logViewRTB = ((System.Windows.Controls.RichTextBox)(target));
            return;
            case 3:
            
            #line 106 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.aboutBTN_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 108 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.closeBTN_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.miniMizeLBL = ((System.Windows.Controls.Label)(target));
            
            #line 112 "..\..\MainWindow.xaml"
            this.miniMizeLBL.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.miniMizeLBL_PreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.upperLogo = ((System.Windows.Controls.Image)(target));
            return;
            case 7:
            this.GridMenu = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.btnCloseMenu = ((System.Windows.Controls.Button)(target));
            
            #line 118 "..\..\MainWindow.xaml"
            this.btnCloseMenu.Click += new System.Windows.RoutedEventHandler(this.btnCloseMenu_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnOpenMenu = ((System.Windows.Controls.Button)(target));
            
            #line 121 "..\..\MainWindow.xaml"
            this.btnOpenMenu.Click += new System.Windows.RoutedEventHandler(this.btnOpenMenu_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 125 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.ListViewItem)(target)).PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ListViewItem_PreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 133 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.ListViewItem)(target)).PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ListViewItem_PreviewMouseDownCMD);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 140 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.ListViewItem)(target)).PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ListViewItem_PreviewMouseDownBAD);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 147 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.ListViewItem)(target)).PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ListViewItem_PreviewMouseDownBot);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 154 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.ListViewItem)(target)).PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ListViewItem_PreviewMouseDownYT);
            
            #line default
            #line hidden
            return;
            case 15:
            this.startBotBTN = ((System.Windows.Controls.Button)(target));
            
            #line 162 "..\..\MainWindow.xaml"
            this.startBotBTN.Click += new System.Windows.RoutedEventHandler(this.startBTN_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.statIMG = ((System.Windows.Controls.Image)(target));
            
            #line 163 "..\..\MainWindow.xaml"
            this.statIMG.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.statIMG_PreviewMouseDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


﻿#pragma checksum "..\..\youtube.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "A48CF217EE5255B39273F587E14C4F920012BF50F0F581C212FE53D222214BD1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Gu.Wpf.Adorners;
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
    /// youtube
    /// </summary>
    public partial class youtube : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\youtube.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal xBot_WPF.youtube YtPlayer;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\youtube.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label closeLBL;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\youtube.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label miniMizeLBL;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\youtube.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WebBrowser ytBrowser;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\youtube.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button playBTN;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\youtube.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox youtTubeLink;
        
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
            System.Uri resourceLocater = new System.Uri("/xBot_WPF;component/youtube.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\youtube.xaml"
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
            this.YtPlayer = ((xBot_WPF.youtube)(target));
            
            #line 10 "..\..\youtube.xaml"
            this.YtPlayer.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            
            #line 10 "..\..\youtube.xaml"
            this.YtPlayer.Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.closeLBL = ((System.Windows.Controls.Label)(target));
            
            #line 18 "..\..\youtube.xaml"
            this.closeLBL.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.closeLBL_PreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.miniMizeLBL = ((System.Windows.Controls.Label)(target));
            
            #line 19 "..\..\youtube.xaml"
            this.miniMizeLBL.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.miniMizeLBL_PreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ytBrowser = ((System.Windows.Controls.WebBrowser)(target));
            return;
            case 5:
            this.playBTN = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\youtube.xaml"
            this.playBTN.Click += new System.Windows.RoutedEventHandler(this.playBTN_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.youtTubeLink = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}


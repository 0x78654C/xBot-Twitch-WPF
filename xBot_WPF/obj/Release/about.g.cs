﻿#pragma checksum "..\..\about.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EB43C1550470F611D2E74E39B1862ED875F7D7257FFDCC2B4ADDA7CEE6B53114"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    /// about
    /// </summary>
    public partial class about : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\about.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label miniMizeLBL;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\about.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button closeBTNMSG;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\about.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label assamblyNameLBL;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\about.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label versionNameLBL;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\about.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label copyRightLBL;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\about.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox descriptionTXT;
        
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
            System.Uri resourceLocater = new System.Uri("/xBot;component/about.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\about.xaml"
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
            
            #line 8 "..\..\about.xaml"
            ((xBot_WPF.about)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.miniMizeLBL = ((System.Windows.Controls.Label)(target));
            
            #line 13 "..\..\about.xaml"
            this.miniMizeLBL.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.miniMizeLBL_PreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.closeBTNMSG = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\about.xaml"
            this.closeBTNMSG.Click += new System.Windows.RoutedEventHandler(this.closeBTNMSG_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.assamblyNameLBL = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.versionNameLBL = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.copyRightLBL = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.descriptionTXT = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}


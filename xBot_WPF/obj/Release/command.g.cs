﻿#pragma checksum "..\..\command.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8E8C5E603B443A9318D9B373811BA2F3C7646A811F8E5EA89E2302B6A6203461"
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
    /// command
    /// </summary>
    public partial class command : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 44 "..\..\command.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label closeLBL;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\command.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label miniMizeLBL;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\command.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox nameTXT;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\command.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox contentTXT;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\command.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button adderBTN;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\command.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button removerBTN;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\command.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox commandList;
        
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
            System.Uri resourceLocater = new System.Uri("/xBot;component/command.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\command.xaml"
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
            
            #line 8 "..\..\command.xaml"
            ((xBot_WPF.command)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.closeLBL = ((System.Windows.Controls.Label)(target));
            
            #line 44 "..\..\command.xaml"
            this.closeLBL.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.closeLBL_PreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.miniMizeLBL = ((System.Windows.Controls.Label)(target));
            
            #line 45 "..\..\command.xaml"
            this.miniMizeLBL.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.miniMizeLBL_PreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.nameTXT = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.contentTXT = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.adderBTN = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\command.xaml"
            this.adderBTN.Click += new System.Windows.RoutedEventHandler(this.adderBTN_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.removerBTN = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\command.xaml"
            this.removerBTN.Click += new System.Windows.RoutedEventHandler(this.removerBTN_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.commandList = ((System.Windows.Controls.ListBox)(target));
            
            #line 51 "..\..\command.xaml"
            this.commandList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.commandList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


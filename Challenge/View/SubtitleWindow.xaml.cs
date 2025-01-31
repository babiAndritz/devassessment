﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SubtitleTimeshift;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Challenge.ViewModel;
using Path = System.IO.Path;
using System.Diagnostics;

namespace Challenge.View
{
    /// <summary>
    /// Interaction logic for SubtitleWindow.xaml
    /// </summary>
    public partial class SubtitleWindow : Window
    {
        private readonly SubtitleVM viewModel;

        public SubtitleWindow()
        {
            InitializeComponent();
            viewModel = new SubtitleVM();
            DataContext = viewModel;
        }

    }
}

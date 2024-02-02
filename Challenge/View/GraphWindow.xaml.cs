using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Challenge.ViewModel;
using Graph;
using static System.Windows.Forms.LinkLabel;

namespace Challenge.View
{
    public partial class GraphWindow : Window
    {

        private GraphVM vm;

        public GraphWindow()
        {
            InitializeComponent();

            vm = new GraphVM();
            DataContext = vm;
        }
    }
}

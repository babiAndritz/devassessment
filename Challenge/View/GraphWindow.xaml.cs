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

        // --- Create --- //
        private void graphLinkOriginComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrigin = ((ComboBoxItem)graphLinkOriginComboBox.SelectedItem)?.Content?.ToString();
            foreach (ComboBoxItem item in graphLinkTargetComboBox.Items)
            {
                if (item.Content.ToString() == selectedOrigin)
                {
                    item.IsEnabled = false;
                }
                else
                {
                    item.IsEnabled = true;
                }
            }
        }

        private void graphLinkTargetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTarget = ((ComboBoxItem)graphLinkTargetComboBox.SelectedItem)?.Content?.ToString();

            foreach (ComboBoxItem item in graphLinkOriginComboBox.Items)
            {
                if (item.Content.ToString() == selectedTarget)
                {
                    item.IsEnabled = false;
                }
                else
                {
                    item.IsEnabled = true;
                }
            }
        }

        private void LinkButton_Click(object sender, RoutedEventArgs e)
        {
            bool linkExists = vm.Links.Any(link => link.Source == graphLinkOriginComboBox.Text && link.Target == graphLinkTargetComboBox.Text);

            if (graphLinkOriginComboBox.Text == string.Empty || graphLinkTargetComboBox.Text == string.Empty)
            {
                MessageBox.Show("Please insert both origin and target to create a link", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {



                if (!linkExists)
                {
                    vm.Links.Add(new Link<string>(graphLinkOriginComboBox.Text, graphLinkTargetComboBox.Text));
                    UpdateGraphMessage();
                }

                vm.Graph = new Graph<string>(vm.Links);

                graphLinkOriginComboBox.Text = string.Empty;
                graphLinkTargetComboBox.Text = string.Empty;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Links.Count > 0)
            {
                vm.Links.Remove(vm.Links.Last());
                UpdateGraphMessage();
            }
        }


        // --- Random --- //
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeGraph();
            randomButton.IsEnabled = false;
            createButton.IsEnabled = false;
        }

        private void InitializeGraph()
        {
            Random random = new Random();

            for (int i = 0; i < vm.Vertices.Length; i++)
            {
                var source = vm.Vertices[i];
                var target = vm.Vertices[random.Next(vm.Vertices.Length - 1)];

                if (target == source)
                {
                    target = vm.Vertices.Last();
                }

                vm.Links.Add(new Link<string>(source, target));

            }

            vm.Graph = new Graph<string>(vm.Links);
            UpdateGraphMessage();
        }

        // - Message - //
        private void UpdateGraphMessage()
        {
            string message = "Links:\n";

            foreach (var link in vm.Links)
            {
                message += $"\n{link}\n";
            }

            verticesText.Text = message;
        }


        // --- DFS --- //
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if(vm.Links != null && vm.Graph != null && vm.Source != null && vm.Target != null)
            {
                ProcessDFS();
            } 
            else
            {
                MessageBox.Show("Please create a graph and select the origin and target first, then start the DFS algorithm.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ProcessDFS()
        {
            var paths = vm.Graph.RoutesBetween(vm.Source, vm.Target);

            var list = paths.ToEnumerable().ToArray();

            string message = "";

            if (list.Count() < 1)
            {
                message = "There isn't an existing path.";
            }
            else
            {
                message = "Paths:\n";

                for (int i = 0; i < list.Count(); i++)
                {
                    var formattedPath = string.Join(" - ", list[i]);
                    message += $"\nPath {i + 1}: {formattedPath}\n";
                }
            }

            resultText.Text = message;
        }


        // - Clear - //
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (resultText != null)
            {
                resultText.Text = string.Empty; 
            }
            if (verticesText != null)
            {
                verticesText.Text = string.Empty;
            } 
            if (vm.Links != null)
            {
                vm.Links.Clear();
            } 
            if (vm.Graph != null)
            {
                vm.Graph = null;
            }
            clearButton.IsEnabled = true;
            randomButton.IsEnabled = true;
            startButton.IsEnabled = true;
            createButton.IsEnabled = true;
            CreatestackPanel.Visibility = Visibility.Hidden;
        }


        // --- Path --- //
        private void graphOriginComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var origin = ((ComboBoxItem)graphOriginComboBox.SelectedItem)?.Content?.ToString();
            foreach (ComboBoxItem item in graphTargetComboBox.Items)
            {
                if (item.Content.ToString() == origin)
                {
                    item.IsEnabled = false;
                }
                else
                {
                    item.IsEnabled = true;
                }
            }
        }

        private void graphTargetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var target = ((ComboBoxItem)graphTargetComboBox.SelectedItem)?.Content?.ToString();

            foreach (ComboBoxItem item in graphOriginComboBox.Items)
            {
                if (item.Content.ToString() == target)
                {
                    item.IsEnabled = false;
                }
                else
                {
                    item.IsEnabled = true;
                }
            }
        }

        private void PathButton_Click(object sender, RoutedEventArgs e)
        {
            vm.Source = graphOriginComboBox.Text;
            vm.Target = graphTargetComboBox.Text;

            PathText.Text = $"Choosen Path:  {vm.Source} -> {vm.Target}";

            graphOriginComboBox.Text = string.Empty;
            graphTargetComboBox.Text = string.Empty;
        }

    }
}

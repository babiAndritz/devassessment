using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Challenge1.ViewModel;
using Graph;
using static System.Windows.Forms.LinkLabel;

namespace Challenge1.View
{
    public partial class GraphWindow : Window
    {

        private GraphVM vm;
        //private Graph<string> graph;
        private readonly string[] vertices = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };
        private readonly List<ILink<string>> links = new List<ILink<string>>();
        private string selectedOrigin, selectedTarget;

        public GraphWindow()
        {
            InitializeComponent();

            vm = new GraphVM();
            DataContext = vm;

            links.Clear();
            vm.Graph = null;
            CreatestackPanel.Visibility = Visibility.Hidden;
        }

        // --- Create --- //
        private void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            CreatestackPanel.Visibility = Visibility.Visible;
            randomButton.IsEnabled = false;
            createButton.IsEnabled = false;
        }

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
            bool linkExists = links.Any(link => link.Source == graphLinkOriginComboBox.Text && link.Target == graphLinkTargetComboBox.Text);

            if (graphLinkOriginComboBox.Text == string.Empty || graphLinkTargetComboBox.Text == string.Empty)
            {
                MessageBox.Show("Please insert both origin and target to create a link", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {



                if (!linkExists)
                {
                    links.Add(new Link<string>(graphLinkOriginComboBox.Text, graphLinkTargetComboBox.Text));
                    UpdateGraphMessage();
                }

                vm.Graph = new Graph<string>(links);

                graphLinkOriginComboBox.Text = string.Empty;
                graphLinkTargetComboBox.Text = string.Empty;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (links.Count > 0)
            {
                links.Remove(links.Last());
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

            for (int i = 0; i < vertices.Length; i++)
            {
                var source = vertices[i];
                var target = vertices[random.Next(vertices.Length - 1)];

                if (target == source)
                {
                    target = vertices.Last();
                }

                links.Add(new Link<string>(source, target));

            }

            vm.Graph = new Graph<string>(links);
            UpdateGraphMessage();
        }

        // - Message - //
        private void UpdateGraphMessage()
        {
            string message = "Links:\n";

            foreach (var link in links)
            {
                message += $"\n{link}\n";
            }

            verticesText.Text = message;
        }


        // --- DFS --- //
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if(links != null && vm.Graph != null && selectedOrigin != null && selectedTarget != null)
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
            var paths = vm.Graph.RoutesBetween(selectedOrigin, selectedTarget);

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
            if (links != null)
            {
                links.Clear();
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
            selectedOrigin = graphOriginComboBox.Text;
            selectedTarget = graphTargetComboBox.Text;

            PathText.Text = $"Choosen Path:  {selectedOrigin} -> {selectedTarget}";

            graphOriginComboBox.Text = string.Empty;
            graphTargetComboBox.Text = string.Empty;
        }

    }
}

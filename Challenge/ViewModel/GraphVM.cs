using Graph;
using System;
using Prism.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace Challenge.ViewModel
{
    public class GraphItem
    {
        public string Name { get; set; }
    }

    public class GraphVM : INotifyPropertyChanged
    {
        private Graph<string> graph;
        public Graph<string>? Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                OnPropertyChanged("Graph");
            }
        }

        private GraphItem source;
        public GraphItem Source
        {
            get { return source; }
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        private GraphItem target;
        public GraphItem Target
        {
            get { return target; }
            set
            {
                target = value;
                OnPropertyChanged("Target");
            }
        }

        private string[] vertices;
        public string[] Vertices
        {
            get { return vertices; }
            set
            {
                vertices = value;
                OnPropertyChanged("Vertices");
            }
        }

        private List<ILink<string>> links;

        public List<ILink<string>> Links
        {
            get { return links; }
            set
            {
                links = value;
                OnPropertyChanged("Links");
            }
        }

        public ICommand CreateGraphCommand { get; set; }
        public ICommand OriginItemSelectedCommand { get; set; }
        public ICommand TargetItemSelectedCommand { get; set; }
        public ICommand SelectedPathCommand { get; set; }

        private Visibility stackPannel;
        public Visibility StackPannel
        {
            get { return stackPannel; }
            set
            {
                stackPannel = value;
                OnPropertyChanged("StackPannel");
            }
        }

        private bool randomEnabled;
        public bool RandomEnabled
        {
            get { return randomEnabled; }
            set
            {
                randomEnabled = value;
                OnPropertyChanged("RandomEnabled");
            }
        }

        private bool createEnabled;
        public bool CreateEnabled
        {
            get { return createEnabled; }
            set
            {
                createEnabled = value;
                OnPropertyChanged("CreateEnabled");
            }
        }

        private string originSelectedItem;
        public string OriginSelectedItem
        {
            get { return originSelectedItem; }
            set
            {
                originSelectedItem = value;
                OnPropertyChanged("OriginSelectedItem");
            }
        }

        private string targetSelectedItem;
        public string TargetSelectedItem
        {
            get { return targetSelectedItem; }
            set
            {
                targetSelectedItem = value;
                OnPropertyChanged("TargetSelectedItem");
            }
        }

        private string pathText;
        public string PathText
        {
            get { return pathText; }
            set
            {
                pathText = value;
                OnPropertyChanged("PathText");
            }
        }

        public ObservableCollection<GraphItem> SourceItemsSource { get; set; }
        public ObservableCollection<GraphItem> TargetItemsSource { get; set; }

        public GraphVM()
        {
            InitializeVertices();
            InitializeLinks();

            Links.Clear();
            Graph = null;
            StackPannel = Visibility.Hidden;
            RandomEnabled = true;
            CreateEnabled = true;

            // --- Create --- //
            CreateGraphCommand = new DelegateCommand(() =>
            {
                StackPannel = Visibility.Visible;
                RandomEnabled = false;
                CreateEnabled = false;
            });

            // --- ComboBox --- //
            SourceItemsSource = new ObservableCollection<GraphItem>
            {
            new GraphItem { Name = "A" },
            new GraphItem { Name = "B" },
            new GraphItem { Name = "C" },
            new GraphItem { Name = "D" },
            new GraphItem { Name = "E" },
            new GraphItem { Name = "F" },
            new GraphItem { Name = "G" },
            new GraphItem { Name = "H" },
            new GraphItem { Name = "I" },
            };

            TargetItemsSource = new ObservableCollection<GraphItem>
            {
            new GraphItem { Name = "A" },
            new GraphItem { Name = "B" },
            new GraphItem { Name = "C" },
            new GraphItem { Name = "D" },
            new GraphItem { Name = "E" },
            new GraphItem { Name = "F" },
            new GraphItem { Name = "G" },
            new GraphItem { Name = "H" },
            new GraphItem { Name = "I" },
            };

            // --- Path --- //
            OriginItemSelectedCommand = new DelegateCommand(() =>
            {
                var selectedOrigin = Source?.Name;

                if (selectedOrigin != null)
                {
                    if (selectedOrigin == Target?.Name)
                    {
                        MessageBox.Show("The selected target cannot be the same as the origin. Please select another source.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        Source = null;
                    }
                }
            });

            TargetItemSelectedCommand = new DelegateCommand(() =>
            {
                var selectedTarget = Target?.Name;

                if (selectedTarget != null)
                {
                    if (selectedTarget == Source?.Name)
                    {
                        MessageBox.Show("The selected origin cannot be the same as the target. Please select another target.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        Target = null;
                    }
                }
            });

            SelectedPathCommand = new DelegateCommand(() =>
            {
                PathText = $"Choosen Path:  {Source.Name.ToString()} -> {Target.Name.ToString()}";
            });


        }

        private void InitializeLinks()
        {
            Links = new List<ILink<string>>();
        }

        private void InitializeVertices()
        {
            Vertices = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
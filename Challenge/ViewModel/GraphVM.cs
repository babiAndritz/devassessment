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
using System.Reactive.Linq;

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
        public GraphItem? Source
        {
            get { return source; }
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        private GraphItem target;
        public GraphItem? Target
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

        private GraphItem originSelectedItem;
        public GraphItem OriginSelectedItem
        {
            get { return originSelectedItem; }
            set
            {
                originSelectedItem = value;
                OnPropertyChanged("OriginSelectedItem");
            }
        }

        private GraphItem targetSelectedItem;
        public GraphItem TargetSelectedItem
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

        private string verticesText;
        public string VerticesText
        {
            get { return verticesText; }
            set
            {
                verticesText = value;
                OnPropertyChanged("VerticesText");
            }
        }

        private string resultText;
        public string ResultText
        {
            get { return resultText; }
            set
            {
                resultText = value;
                OnPropertyChanged("ResultText");
            }
        }

        public ICommand CreateGraphCommand { get; set; }
        public ICommand OriginLinkItemSelectedCommand { get; set; }
        public ICommand TargetLinkItemSelectedCommand { get; set; }
        public ICommand CreateLinkCommand { get; set; }
        public ICommand DeleteLinkCommand { get; set; }


        public ICommand OriginItemSelectedCommand { get; set; }
        public ICommand TargetItemSelectedCommand { get; set; }
        public ICommand SelectedPathCommand { get; set; }

        public ICommand CreateRandomGraphCommand { get; set; }

        public ICommand RunDFSCommand { get; set; }

        public ICommand ClearCommand { get; set; }

        public ObservableCollection<GraphItem> SourceItemsSource { get; set; }
        public ObservableCollection<GraphItem> TargetItemsSource { get; set; }
        public ObservableCollection<GraphItem> OriginLinkItemsSource { get; set; }
        public ObservableCollection<GraphItem> TargetLinkItemsSource { get; set; }

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

            OriginLinkItemSelectedCommand = new DelegateCommand(() =>
            {
                var selectedOrigin = OriginSelectedItem?.Name;

                if (selectedOrigin != null)
                {
                    if (selectedOrigin == TargetSelectedItem?.Name)
                    {
                        MessageBox.Show("The selected target cannot be the same as the origin. Please select another source.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        OriginSelectedItem = null;
                    }
                }
            });

            TargetLinkItemSelectedCommand = new DelegateCommand(() =>
            {
                var selectedTarget = TargetSelectedItem?.Name;

                if (selectedTarget != null)
                {
                    if (selectedTarget == OriginSelectedItem?.Name)
                    {
                        MessageBox.Show("The selected origin cannot be the same as the target. Please select another target.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        TargetSelectedItem = null;
                    }
                }
            });

            CreateLinkCommand = new DelegateCommand(() =>
            {
                bool linkExists = Links.Any(link => link.Source == OriginSelectedItem?.Name.ToString() && link.Target == TargetSelectedItem?.Name.ToString());

                if (OriginSelectedItem == null || TargetSelectedItem == null)
                {
                    MessageBox.Show("Please insert both origin and target to create a link", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (!linkExists)
                    {
                        Links.Add(new Link<string>(OriginSelectedItem.Name.ToString(), TargetSelectedItem.Name.ToString()));
                        UpdateGraphMessage();
                    }

                    Graph = new Graph<string>(Links);
                }
            });

            DeleteLinkCommand = new DelegateCommand(() =>
            {
                if (Links.Count > 0)
                {
                    Links.Remove(Links.Last());
                    UpdateGraphMessage();
                }
            });

            // --- ComboBox Path --- //
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

            // --- ComboBox Links --- //
            OriginLinkItemsSource = new ObservableCollection<GraphItem>
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

            TargetLinkItemsSource = new ObservableCollection<GraphItem>
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

            // --- Create Random --- //
            CreateRandomGraphCommand = new DelegateCommand(() =>
            {
                InitializeGraph();
                RandomEnabled = false;
                CreateEnabled = false;
            });

            // --- DFS --- //
            RunDFSCommand = new DelegateCommand(() =>
            {
                if (Links != null && Graph != null && Source != null && Target != null)
                {
                    ProcessDFS();
                }
                else
                {
                    MessageBox.Show("Please create a graph and select the origin and target first, then start the DFS algorithm.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });

            // --- Clear --- //
            ClearCommand = new DelegateCommand(() =>
            {
                if (ResultText != null)
                {
                    ResultText = string.Empty;
                }
                if (VerticesText != null)
                {
                    VerticesText = string.Empty;
                }
                if (PathText != null)
                {
                    PathText = string.Empty;
                }
                if (Links != null)
                {
                    Links.Clear();
                }
                if (Graph != null)
                {
                    Graph = null;
                }
                if (Source != null)
                {
                    Source = null;
                }
                if (Target != null)
                {
                    Target = null;
                }
                RandomEnabled = true;
                CreateEnabled = true;
                StackPannel = Visibility.Hidden;
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

        // - Message - //
        private void UpdateGraphMessage()
        {
            string message = "Links:\n";

            foreach (var link in Links)
            {
                message += $"\n{link}\n";
            }

            VerticesText = message;
        }

        private void InitializeGraph()
        {
            Random random = new Random();

            for (int i = 0; i < Vertices.Length; i++)
            {
                var source = Vertices[i];
                var target = Vertices[random.Next(Vertices.Length - 1)];

                if (target == source)
                {
                    target = Vertices.Last();
                }

                Links.Add(new Link<string>(source, target));

            }

            Graph = new Graph<string>(Links);
            UpdateGraphMessage();
        }

        private void ProcessDFS()
        {
            var paths = Graph.RoutesBetween(Source.Name.ToString(), Target.Name.ToString());

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

            ResultText = message;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
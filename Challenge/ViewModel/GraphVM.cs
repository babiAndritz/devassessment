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

        private string source;
        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        private string target;
        public string Target
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

        private object originSelectedItem;
        public object OriginSelectedItem
        {
            get { return originSelectedItem; }
            set
            {
                originSelectedItem = value;
                OnPropertyChanged("OriginSelectedItem");
            }
        }

        private object targetSelectedItem;
        public object TargetSelectedItem
        {
            get { return targetSelectedItem; }
            set
            {
                targetSelectedItem = value;
                OnPropertyChanged("TargetSelectedItem");
            }
        }

        private ObservableCollection<string> originAllItems;
        public ObservableCollection<string> OriginAllItems
        {
            get { return originAllItems; }
            set
            {
                originAllItems = value;
                OnPropertyChanged("OriginAllItems");
            }
        }

        private ObservableCollection<string> targetAllItems;
        public ObservableCollection<string> TargetAllItems
        {
            get { return targetAllItems; }
            set
            {
                targetAllItems = value;
                OnPropertyChanged("TargetAllItems");
            }
        }

        private bool originIsEnabled;
        public bool OriginIsEnabled
        {
            get { return originIsEnabled; }
            set
            {
                originIsEnabled = value;
                OnPropertyChanged("OriginIsEnabled");
            }
        }

        public GraphVM()
        {
            InitializeVertices();
            InitializeLinks();

            Links.Clear();
            Graph = null;
            StackPannel = Visibility.Hidden;
            RandomEnabled = true;
            CreateEnabled = true;
            OriginIsEnabled = true;

            CreateGraphCommand = new DelegateCommand(() =>
            {
                StackPannel = Visibility.Visible;
                RandomEnabled = false;
                CreateEnabled = false;
            });

            OriginItemSelectedCommand = new DelegateCommand(() =>
            {
                var selectedOrigin = (OriginSelectedItem).ToString();
                foreach (var item in TargetAllItems)
                {
                    if (item.ToString() == selectedOrigin)
                    {
                        OriginIsEnabled = false;
                    }
                    else 
                    {
                        OriginIsEnabled = true;
                    }
                }
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

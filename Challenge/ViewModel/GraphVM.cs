using Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.ViewModel
{
    public class GraphVM : INotifyPropertyChanged
    {
        private Graph<string> graph;
        public Graph<string> Graph
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GraphVM()
        {
            InitializeVertices();
            InitializeLinks();
        }

        private void InitializeLinks()
        {
            Links = new List<ILink<string>>();
        }

        private void InitializeVertices()
        {
            Vertices = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };
        }
    }
}

using Challenge.View;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Challenge.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        // --- Main variables --- //
        public string githubLink = "https://github.com/babiAndritz/devassessment";
        public string GithubLink
        {
            get { return githubLink; }
            set
            {
                githubLink = value;
                OnPropertyChanged("GithubLink");
            }
        }


        /* - Commands - */
        public ICommand OpenGraphPage { get; set; }
        public ICommand OpenSubtitlePage { get; set; }
        public ICommand OpenGithubLinkPage { get; set; }


        public MainVM()
        {
            OpenGraphPage = new DelegateCommand(() =>
            {
                GraphWindow page = new();
                page.Show();
            });

            OpenSubtitlePage = new DelegateCommand(() =>
            {
                SubtitleWindow page = new();
                page.Show();
            });

            OpenGithubLinkPage = new DelegateCommand(() =>
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = GithubLink,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error to open the link: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

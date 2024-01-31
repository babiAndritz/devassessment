using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SubtitleTimeshift;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Challenge1.ViewModel;
using Path = System.IO.Path;

namespace Challenge1.View
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

        private void TimeSpanButton_Click(object sender, RoutedEventArgs e)
        {

            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Subtitle files (*.srt)|*.srt|All files(*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (dialog.ShowDialog() == true)
            {
                viewModel.InputFile = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read);

                MessageBox.Show($"File selected with success: {dialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                string fileName = Path.GetFileName(dialog.FileName);

                FileResult.Text = $"File: {fileName}";
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.InputFile == null)
            {
                MessageBox.Show("Please select a file, then click this button.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else if (viewModel.TimeSpan <= TimeSpan.Zero)
            {
                MessageBox.Show("Please write the timeSpan you wish to use on the subtitle, then click this button.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                using (viewModel.InputFile)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Subtitle files (*.srt)|*.srt|All files(*.*)|*.*";
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        viewModel.OutputFile = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                        await Shifter.Shift(viewModel.InputFile, viewModel.OutputFile, viewModel.TimeSpan, viewModel.Encoding);

                        MessageBox.Show($"File modified and saved with success: {saveFileDialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        string fileName = Path.GetFileName(saveFileDialog.FileName);

                        OutFileResult.Text = $"Output File: {fileName}";

                    }
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.TimeSpan = TimeSpan.Zero;
            TimeResult.Text = String.Empty;
            viewModel.InputFile = null;
            FileResult.Text = String.Empty;
            viewModel.OutputFile = null;
            OutFileResult.Text = String.Empty;

        }
    }
    
}

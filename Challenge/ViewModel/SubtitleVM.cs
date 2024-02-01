﻿using Prism.Commands;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using SubtitleTimeshift;

namespace Challenge.ViewModel
{
    public class SubtitleVM : INotifyPropertyChanged
    {
        private Stream inputFile;
        public Stream? InputFile
        {
            get { return inputFile; }
            set
            {
                inputFile = value;
                OnPropertyChanged("InputFile");
            }
        }

        private Stream outputFile;
        public Stream? OutputFile
        {
            get { return outputFile; }
            set
            {
                outputFile = value;
                OnPropertyChanged("OutputFile");
            }
        }

        public TimeSpan timeSpan;
        public TimeSpan TimeSpan
        {
            get { return timeSpan; }
            set
            {
                timeSpan = value;
                OnPropertyChanged("TimeSpan");
            }
        }

        private Encoding encoding = System.Text.Encoding.UTF8;
        public Encoding Encoding
        {
            get { return encoding; }
            set
            {
                encoding = value;
                OnPropertyChanged("Encoding");
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand SelectFileCommand { get; set; }
        public ICommand OutputFileCommand { get; set; }
        public ICommand ClearCommand { get; set; }

        public string timeResultText;
        public string TimeResultText
        {
            get { return timeResultText; }
            set
            {
                timeResultText = value;
                OnPropertyChanged("TimeResultText");
            }
        }

        public string timeText;
        public string TimeText
        {
            get { return timeText; }
            set
            {
                timeText = value;
                OnPropertyChanged("TimeText");
            }
        }

        public string fileResultText;
        public string FileResultText
        {
            get { return fileResultText; }
            set
            {
                fileResultText = value;
                OnPropertyChanged("FileResultText");
            }
        }

        public string outFileResultText;
        public string OutFileResultText
        {
            get { return outFileResultText; }
            set
            {
                outFileResultText = value;
                OnPropertyChanged("OutFileResultText");
            }
        }

        public SubtitleVM()
        {

            SaveCommand = new DelegateCommand(() =>
            {
                if (long.TryParse(TimeText, out long milliseconds))
                {
                    TimeSpan = TimeSpan.FromMilliseconds(milliseconds);
                    TimeText = string.Empty;
                    TimeResultText = TimeSpan.ToString();  
                }
                else
                {
                    MessageBox.Show("Please write a correct TimeSpan.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });

            SelectFileCommand = new DelegateCommand(() =>
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Subtitle files (*.srt)|*.srt|All files(*.*)|*.*";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (dialog.ShowDialog() == true)
                {
                    InputFile = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read);

                    MessageBox.Show($"File selected with success: {dialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    string fileName = Path.GetFileName(dialog.FileName);

                    FileResultText = $"File: {fileName}";
                }

            });

            OutputFileCommand = new DelegateCommand(async () =>
            {
                if (InputFile == null)
            {
                    MessageBox.Show("Please select a file, then click this button.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (TimeSpan <= TimeSpan.Zero)
                {
                    MessageBox.Show("Please write the timeSpan you wish to use on the subtitle, then click this button.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    using (InputFile)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Subtitle files (*.srt)|*.srt|All files(*.*)|*.*";
                        saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            OutputFile = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                            await Shifter.Shift(InputFile, OutputFile, TimeSpan, Encoding);

                            MessageBox.Show($"File modified and saved with success: {saveFileDialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            string fileName = Path.GetFileName(saveFileDialog.FileName);

                            OutFileResultText = $"Output File: {fileName}";

                        }
                    }
                }
            });

            ClearCommand = new DelegateCommand(() =>
            {
                TimeSpan = TimeSpan.Zero;
                TimeResultText = String.Empty;
                InputFile = null;
                FileResultText = String.Empty;
                OutputFile = null;
                OutFileResultText = String.Empty;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

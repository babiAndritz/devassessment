﻿using Prism.Commands;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace Challenge.ViewModel
{
    public class SubtitleVM : INotifyPropertyChanged
    {
        

        private Stream inputFile;
        public Stream InputFile
        {
            get { return inputFile; }
            set
            {
                inputFile = value;
                OnPropertyChanged("InputFile");
            }
        }

        private Stream outputFile;
        public Stream OutputFile
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand SaveCommand { get; set; }
        public string texto { get; set; }


        public SubtitleVM()
        {
            timeSpan = new TimeSpan(123);
            SaveCommand = new DelegateCommand(() =>
            {
                texto = timeSpan.ToString();
                
            });
        }
    }
}
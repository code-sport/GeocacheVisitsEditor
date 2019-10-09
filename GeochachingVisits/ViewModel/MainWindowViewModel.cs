using GeochachingVisits.Helper;
using GeochachingVisits.Model;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GeochachingVisits.ViewModel
{
    public class MainWindowViewModel : NotifyObject
    {
        private ObservableCollection<GeochachingVisitsModel> _visitsCollection;
        public ObservableCollection<GeochachingVisitsModel> GeochachingVisitsModel
        {
            get { return this._visitsCollection; }
            private set
            {
                if (value == this._visitsCollection)
                {
                    return;
                }

                this._visitsCollection = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand RunOpenFileCommand { get; }
        public ICommand RunSaveFileCommand { get; }
        public ICommand RunDeleteItemCommand { get; }

        public MainWindowViewModel()
        {
            this.RunOpenFileCommand = new RelayCommand(this.RunOpenFile);
            this.RunSaveFileCommand = new RelayCommand(this.RunSaveFile);
            this.RunDeleteItemCommand = new RelayCommand(this.RunDeleteItemFile);
            this._visitsCollection = new ObservableCollection<GeochachingVisitsModel>();
            this._visitsCollection.CollectionChanged += this._visitsCollection_CollectionChanged;
        }

        private void _visitsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (var item in e.NewItems.OfType<INotifyPropertyChanged>())
                {
                    item.PropertyChanged += this.visitsCollection_PropertyChanged;
                }
            }
            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (var item in e.OldItems.OfType<INotifyPropertyChanged>())
                {
                    item.PropertyChanged -= this.visitsCollection_PropertyChanged;
                }
            }
        }

        private void visitsCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var row = sender as GeochachingVisitsModel;
            this.SaveData(row);
        }

        private void SaveData(GeochachingVisitsModel row)
        {
            //Save the row to the database here.
        }


        private void RunOpenFile(object obj)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (this.GeochachingVisitsModel.Count > 0
                    && MessageBox.Show("Should the list be cleared?", "Question", MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    this.GeochachingVisitsModel.Clear();
                }

                //Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();

                using (var reader = new StreamReader(fileStream))
                {
                    GeochachingVisitsModel item = null;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (line != null)
                        {
                            var values = line.Split(',');

                            if (values.Length == 4 && values[3].StartsWith("\""))
                            {
                                if (item != null)
                                {
                                    this._visitsCollection.Add(item);
                                }

                                item = new GeochachingVisitsModel
                                {
                                    GcCode = values[0],
                                    FoundDate = values[1],
                                    FoundStatus = values[2],
                                    Message = values[3]
                                };
                            }
                            else if (item != null)
                            {
                                item.Message += $"\r\n{line}";
                            }
                        }
                    }
                    if (item != null)
                    {
                        this.GeochachingVisitsModel.Add(item);
                    }
                }
            }

            this.OnPropertyChanged($"GeochachingVisitsModel");

        }

        private void RunDeleteItemFile(object obj)
        {
            if (obj is GeochachingVisitsModel visit)
            {
                this.GeochachingVisitsModel.Remove(visit);
            }
        }

        private void RunSaveFile(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,
                AddExtension = true,
                DefaultExt = "txt",
                FileName = "geocache_visits.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var fileName = saveFileDialog.FileName;
                using (var file = new StreamWriter(fileName))
                {
                    foreach (var item in this.GeochachingVisitsModel)
                    {
                        file.Write($"{item.GcCode},{item.FoundDate},{item.FoundStatus},{item.Message}\r\n");
                    }
                }
                MessageBox.Show($"Successful saved in {fileName}", "Saved", MessageBoxButton.OK);
            }
        }

    }

}
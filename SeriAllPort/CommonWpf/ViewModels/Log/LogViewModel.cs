using System.Collections.ObjectModel;
using System.Windows;

namespace CommonWpf.ViewModels.Log
{
    public class LogViewModel : ViewModel
    {
        private ObservableCollection<LogEntry> _entries = [];
        public ObservableCollection<LogEntry> Entries
        {
            get => _entries;
            set
            {
                if (_entries != value)
                {
                    _entries = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _autoScroll = true;
        public bool AutoScroll
        {
            get => _autoScroll;
            set
            {
                if (_autoScroll != value)
                {
                    _autoScroll = value;
                    OnPropertyChanged();
                }
            }
        }

        public SimpleCommand ClearCommand { get; private set; }

        public LogViewModel()
        {
            ClearCommand = new SimpleCommand((parameters) => Clear());
        }

        public void Clear()
        {
            Entries.Clear();
        }

        public void AppendLog(string message)
        {
            AppendLog(DateTime.Now, message);
        }

        public void AppendLog(DateTime time, string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Entries.Count > 1000)
                {
                    Entries.RemoveAt(0);
                }

                if (Entries.Count <= 0)
                {
                    _entries.Add(new LogEntry(time, message));
                }
                else
                {
                    for (int i = Entries.Count - 1; i >= 0; --i)
                    {
                        if (time >= Entries[i].Time)
                        {
                            _entries.Insert(i + 1, new LogEntry(time, message));
                            break;
                        }
                    }
                }
            });
        }
    }
}

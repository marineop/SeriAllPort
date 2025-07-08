using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CommonWpf.ViewModels.Log
{
    public class LogViewModel : ViewModel
    {
        private ObservableCollection<LogEntry> _entries = new ObservableCollection<LogEntry>();
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

        public ICommand ClearCommand { get; private set; }

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
            DateTime now = DateTime.Now;
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Entries.Count > 1000)
                {
                    Entries.RemoveAt(0);
                }

                _entries.Add(new LogEntry(now, message));
            });
        }
    }
}

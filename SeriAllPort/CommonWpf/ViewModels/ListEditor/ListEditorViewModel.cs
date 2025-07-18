using System.Collections.ObjectModel;

namespace CommonWpf.ViewModels.ListEditor
{
    public abstract class ListEditorViewModel<T> : ViewModel where T : class, IListEditorItem
    {
        public abstract string Name { get; }

        private ObservableCollection<T> _items = [];
        public ObservableCollection<T> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged();
                }
            }
        }

        private T? _selectedItem;
        public virtual T? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged();

                    UpCommand.RaiseCanExecuteChangedEvent();
                    DownCommand.RaiseCanExecuteChangedEvent();
                    DeleteCommand.RaiseCanExecuteChangedEvent();
                }
            }
        }

        private int _selectedItemIndex;
        public int SelectedItemIndex
        {
            get => _selectedItemIndex;
            set
            {
                if (_selectedItemIndex != value)
                {
                    _selectedItemIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        protected IShowErrorDialog ShowErrorDialog { get; set; }

        public abstract ObservableCollection<Tuple<string, object>> NewTypes { get; }

        public SimpleCommand UpCommand { get; private set; }
        public SimpleCommand DownCommand { get; private set; }
        public SimpleCommand NewCommand { get; private set; }
        public SimpleCommand DeleteCommand { get; private set; }

        public ListEditorViewModel(
            IList<T> items,
            int selectedIndex,
            IShowErrorDialog showErrorDialog)
        {
            Items = new ObservableCollection<T>(items);

            ShowErrorDialog = showErrorDialog;

            NewCommand = new SimpleCommand((parameter) => ItemNew(parameter));

            UpCommand = new SimpleCommand(
                (parameter) => ItemUp(),
                (parameter) => SelectedItem != null);

            DownCommand = new SimpleCommand(
                (parameter) => ItemDown(),
                (parameter) => SelectedItem != null);

            DeleteCommand = new SimpleCommand(
                (parameter) => ItemDelete(),
                (parameter) =>
                {
                    return SelectedItem != null && !SelectedItem.CanNotDelete;
                });

            SelectedItem = Items[selectedIndex];
        }

        private void ItemUp()
        {
            if (SelectedItemIndex >= 0)
            {
                int index = SelectedItemIndex;

                if (index > 0)
                {
                    T selectedPrevious = Items[index - 1];
                    Items.RemoveAt(index - 1);
                    Items.Insert(index, selectedPrevious);
                }
            }
        }

        private void ItemDown()
        {
            if (SelectedItemIndex >= 0)
            {
                int index = SelectedItemIndex;

                if (index < Items.Count - 1)
                {
                    T selectedNext = Items[index + 1];
                    Items.RemoveAt(index + 1);
                    Items.Insert(index, selectedNext);
                }
            }
        }

        public abstract void ItemNew(object? parameter);

        private void ItemDelete()
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.CanNotDelete)
                {
                    ShowErrorDialog.ShowError("Error", $"The {SelectedItem.Name} T can not be deleted.");
                }
                else
                {
                    Items.Remove(SelectedItem);
                }
            }
        }
    }
}

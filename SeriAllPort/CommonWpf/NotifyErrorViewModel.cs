using System.Collections;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CommonWpf
{
    public class NotifyErrorViewModel : ViewModel, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        [JsonIgnore]
        public bool HasErrors => _errors.Count > 0;

        private readonly Dictionary<string, List<string>> _errors = [];

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName != null && _errors.TryGetValue(propertyName, out List<string>? errors))
            {
                return errors;
            }

            return Array.Empty<string>();
        }

        protected void ClearErrors(string propertyName)
        {
            if (_errors.Remove(propertyName))
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public void AddError(string propertyName, string error)
        {
            if (!_errors.TryGetValue(propertyName, out List<string>? value))
            {
                value = [];
                _errors[propertyName] = value;
            }

            if (!value.Contains(error))
            {
                value.Add(error);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
    }
}

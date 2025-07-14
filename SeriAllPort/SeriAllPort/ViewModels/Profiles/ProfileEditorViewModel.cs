using CommonWpf;
using CommonWpf.FileHelper;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.Profiles
{
    public class ProfileEditorViewModel : ViewModel
    {
        private ObservableCollection<Profile> _profiles = new ObservableCollection<Profile>();
        public ObservableCollection<Profile> Profiles
        {
            get => _profiles;
            set
            {
                if (_profiles != value)
                {
                    _profiles = value;
                    OnPropertyChanged();
                }
            }
        }

        private Profile? _selectedProfile;
        public Profile? SelectedProfile
        {
            get => _selectedProfile;
            set
            {
                if (_selectedProfile != value)
                {
                    _selectedProfile = value;
                    OnPropertyChanged();

                    ProfileUpCommand.RaiseCanExecuteChangedEvent();
                    ProfileDownCommand.RaiseCanExecuteChangedEvent();
                    ProfileDeleteCommand.RaiseCanExecuteChangedEvent();
                }
            }
        }

        private int _selectedProfileIndex;
        public int SelectedProfileIndex
        {
            get => _selectedProfileIndex;
            set
            {
                if (_selectedProfileIndex != value)
                {
                    _selectedProfileIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private IShowErrorDialog ShowErrorDialog { get; set; }

        public SimpleCommand ProfileUpCommand { get; private set; }
        public SimpleCommand ProfileDownCommand { get; private set; }
        public SimpleCommand ProfileNewCommand { get; private set; }
        public SimpleCommand ProfileDeleteCommand { get; private set; }

        public ProfileEditorViewModel(
            IList<Profile> profiles,
            Profile currentProfile,
            IShowErrorDialog showErrorDialog)
        {
            foreach (Profile profile in profiles)
            {
                Profiles.Add(profile.CreateClone());
            }

            ShowErrorDialog = showErrorDialog;

            ProfileNewCommand = new SimpleCommand((parameter) => ProfileNew());

            ProfileUpCommand = new SimpleCommand(
                (parameter) => ProfileUp(),
                (parameter) => SelectedProfile != null);

            ProfileDownCommand = new SimpleCommand(
                (parameter) => ProfileDown(),
                (parameter) => SelectedProfile != null);

            ProfileDeleteCommand = new SimpleCommand(
                (parameter) => ProfileDelete(),
                (parameter) =>
                {
                    return SelectedProfile != null && !SelectedProfile.CanNotDelete;
                });

            SelectedProfile = currentProfile;
        }

        private void ProfileUp()
        {
            if (SelectedProfileIndex >= 0)
            {
                IList<Profile>? profiles = Profiles;
                int index = SelectedProfileIndex;

                if (index > 0)
                {
                    Profile selectedProfilePrevious = profiles[index - 1];
                    profiles.RemoveAt(index - 1);
                    profiles.Insert(index, selectedProfilePrevious);
                }
            }
        }

        private void ProfileDown()
        {
            if (SelectedProfileIndex >= 0)
            {
                IList<Profile>? profiles = Profiles;
                int index = SelectedProfileIndex;

                if (index < profiles.Count - 1)
                {
                    Profile selectedProfileNext = profiles[index + 1];
                    profiles.RemoveAt(index + 1);
                    profiles.Insert(index, selectedProfileNext);
                }
            }
        }

        private void ProfileNew()
        {
            List<string> profileNames = new List<string>();

            foreach (Profile profile in Profiles)
            {
                profileNames.Add(profile.Name);
            }

            string newProfileName = GetUniqueName(profileNames, "Profile");

            Profile newProfile = new Profile(
                newProfileName,
                AppDataFolderFileHelper.GenerateUnusedId(Profiles));

            Profiles.Add(newProfile);

            SelectedProfile = newProfile;
        }

        private void ProfileDelete()
        {
            if (SelectedProfile != null)
            {
                if (SelectedProfile.CanNotDelete)
                {
                    ShowErrorDialog.ShowError("Error", $"The {SelectedProfile.Name} Profile can not be deleted.");
                }
                else
                {
                    Profiles.Remove(SelectedProfile);
                }
            }
        }

        private string GetUniqueName(IList<string> currentList, string prefix)
        {
            HashSet<string> table = new HashSet<string>();
            foreach (string s in currentList)
            {
                if (s.StartsWith(prefix))
                {
                    table.Add(s);
                }
            }

            int numberNow = 0;
            while (true)
            {
                ++numberNow;
                string name = $"{prefix}{numberNow}";
                if (!table.Contains(name))
                {
                    break;
                }
            }

            return $"{prefix}{numberNow}";
        }
    }
}

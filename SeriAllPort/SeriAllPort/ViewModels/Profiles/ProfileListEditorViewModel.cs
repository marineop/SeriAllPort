using CommonWpf;
using CommonWpf.FileHelper;
using CommonWpf.ViewModels.ListEditor;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.Profiles
{
    public class ProfileListEditorViewModel : ListEditorViewModel<Profile>
    {
        public override string Name => "Profiles";

        public ObservableCollection<Profile> Profiles => Items;

        public Profile? SelectedProfile
        {
            get => base.SelectedItem;
            set => base.SelectedItem = value;
        }

        public override ObservableCollection<Tuple<string, object>> NewTypes =>
        [
           Tuple.Create<string,object>("Profile", 0)
        ];

        public ProfileListEditorViewModel(IList<Profile> items,
                                      int selectedIndex,
                                      IShowErrorDialog showErrorDialog)
            : base(items, selectedIndex, showErrorDialog)
        {
        }

        public override void ItemNew(object? parameter)
        {
            List<string> profileNames = [];

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

        private static string GetUniqueName(IList<string> currentList, string prefix)
        {
            HashSet<string> table = [];
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

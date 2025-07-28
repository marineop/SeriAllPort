using CommonWpf;
using CommonWpf.FileHelper;
using CommonWpf.ViewModels.ListEditor;
using System.Collections.ObjectModel;

namespace SeriAllPort.ViewModels.Profiles
{
    public class ProfileListEditor : ListEditorViewModel<Profile>
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

        public ProfileListEditor(IList<Profile> items,
                                      int selectedIndex,
                                      IShowErrorDialog showErrorDialog)
            : base(items, selectedIndex, showErrorDialog)
        {
        }

        public override void ItemNew(object? parameter)
        {
            string newProfileName = NameHelper.GetUniqueName(Profiles, "Profile");

            Profile newProfile = new Profile(
                newProfileName,
                AppDataFolderFileHelper.GenerateUnusedId(Profiles));

            Profiles.Add(newProfile);

            SelectedProfile = newProfile;
        }
    }
}

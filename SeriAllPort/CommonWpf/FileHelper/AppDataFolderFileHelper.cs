using System.Collections.ObjectModel;
using System.IO;

namespace CommonWpf.FileHelper
{
    public static class AppDataFolderFileHelper
    {
        public static ObservableCollection<T> LoadFiles<T>() where T : IAppDataFolderFile
        {
            string folder = Path.Combine(FileSerializer.AppDataPath, T.AppDataSubFolder);
            string filePattern = $"*.{T.FileExtensionName}";

            FileSerializer.CreateFolderIfNotExist(folder);

            IEnumerable<string> files = Directory.EnumerateFiles(
                folder,
                filePattern,
                SearchOption.TopDirectoryOnly);

            List<T> loadedTs = new List<T>();
            HashSet<Guid> ids = new HashSet<Guid>();

            foreach (string file in files)
            {
                T? loadedT = FileSerializer.Load<T>(file);
                if (loadedT != null && !ids.Contains(loadedT.Id))
                {
                    loadedTs.Add(loadedT);
                    ids.Add(loadedT.Id);
                }
            }

            loadedTs.Sort((a, b) => a.Order.CompareTo(b.Order));

            return new ObservableCollection<T>(loadedTs);
        }

        public static void SaveFiles<T>(IList<T> listOfT) where T : IAppDataFolderFile
        {
            for (int i = 0; i < listOfT.Count; ++i)
            {
                listOfT[i].Order = i;
            }

            string folder = Path.Combine(FileSerializer.AppDataPath, T.AppDataSubFolder);
            string filePattern = $"*.{T.FileExtensionName}";

            FileSerializer.CreateFolderIfNotExist(folder);

            IEnumerable<string> existTFiles = Directory.EnumerateFiles(
                folder,
                filePattern,
                SearchOption.TopDirectoryOnly);

            HashSet<string> newTFiles = new HashSet<string>();

            foreach (T t in listOfT)
            {
                string subPath = Path.Combine(folder, $"{t.Id}.{T.FileExtensionName}");
                string newFileFullPath = FileSerializer.SaveToAppDataPath(t, subPath);
                newTFiles.Add(newFileFullPath);
            }

            foreach (string oldTfile in existTFiles)
            {
                if (!newTFiles.Contains(oldTfile))
                {
                    File.Delete(oldTfile);
                }
            }
        }

        public static Guid GenerateUnusedId<T>(IList<T> existedTs) where T : IAppDataFolderFile
        {
            HashSet<Guid> ids = new HashSet<Guid>();

            foreach (T t in existedTs)
            {
                ids.Add(t.Id);
            }

            Guid newId = Guid.NewGuid();
            while (ids.Contains(newId))
            {
                newId = Guid.NewGuid();
            }

            return newId;
        }
    }
}


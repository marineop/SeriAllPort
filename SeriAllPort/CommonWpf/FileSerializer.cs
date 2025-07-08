using System.IO;
using System.Text.Json;

namespace CommonWpf
{
    public static class FileSerializer<T> where T : new()
    {
        public static void Save(T data, string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string? pathFolder = Path.GetDirectoryName(path);
            if (pathFolder != null && !Directory.Exists(pathFolder))
            {
                Directory.CreateDirectory(pathFolder);
            }

            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(path, json);
        }

        public static void SaveToAppDataPath(T data, string subPath)
        {
            string folderPath = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create);

            string path = Path.Combine(folderPath, subPath);

            Save(data, path);
        }

        public static T Load(string path)
        {
            if (!File.Exists(path))
            {
                return new T();
            }

            string json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<T>(json) ?? new T();
        }

        public static T LoadFromAppDataPath(string subPath)
        {
            string folderPath = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create);

            string path = Path.Combine(folderPath, subPath);

            return Load(path);
        }
    }
}

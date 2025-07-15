using System.IO;
using System.Text.Json;

namespace CommonWpf.FileHelper
{
    public static class FileSerializer
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public static string AppName { get; set; } = string.Empty;

        public static void Save<T>(T data, string path)
        {
            string json = JsonSerializer.Serialize(data, _options);

            string? pathFolder = Path.GetDirectoryName(path);

            CreateFolderIfNotExist(pathFolder);

            string tempFilePath = $"{path}.t";

            File.WriteAllText(tempFilePath, json);
            File.Copy(tempFilePath, path, overwrite: true);
            File.Delete(tempFilePath);
        }

        public static string SaveToAppDataPath<T>(T data, string subPath)
        {
            string path = Path.Combine(AppDataPath, subPath);

            Save(data, path);

            return path;
        }

        public static T? Load<T>(string path)
        {
            string json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<T>(json);
        }

        public static T? LoadFromAppDataPath<T>(string subPath)
        {
            string path = Path.Combine(AppDataPath, subPath);

            return Load<T>(path);
        }

        public static void CreateFolderIfNotExist(string? path)
        {
            if (path != null && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string AppDataPath
        {
            get
            {
                string path = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData,
                    Environment.SpecialFolderOption.Create),
                    AppName);

                return path;
            }
        }
    }
}

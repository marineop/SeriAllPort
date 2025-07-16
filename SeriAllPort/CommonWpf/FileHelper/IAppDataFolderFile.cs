namespace CommonWpf.FileHelper
{
    public interface IAppDataFolderFile
    {
        static abstract string AppDataSubFolder { get; }

        static abstract string FileExtensionName { get; }

        Guid Id { get; set; }

        int Order { get; set; }
    }
}

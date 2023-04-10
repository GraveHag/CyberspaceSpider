namespace CS_Core
{
    /// <summary>
    /// FileService
    /// can handle any file
    /// </summary>
    public sealed class FileService : IFileService, IService
    {
        public FileService()
        {   
        }

        FileInfo? FileInfo;

        static bool Exists(string path) => File.Exists(path);

        static string ReadFile(string path) => File.ReadAllText(path);

        void Load(string path)
        {
            if (!Exists(path)) throw new FileNotFoundException();
            FileInfo = new FileInfo(path);
        }

        void IFileService.Load(string path) => Load(path);

        bool IFileService.Exists(string path) => Exists(path);
        
        string IFileService.LoadFileContent()
        {
            if (FileInfo == null) throw new FileNotFoundException();
            return ReadFile(FileInfo.FullName);
        }

        string IFileService.LoadFileContent(string path)
        {
            Load(path); return ReadFile(path);
        }
    }
}

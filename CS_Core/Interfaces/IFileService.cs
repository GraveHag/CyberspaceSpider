namespace CS_Core
{
    public interface IFileService
    {
        void Load(string path);

        bool Exists(string path);

        string LoadFileContent();

        string LoadFileContent(string path);

    }
}

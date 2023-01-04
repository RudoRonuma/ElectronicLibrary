using ElectronicLibrary.UI;
using ElectronicLibrary.Core.DataBase;
using ElectronicLibrary.Core.DataBase.Client.MemoryClient;
using ElectronicLibrary.Core.DataBase.Client.FileClient;

namespace ElectronicLibrary
{
    public class Program
    {
        public static LibraryUI UIClient { get; set; }

        static Program()
        {
#if USE_CONSOLE_UI
            UIClient = new LibraryConsoleUI();
#endif
        }

#if USE_MEMORY_DB
        static ILibraryRepository GetLibraryRepository() =>
            _dbCtx;
        static ILibrarianRepository GetLibrarianRepository() =>
            _dbCtx;
        private static MemoryDBContext _dbCtx = new();
#elif USE_FILE_DB
        static ILibraryRepository GetLibraryRepository() =>
            _dbCtx;
        static ILibrarianRepository GetLibrarianRepository() =>
            _dbCtx;
        private static readonly FileDBContext _dbCtx = new();
#endif

        static void Main(string[] args)
        {
            UIClient.InitUI(GetLibraryRepository(), GetLibrarianRepository());
        }
    }
}
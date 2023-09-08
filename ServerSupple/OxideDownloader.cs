
using System.IO.Compression;

namespace ServerSupple
{
    internal class OxideDownloader
    {
        public static async Task CheckOxide()
        {
            var url = "https://umod.org/games/rust/download?tag=public";
            var archivePath = Path.Combine(AppContext.BaseDirectory, "oxidefiles.zip");
            var extractPath = AppContext.BaseDirectory;

            await DownloadFileAsync(url, archivePath);
            ExtractArchive(archivePath, extractPath);

            File.Delete(archivePath);
        }

        public static async Task DownloadFileAsync(string url, string destination)
        {
            using (var httpClient = new HttpClient())
            {
                var bytes = await httpClient.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(destination, bytes);
            }
        }

        public static void ExtractArchive(string archivePath, string destination)
        {
            ZipFile.ExtractToDirectory(archivePath, destination, true); // true указывает на замену файлов
        }
    }
}

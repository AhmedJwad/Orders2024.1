
namespace Orders.Backend.Helpers
{
    public class FileStorage : IFileStoragecs
    {

        public async Task RemoveFileAsync(string path, string nombreContenedor)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), path);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<string> SaveFileAsync(byte[] content, string extention, string containerName)
        {
            MemoryStream stream = new MemoryStream(content);
            string guid = $"{Guid.NewGuid()}{extention}";


            try
            {
                stream.Position = 0;
                string sharedFolderPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Orders.frondEnd");
                string path = Path.Combine(sharedFolderPath, $"wwwroot\\images\\{containerName}", guid);
                File.WriteAllBytes(path, stream.ToArray());
            }
            catch
            {
                return string.Empty;
            }

            return $"{containerName}/{guid}";
        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;

namespace FoxyFaceAPI
{
    public class CloudStorage
    {
        private static CloudStorage instance;

        private DirectoryInfo storagePath;
        
        public static CloudStorage Instance
        {
            get
            {
                if (instance == null)
                    throw new ArgumentException("Please initialize the storage first");

                return instance;
            }
        }

        public static void Initialize(string path = "wwwroot/storage")
        {
            instance = new CloudStorage(path);
        }

        private CloudStorage(string path)
        {
            this.storagePath = new DirectoryInfo(path);
            if (!storagePath.Exists)
            {
                Directory.CreateDirectory(storagePath.FullName);
            }
        }

        public async Task<Uri> UploadFile(String name, Stream stream)
        {
            var file = new FileInfo(Path.Combine(storagePath.FullName, name));
            if (file.Directory != null && !file.Directory.Exists)
            {
                file.Directory.Create();
            }
            
            using (var fileStream = file.OpenWrite())
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }

            return new Uri("https://foxyface.owl.sh/" + name);
        }

        public bool FileExists(String name)
        {
            var file = new FileInfo(Path.Combine(storagePath.FullName, name));

            return file.Exists;
        }
    }
}
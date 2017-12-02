using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FoxyFaceAPI
{
    public class CloudStorage
    {
        private CloudBlobContainer container;

        private static CloudStorage instance;
        public static CloudStorage Instance
        {
            get
            {
                if (instance == null)
                    throw new ArgumentException("Please initialize the storage first");

                return instance;
            }
        }

        public static void Initialize(string accountName, string key)
        {
            instance = new CloudStorage(accountName, key);
        }

        private CloudStorage(string accountName, string key)
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, key), true);
            
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get a reference to a container named "mycontainer."
            container = blobClient.GetContainerReference("foxyface-images");
        }

        public async Task<Uri> UploadFile(String name, Stream stream)
        {
            // Get a reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            
            await blockBlob.UploadFromStreamAsync(stream);

            return blockBlob.Uri;
        }

        public bool FileExists(String name)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            return blockBlob.ExistsAsync().Result;
        }
    }
}
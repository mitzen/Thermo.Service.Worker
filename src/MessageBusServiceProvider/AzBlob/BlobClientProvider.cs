using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Service.MessageBusServiceProvider.AzBlob
{
    public class BlobClientProvider
    {
        public BlobClientProvider()
        {

        }

        public async Task PushImageToStore(string targetContainer, string path)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=devstbank;AccountKey=0FZg1ynJlPo0sjy+NVNdfmDkZRrurzG2NVHEZultxdGC0efRzso0CsYqwku+lclngFn07BEFWJTLAEWWhqBRXg=="; 

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(targetContainer);
            await container.CreateIfNotExistsAsync();

            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(30);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create;

            var blob = container.GetBlockBlobReference(Path.GetFileName(path));

            var target = blob.Uri.ToString() + blob.GetSharedAccessSignature(sasConstraints);
            var cloudBlockBlob = new CloudBlockBlob(new Uri(target));
            await cloudBlockBlob.UploadFromFileAsync(path);
        }
    }
}

using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace Service.MessageBusServiceProvider.AzBlob
{
    public class BlobClientProvider
    {
        public async Task PushImageToStore(string sasUri, string path)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=devstbank;AccountKey=0FZg1ynJlPo0sjy+NVNdfmDkZRrurzG2NVHEZultxdGC0efRzso0CsYqwku+lclngFn07BEFWJTLAEWWhqBRXg=="; // your storage account access key
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("attendanceimage");

            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(30);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create;

            var blob = container.GetBlockBlobReference("outputfile.txt");

            var target = blob.Uri.ToString() + blob.GetSharedAccessSignature(sasConstraints);
            var cloudBlockBlob = new CloudBlockBlob(new Uri(target));
            await cloudBlockBlob.UploadFromFileAsync(path);
        }
    }
}

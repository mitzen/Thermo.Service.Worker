using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public interface IDataStoreProcesor
    {        
        Task<int> SavePersonAsync(string source);

        Task<int> SaveDevicesAsync(string source);

        Task<int> SaveAttendRecordAsync(string source);

        Task<int> SavePersonImgAsync(string source);
    }
}

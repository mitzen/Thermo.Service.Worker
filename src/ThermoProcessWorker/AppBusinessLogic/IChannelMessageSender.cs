using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Configuration;
using Service.ThermoDataModel.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Service.ThermoProcessWorker.AppBusinessLogic
{
    public interface IChannelMessageSender
    {
        void Setup(CancellationToken stoppingToken);

        Task SendMessagesToAzureServiceBus(AttendanceResponse attendanceRecResult);

        Task SendHeartBeatMessagesToAzureServiceBus(TargetDevice targetDevice);

    }
}
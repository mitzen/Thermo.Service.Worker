using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.ThermoDataModel.Configuration;
using Service.ThermoDataModel.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public interface ISendMailService
    {
        Task<int> SendMailAsync(AttendanceRecord record);
    }

    public class SendMailService : ISendMailService
    {
        private ILogger<SendMailService> _logger;
        private NotificationConfiguration _notificationConfiguration;

        public SendMailService(ILogger<SendMailService> logger, IOptions<NotificationConfiguration> notificationConfiguration)
        {
            _logger = logger;
            _notificationConfiguration = notificationConfiguration.Value;
        }

        public async Task<int> SendMailAsync(AttendanceRecord record)
        {
            await this.SendMailAsync();
            return 1;
        }

        private Task SendMailAsync()
        {
            this._logger.LogInformation($"Sending email : {this._notificationConfiguration.SmtpServer}");

            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("kepung@gmail.com");
            message.To.Add(new MailAddress("kepung@gmail.com"));
       
            message.Subject = "Test";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = "helllllll oo there";
            smtp.Port = this._notificationConfiguration.Port;
            smtp.Host = this._notificationConfiguration.SmtpServer; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(
                this._notificationConfiguration.Username, 
                this._notificationConfiguration.Password
             );
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);

            smtp.Send(message);
            return Task.CompletedTask;
        }
    }
}
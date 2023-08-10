﻿using AzCloudApp.MessageProcessor.Core.Utils;
using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Models;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class NotificationMessageProcessor 
        : INotificationProcessor
    {
        public float SafeBodyTemperature { get; set; }

        private readonly ISendMailService _sendMailService;
        public NotificationMessageProcessor(ISendMailService sendMail)
        {
            this._sendMailService = sendMail;
        }

        public async Task ProcessAsync(string source, ILogger logger)
        {
            logger.LogInformation($"NotificationMessageProcessor starts execution wth {this._sendMailService.GetType().Name}");

            if (!string.IsNullOrWhiteSpace(source))
            {
                var target = MessageConverter.GetMessageType<MailContentData>(source);

                if (target != null)
                    await this._sendMailService.SendMailAsync(target, logger);
            }
        }
    }
}

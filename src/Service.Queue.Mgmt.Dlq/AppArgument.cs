namespace Service.Queue.Mgmt.Dlq
{
    internal class AppArgument
    {
        public string Endpoint { get; set; }

        public string TargetQueueName { get; set; }

        public string TargetFilePath { get; set;  }
    }
}
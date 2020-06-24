namespace Service.Queue.Mgmt.ResetQueue
{
    internal class AppArgument
    {
        public string Endpoint { get; set; }

        public string TargetQueueName { get; set; }

        public string TargetFilePath { get; set;  }
    }
}
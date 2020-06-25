namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class ExecutionState
    {
        public ExecutionState(int status)
        {
            Status = status;
        }

        public int Status { get; set; }
    }
}
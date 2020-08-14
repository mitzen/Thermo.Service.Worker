namespace AzCloudApp.MessageProcessor.Core.EmailNotifier.Utils
{
    public static class TemplateContentHelper
    {
        public static string ReplaceContent(this string source, string from, string to)
        {
            return source.Replace(from, to);
        }
    }
}

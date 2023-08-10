using Xunit;
using AzCloudApp.MessageProcessor.Core.EmailNotifier.Utils;

namespace AzCloudApp.MessageProcessor.Core.Unit.Tests.EmailNotifier.Utils
{
    public class TemplateContentHelperTests
    {
        private const string ToTargetContent = "ok";

        [Fact]
        public void TemplateContent()
        {
            var contentReplace = "test";
            var result = contentReplace.ReplaceContent(contentReplace, ToTargetContent);
            Assert.Equal(ToTargetContent, result);
        }
    }
}
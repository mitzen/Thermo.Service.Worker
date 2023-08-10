using Xunit;
using AzCloudApp.MessageProcessor.Core.EmailNotifier.Utils;

namespace AzCloudApp.MessageProcessor.Core.Unit.Tests.EmailNotifier.Utils
{
    public class TemplateContentHelperTests
    {
        [Fact]
        public void TemplateContent()
        {
            var contentReplace = "test";
            var result = contentReplace.ReplaceContent("test", "ok");
            Assert.Equal("ok", result);
        }
    }
}

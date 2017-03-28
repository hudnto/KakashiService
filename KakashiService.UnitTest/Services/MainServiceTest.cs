using KakashiService.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KakashiService.UnitTest.Services
{
    [TestClass]
    public class MainServiceTest
    {
        [TestMethod]
        public void SimpleRun()
        {
            var main = new MainService();
            main.Execute();
        }
    }
}

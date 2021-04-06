using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Itamaram.Excel.SpecFlowPlugin.DotNetCore.IntegrationTests.Steps
{
    [Binding]
    public sealed class ExcelLoadingStepDefinitions
    {
        [Given("I have followed the instructions")]
        public void GivenIHaveFollowedTheInstructions()
        {
        }

        [Then("I should've loaded (.*?) (.*?) (.*?)")]
        public void ThenIShouldveLoaded(int a, int b, int c)
        {
            Assert.AreEqual(a, 10);
            Assert.AreEqual(b, 20);
            Assert.AreEqual(c, 30);
        }
    }
}

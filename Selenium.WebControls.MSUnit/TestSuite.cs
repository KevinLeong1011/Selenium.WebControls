/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/27 23:05:24
 * ***********************************************/
using Selenium.WebControls.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selenium.WebControls.MSUnit
{
    /// <summary>
    /// 
    /// </summary>
    public class TestSuite : TestSuiteBase
    {
        public TestSuite(string name) : base(name)
        {
        }

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            DataFactory.RegisterAssert<MSAssert>();
        }

        [TestCleanup]
        public override void TearDown()
        {
            base.TearDown();
        }
    }
}

using System;
using NUnit.Framework;
using Selenium.WebControls.Utils;

namespace Selenium.WebControls.NUnit
{
    public class NTestSuite : TestSuiteBase
    {
        public NTestSuite(string name) : base(name)
        {
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DataFactory.RegisterAssert<NAssert>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
    }
}

/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/23 19:40:03
 * ***********************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selenium.WebControls.MSUnit;
using System.Data;

namespace Selenium.WebControls.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class WebTableTest : TestSuite
    {
        public WebTableTest() : base("")
        {
        }

        [TestMethod]
        public void SelectData()
        {
            Describe("", s =>
            {
                s.ClickLink("table");
                DataRow dr = Tb("table").SelectData(1);
                string name = dr["名称"].ToString();
            });
        }
    }
}

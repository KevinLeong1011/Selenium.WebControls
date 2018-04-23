/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/8 1:49:13
 * ***********************************************/
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Selenium.WebControls.CaseGeneration
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigHelper
    {
        static ConfigHelper()
        {
            JObject obj = JObject.Parse(File.ReadAllText(@"Configs\CaseGeneration.json"));

        }
    }
}

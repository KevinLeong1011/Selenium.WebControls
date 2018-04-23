/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/28 1:59:36
 * ***********************************************/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selenium.WebControls.Environments
{
    /// <summary>
    /// 模板配置
    /// </summary>
    [JsonObject]
    public class TemplateConfig
    {
        /// <summary>
        /// 模板路径
        /// </summary>
        [JsonProperty]
        public string Location { get; set; }

        /// <summary>
        /// 标记前缀
        /// </summary>
        [JsonProperty]
        public string MarkPrefix { get; set; }
    }
}

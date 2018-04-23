/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:17:47
 * ***********************************************/
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Selenium.WebControls.Environments
{
    /// <summary>
    /// 测试环境
    /// </summary>
    [JsonObject]
    public class TestEnvironment
    {
        /// <summary>
        /// 被测应用程序名称
        /// </summary>
        [JsonProperty]
        public string AppName { get; set; }

        /// <summary>
        /// WebDriver所在地址
        /// </summary>
        [JsonProperty]
        public string DriverServiceLocation { get; set; }

        /// <summary>
        /// 当前使用的WebDriver配置
        /// </summary>
        [JsonProperty]
        public string ActiveDriverConfig { get; set; }

        /// <summary>
        /// 当前被测地址
        /// </summary>
        [JsonProperty]
        public string ActiveWebsiteConfig { get; set; }

        /// <summary>
        /// 测试网址配置
        /// </summary>
        [JsonProperty]
        public Dictionary<string, WebsiteConfig> WebSiteConfigs { get; set; }

        /// <summary>
        /// WebDriver配置字典
        /// </summary>
        [JsonProperty]
        public Dictionary<string, DriverConfig> DriverConfigs { get; set; }

        /// <summary>
        /// 使用的测试用例生成配置
        /// </summary>
        public string ActiveCaseGeneration { get; set; }

        /// <summary>
        /// 测试用例生成配置
        /// </summary>
        [JsonProperty]
        public Dictionary<string, CaseGenerationConfig> CaseGenerations { get; set; }

        /// <summary>
        /// 执行配置
        /// </summary>
        [JsonProperty]
        public ExecutionConfig ExecutionConfig { get; set; }
    }
}

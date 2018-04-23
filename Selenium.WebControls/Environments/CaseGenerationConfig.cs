/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:17:47
 * ***********************************************/
using Newtonsoft.Json;

namespace Selenium.WebControls.Environments
{
    /// <summary>
    /// 测试用例生成配置
    /// </summary>
    [JsonObject]
    public class CaseGenerationConfig
    {
        /// <summary>
        /// 测试用例生成配置名称
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// 测试用例生成类名
        /// </summary>
        [JsonProperty]
        public string GenerationTypeName { get; set; }

        /// <summary>
        /// 测试用例生成程序集名称
        /// </summary>
        [JsonProperty]
        public string AssemblyName { get; set; }
    }
}
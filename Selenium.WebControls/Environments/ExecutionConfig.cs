/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/7 14:57:15
 * ***********************************************/
using Newtonsoft.Json;

namespace Selenium.WebControls.Environments
{
    /// <summary>
    /// 执行配置
    /// </summary>
    [JsonObject]
    public class ExecutionConfig
    {
        /// <summary>
        /// 默认元素等待时间
        /// </summary>
        [JsonProperty]
        public int DefaultWaitingTime { get; set; }

        /// <summary>
        /// 是否自动化执行模式
        /// </summary>
        [JsonProperty]
        public bool Auto { get; set; }

        /// <summary>
        /// 命令描述配置文件名称
        /// </summary>
        [JsonProperty]
        public string CommandDescConfig { get; set; }

        /// <summary>
        /// 测试执行结果存放位置
        /// </summary>
        [JsonProperty]
        public string ResultLocation { get; set; }

        /// <summary>
        /// 允许的性能超时时间
        /// </summary>
        [JsonProperty]
        public int AllowedPerformanceTimout { get; set; }

        /// <summary>
        /// 模板配置
        /// </summary>
        [JsonProperty]
        public TemplateConfig TemplateConfig { get; set; }
    }
}

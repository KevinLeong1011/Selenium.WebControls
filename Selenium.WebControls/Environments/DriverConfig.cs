/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:17:47
 * ***********************************************/
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Selenium.WebControls.Environments
{
    /// <summary>
    /// Web driver 配置
    /// </summary>
    [JsonObject]
    public class DriverConfig
    {
        /// <summary>
        /// web driver 名称
        /// </summary>
        [JsonProperty]
        public string DriverTypeName { get; set; }

        /// <summary>
        /// dll 名称
        /// </summary>
        [JsonProperty]
        public string AssemblyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public Browsers BrowserValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty]
        public string RemoteCapabilities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty]
        public bool AutoStartRemoteServer { get; set; }
    }
}
/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:17:47
 * ***********************************************/
using Newtonsoft.Json;

namespace Selenium.WebControls.Environments
{
    [JsonObject]
    public class WebsiteConfig
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string BaseUrl { get; set; }
    }
}
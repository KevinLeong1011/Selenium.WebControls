/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/27 0:08:41
 * ***********************************************/
using Newtonsoft.Json.Linq;
using Selenium.WebControls.Environments;
using System;
using System.IO;

namespace Selenium.WebControls.Commands
{
    /// <summary>
    /// 命令描述管理器
    /// </summary>
    public class CommandDescriptions
    {
        private JObject jobject;

        private static CommandDescriptions instance = new CommandDescriptions();

        private CommandDescriptions()
        {
            string content = File.ReadAllText(IOHelper.FindFile(IOHelper.GetCurrentLocation(), EnvManager.CmdDescriptionFile));
            jobject = JObject.Parse(content);
        }

        /// <summary>
        /// 格式化命令字符串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Format(string name, params object[] args)
        {
            if (!instance.jobject.ContainsKey(name))
            {
                throw new Exception($"Cannot find command config item \"{name}\" in the file \"{EnvManager.CmdDescriptionFile}\"");
            }
            return string.Format(instance.jobject[name].Value<string>(), args);
        }
    }
}
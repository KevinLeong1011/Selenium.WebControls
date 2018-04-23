/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:59:20
 * ***********************************************/
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Reflection;

namespace Selenium.WebControls.Environments
{
    /// <summary>
    /// 
    /// </summary>
    public class EnvManager
    {
        private static Type driverType;
        private static DriverFactory driverFactory;
        private static Type caseHandlerType;
        private static string tplLocation;

        static EnvManager()
        {
            string fileaName = IOHelper.FindFile(IOHelper.GetCurrentLocation(), "appconfig.json");
            string content = File.ReadAllText(fileaName);
            TestEnvironment env = JsonConvert.DeserializeObject<TestEnvironment>(content);

            AppName = env.AppName;

            // Driver
            DriverConfig driverConfig = env.DriverConfigs[env.ActiveDriverConfig];
            driverFactory = new DriverFactory(IOHelper.Parse(env.DriverServiceLocation + "/" + env.ActiveDriverConfig));
            Assembly driverAssembly = Assembly.Load(driverConfig.AssemblyName);
            driverType = driverAssembly.GetType(driverConfig.DriverTypeName);

            // Website
            Website = env.WebSiteConfigs[env.ActiveWebsiteConfig];

            // execution config
            var execCfg = env.ExecutionConfig;
            DefaultWaitingTime = execCfg.DefaultWaitingTime;
            ResultLocation = IOHelper.Parse(execCfg.ResultLocation);
            // template config
            var tplCfg = execCfg.TemplateConfig;
            tplLocation = IOHelper.Parse(tplCfg.Location);
            MarkPrefix = tplCfg.MarkPrefix;
            Auto = env.ExecutionConfig.Auto;
            CmdDescriptionFile = env.ExecutionConfig.CommandDescConfig;
            if (!Auto)
            {
                // case config
                var caseHandlerConfig = env.CaseGenerations[env.ActiveCaseGeneration];
                if (caseHandlerConfig != null && !caseHandlerConfig.AssemblyName.NullOrWhiteSpace())
                {
                    Assembly caseHandlerAssembly = Assembly.Load(caseHandlerConfig.AssemblyName);
                    caseHandlerType = caseHandlerAssembly.GetType(caseHandlerConfig.GenerationTypeName);
                }
            }
        }

        /// <summary>
        /// 被测应用程序名称
        /// </summary>
        public static string AppName { get; private set; }

        /// <summary>
        /// 命令执行配置文件
        /// </summary>
        public static string CmdDescriptionFile { get; private set; }

        /// <summary>
        /// 被测网站配置
        /// </summary>
        public static WebsiteConfig Website { get; private set; }

        /// <summary>
        /// 默认元素等待时间
        /// </summary>
        public static int DefaultWaitingTime { get; }

        /// <summary>
        /// 执行结果保存位置
        /// </summary>
        public static string ResultLocation { get; set; }

        /// <summary>
        /// 测试用例生成器
        /// </summary>
        public static ICaseGenerator GetCaseGenerator() => Activator.CreateInstance(caseHandlerType) as ICaseGenerator;


        /// <summary>
        /// 标记前缀
        /// </summary>
        public static string MarkPrefix { get; }

        /// <summary>
        /// Execute auto-testing if true and generate documents if false.
        /// 如果为true，将会开启自动化指定，否则执行文档生成操作
        /// </summary>
        public static bool Auto { get; private set; }

        /// <summary>
        /// Find the template file in the directory specified in "appconfig.json".
        /// 从“appconfig.json”配置文件所设置的模板路径找到相应的文件。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FindTemplateFile(string fileName)
        {
            string filePath = IOHelper.FindFile(IOHelper.Parse(tplLocation), fileName);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new FileNotFoundException($"Cannot find the template file which name is {fileName}");
            }
            return filePath;
        }

        /// <summary>
        /// 创建WebDriver
        /// </summary>
        /// <returns></returns>
        public static IWebDriver CreateDriverInstance()
        {
            return driverFactory.CreateDriver(driverType);
        }
    }
}

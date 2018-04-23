/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/19 22:30:55
 * ***********************************************/
using OpenQA.Selenium;
using Selenium.WebControls.Environments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.WebControls.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class Runner
    {
        public Runner(string testSuite)
        {
            string dirName = $"{EnvManager.AppName}-Auto-{DateTime.Now.ToString("yyyyMMddhhmmss")}";
            Location = Path.Combine(EnvManager.ResultLocation, dirName);
            Directory.CreateDirectory(Location);
            ResultFile = Path.Combine(Location, testSuite + ".txt");
            sw = File.CreateText(ResultFile);
        }

        /// <summary>
        /// 测试结果存放位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 测试套件
        /// </summary>
        public string ResultFile { get; set; }

        public void RunCase(string caseName)
        {

        }


        public void Warn(string message)
        {

        }

        public void Debug(string message)
        {

        }

        public void Error(string message)
        {

        }

        public void Info(string message)
        {

        }

        /// <summary>
        /// 保存截图
        /// </summary>
        /// <param name="name"></param>
        /// <param name="screenshot"></param>
        public void SaveScreenshot(string name, Screenshot screenshot)
        {
            screenshot.ThrowIfNull("screenshot");
            screenshot.SaveAsFile(Path.Combine(Location, name));
        }

        private StreamWriter sw;
    }
}

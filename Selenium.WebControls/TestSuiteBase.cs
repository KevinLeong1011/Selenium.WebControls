/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/27 23:02:33
 * ***********************************************/
using Selenium.WebControls.Controls;
using Selenium.WebControls.Environments;
using Selenium.WebControls.Tracking;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Selenium.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    public class TestSuiteBase
    {
        protected TestSuiteBase()
        {

        }

        protected TestSuiteBase(string name)
        {
            // TODO
        }

        #region Attributes

        /// <summary>
        /// <see cref="WebSelenium"/>对象
        /// </summary>
        protected WebSelenium Selenium { get; private set; }

        #endregion Attributes

        #region Public Methods

        /// <summary>
        /// 测试初始化
        /// </summary>
        public virtual void SetUp()
        {
            if (EnvManager.Auto)
            {
                IWebDriver driver = EnvManager.CreateDriverInstance();
                Selenium = new WebSelenium(driver);
                Selenium.Maximize();
                Selenium.Open(EnvManager.Website.BaseUrl);
            }
            else
            {
                Selenium = new WebSelenium();
            }

        }

        /// <summary>
        /// 测试清理
        /// </summary>
        public virtual void TearDown()
        {
            if (EnvManager.Auto)
            {
                Selenium.Quit();
            }
        }

        /// <summary>
        /// 创建测试用例
        /// </summary>
        /// <param name="caseName"></param>
        /// <param name="action"></param>
        protected void Describe(string caseName, Action<WebSelenium> action)
        {
            Tracker.Log(TrackTag.Case, caseName);
            //try
            //{
                if (EnvManager.Auto)
                {
                    action?.Invoke(Selenium);
                }
            //}
            //catch (Exception ex)
            //{
            //    Screenshot screenshot = Selenium.Driver.TakeScreenshot();
            //    screenshot.SaveAsFile(Path.Combine(resultLocation, caseName, "screenshot.jpg"));
            //    throw ex;
            //}

            if (!EnvManager.Auto)
            {
                Tracker.WriteCase();
            }
        }

        /// <summary>
        /// 创建<see cref="WebControl"/>的简易方法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected WebControl T(string name)
        {
            return T<WebControl>(name);
        }

        /// <summary>
        /// Create a <see cref="WebTable"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected WebTable Tb(string name)
        {
            return T<WebTable>(name);
        }

        /// <summary>
        /// 创建<see cref="WebControl"/>的简易方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T T<T>(string name)
            where T : WebControl<T>
        {
            return Selenium.Handle<T>(name);
        }

        #endregion Public Methods

        #region Fields

        private string resultLocation; // TODO

        #endregion Fields
    }
}

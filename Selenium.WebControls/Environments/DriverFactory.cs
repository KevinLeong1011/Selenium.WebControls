/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:59:20
 * ***********************************************/
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Selenium.WebControls.Environments
{
    /// <summary>
    /// WebDriver工厂
    /// </summary>
    public class DriverFactory
    {
        string driverPath;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="driverPath"></param>
        public DriverFactory(string driverPath)
        {
            if (string.IsNullOrEmpty(driverPath))
            {
                this.driverPath = IOHelper.GetCurrentLocation();
            }
            else
            {
                this.driverPath = driverPath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DriverServicePath
        {
            get { return this.driverPath; }
        }

        /// <summary>
        /// 创建<see cref="IWebDriver"/>
        /// </summary>
        /// <param name="driverType"></param>
        /// <returns></returns>
        public IWebDriver CreateDriver(Type driverType)
        {
            List<Type> constructorArgTypeList = new List<Type>();
            IWebDriver driver = null;
            if (typeof(ChromeDriver).IsAssignableFrom(driverType))
            {
                ChromeDriverService service = ChromeDriverService.CreateDefaultService(this.driverPath);
                constructorArgTypeList.Add(typeof(ChromeDriverService));
                ConstructorInfo ctorInfo = driverType.GetConstructor(constructorArgTypeList.ToArray());
                return (IWebDriver)ctorInfo.Invoke(new object[] { service });
            }

            if (typeof(InternetExplorerDriver).IsAssignableFrom(driverType))
            {
                InternetExplorerDriverService service = InternetExplorerDriverService.CreateDefaultService(this.driverPath);
                constructorArgTypeList.Add(typeof(InternetExplorerDriverService));
                ConstructorInfo ctorInfo = driverType.GetConstructor(constructorArgTypeList.ToArray());
                return (IWebDriver)ctorInfo.Invoke(new object[] { service });
            }

            if (typeof(EdgeDriver).IsAssignableFrom(driverType))
            {
                EdgeDriverService service = EdgeDriverService.CreateDefaultService(this.driverPath);
                constructorArgTypeList.Add(typeof(EdgeDriverService));
                ConstructorInfo ctorInfo = driverType.GetConstructor(constructorArgTypeList.ToArray());
                return (IWebDriver)ctorInfo.Invoke(new object[] { service });
            }

            if (typeof(FirefoxDriver).IsAssignableFrom(driverType))
            {
                FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(this.driverPath);
                constructorArgTypeList.Add(typeof(FirefoxDriverService));
                ConstructorInfo ctorInfo = driverType.GetConstructor(constructorArgTypeList.ToArray());
                return (IWebDriver)ctorInfo.Invoke(new object[] { service });
            }

            driver = (IWebDriver)Activator.CreateInstance(driverType);
            return driver;
        }
    }
}
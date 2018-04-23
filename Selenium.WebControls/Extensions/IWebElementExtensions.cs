/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:13:44
 * ***********************************************/
using OpenQA.Selenium;
using Selenium.WebControls.Environments;
using System;
using System.Text.RegularExpressions;

namespace Selenium.WebControls.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IWebElementExtensions
    {
        /// <summary>
        /// 获取尝试性方案定位器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static RetryingElementSolutionLocator RetryingSolutionLocator(this ISearchContext context, TimeSpan timeSpan)
        {
            return new RetryingElementSolutionLocator(context, timeSpan);
        }

        /// <summary>
        /// 获取尝试性方案定位器
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static RetryingElementSolutionLocator RetryingSolutionLocator(this ISearchContext context)
        {
            return new RetryingElementSolutionLocator(context, TimeSpan.FromSeconds(EnvManager.DefaultWaitingTime));
        }

        /// <summary>
        /// 双击元素
        /// </summary>
        /// <param name="element"></param>
        public static void DoubleClick(this IWebElement element)
        {
            element.Click();
            element.Click();
        }

        /// <summary>
        /// 获取元素的值
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetValue(this IWebElement element)
        {
            string tag = element.TagName.ToLower();
            if (tag == "input")
            {
                return element.GetAttribute("value");
            }
            return element.Text;
        }

        /// <summary>
        /// 判断元素是否具有指定的内容
        /// </summary>
        /// <param name="element"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static bool HasContent(this IWebElement element, params string[] contents)
        {
            string html = element.Text;
            foreach (string content in contents)
            {
                if (!html.Contains(content)) return false;
            }
            return true;
        }

        /// <summary>
        /// 获取元素的innerHTML内容
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetInnerHtml(this IWebElement element)
        {
            return element.GetAttribute("innerHTML");
        }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool Editable(this IWebElement element)
        {
            string tagName = element.TagName.ToUpperInvariant();
            bool acceptableTagName = tagName == "INPUT" || tagName == "SELECT";
            string readOnlyAttribute = string.Empty;
            if (tagName == "INPUT")
            {
                readOnlyAttribute = element.GetAttribute("readonly");
                if (readOnlyAttribute != null && readOnlyAttribute == "false")
                {
                    readOnlyAttribute = string.Empty;
                }
            }

            return element.Enabled && acceptableTagName && string.IsNullOrEmpty(readOnlyAttribute);
        }

        /// <summary>
        /// 是否禁用
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool Disabled(this IWebElement element)
        {
            string value = element.GetAttribute("disabled").ToLower();
            return value == "true" || value == "disabled";
        }

        /// <summary>
        /// 是否包含给定的class
        /// </summary>
        /// <param name="element"></param>
        /// <param name="classes"></param>
        /// <returns></returns>
        public static bool HasClass(this IWebElement element, params string[] classes)
        {
            string attr = element.GetAttribute("class");
            foreach(string cls in classes)
            {
                if (!attr.Contains(cls)) return false;
            }
            return true;
        }
    }
}

/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/8 23:45:32
 * ***********************************************/
using OpenQA.Selenium;
using Selenium.WebControls.Commands;
using Selenium.WebControls.Environments;
using System;
using System.Collections.Generic;

namespace Selenium.WebControls.Constraints
{
    /// <summary>
    /// 
    /// </summary>
    public class All
    {
        /// <summary>
        /// 检查给定的上下文中，字符串集合的每个成员都包含期望值时返回true，否则返回false。
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<string>>, bool> Contains(params string[] keywords)
        {
            return delegate (AssertContext<IEnumerable<string>> context)
            {
                context.Command += "AllContains";
                context.Parameters.Add(string.Join(", ", keywords));
                if (!EnvManager.Auto) return true;
                foreach (string value in context.Data)
                {
                    if (!value.ContainsAll(keywords)) return false;
                }
                return true;
            };
        }

        /// <summary>
        /// 所有元素文本都包含给定值
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<IWebElement>>, bool> ContainsAny(params string[] keywords)
        {
            return delegate (AssertContext<IEnumerable<IWebElement>> context)
            {
                context.Command += "AllContainsAny";
                context.Parameters.Add(keywords);
                if (!EnvManager.Auto) return true;
                foreach (var element in context.Data)
                {
                    if (!element.Text.ContainsAny(keywords)) return false;
                }
                return true;
            };
        }
    }
}

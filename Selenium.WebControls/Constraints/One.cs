/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/14 22:53:11
 * ***********************************************/
using OpenQA.Selenium;
using Selenium.WebControls.Commands;
using Selenium.WebControls.Environments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Selenium.WebControls.Constraints
{
    /// <summary>
    /// 
    /// </summary>
    public class One
    {
        /// <summary>
        /// 所有元素中有一个包含给定值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<IWebElement>>, bool> Contains(string text)
        {
            return delegate (AssertContext<IEnumerable<IWebElement>> context)
            {
                context.Command += "OneContains";
                context.Parameters.Add(text);
                if (!EnvManager.Auto) return true;
                foreach (var element in context.Data)
                {
                    if (element.Text.Contains(text)) return true;
                }
                return false;
            };
        }

        /// <summary>
        /// 所有元素中有一个包含给定值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<IWebElement>>, bool> Equals(string text)
        {
            return delegate (AssertContext<IEnumerable<IWebElement>> context)
            {
                context.Command += "OneEquals";
                context.Parameters.Add(text);
                if (!EnvManager.Auto) return true;
                foreach (var element in context.Data)
                {
                    if (element.Text.Equals(text)) return true;
                }
                return false;
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<IWebElement>>, bool> StartWith(string text)
        {
            return delegate (AssertContext<IEnumerable<IWebElement>> context)
            {
                context.Command += "OneStartWith";
                context.Parameters.Add(text);
                if (!EnvManager.Auto) return true;
                foreach (var element in context.Data)
                {
                    if (element.Text.StartsWith(text)) return true;
                }
                return false;
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<IWebElement>>, bool> EndWith(string text)
        {
            return delegate (AssertContext<IEnumerable<IWebElement>> context)
            {
                context.Command += "OneEndWith";
                context.Parameters.Add(text);
                if (!EnvManager.Auto) return true;
                foreach (var element in context.Data)
                {
                    if (element.Text.EndsWith(text)) return true;
                }
                return false;
            };
        }
    }
}

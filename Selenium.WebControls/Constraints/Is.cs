/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/4 23:42:27
 * ***********************************************/
using OpenQA.Selenium;
using Selenium.WebControls.Commands;
using Selenium.WebControls.Environments;
using Selenium.WebControls.Extensions;
using System;
using System.Text.RegularExpressions;

namespace Selenium.WebControls.Constraints
{
    /// <summary>
    /// 
    /// </summary>
    public class Is
    {
        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定被测对象与期望等价
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static Func<AssertContext<T>, bool> Equals<T>(T expected)
        {
            return delegate (AssertContext<T> context)
            {
                context.Command += "Equals";
                context.Parameters.Add(expected);
                return EnvManager.Auto ? context.Data.Equals(expected) : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定文本值与期望值相等
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static Func<AssertContext, bool> Equals(string expected, bool ignoreCase = true)
        {
            return delegate (AssertContext context)
            {
                context.Command += "Equals";
                context.Parameters.Add(expected);
                return EnvManager.Auto ? context.Data.Equals(expected, ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture) : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定元素可见
        /// </summary>
        /// <returns></returns>
        public static Func<AssertContext<IWebElement>, bool> Visible()
        {
            return delegate (AssertContext<IWebElement> context)
            {
                context.Command += "Visible";
                return EnvManager.Auto ? context.Data.Displayed : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定元素可编辑
        /// </summary>
        /// <returns></returns>
        public static Func<AssertContext<IWebElement>, bool> Editable()
        {
            return delegate (AssertContext<IWebElement> context)
            {
                context.Command += "Editable";
                return EnvManager.Auto ? context.Data.Editable() : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定元素只读
        /// </summary>
        /// <returns></returns>
        public static Func<AssertContext<IWebElement>, bool> Readonly()
        {
            return delegate (AssertContext<IWebElement> context)
            {
                context.Command += "Readonly";
                return EnvManager.Auto ? !context.Data.Editable() : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定元素被禁用
        /// </summary>
        /// <returns></returns>
        public static Func<AssertContext<IWebElement>, bool> Disabled()
        {
            return delegate (AssertContext<IWebElement> context)
            {
                context.Command += "Disabled";
                return EnvManager.Auto ? context.Data.Disabled() : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定元素可以操作
        /// </summary>
        /// <returns></returns>
        public static Func<AssertContext<IWebElement>, bool> Enabled()
        {
            return delegate (AssertContext<IWebElement> context)
            {
                context.Command += "Enabled";
                return EnvManager.Auto ? context.Data.Enabled : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定文本值与指定的正则表达式匹配
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static Func<AssertContext, bool> Matches(string pattern)
        {
            return delegate (AssertContext context)
            {
                context.Command += "Matches";
                context.Parameters.Add(pattern);
                return EnvManager.Auto ? Regex.IsMatch(context.Data, pattern) : true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来被测对象是给定序列的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueList"></param>
        /// <returns></returns>
        public static Func<AssertContext<T>, bool> Within<T>(params T[] valueList)
        {
            return delegate (AssertContext<T> context)
            {
                context.Command += "Within";
                context.Parameters.Add(string.Join(", ", valueList));
                if (!EnvManager.Auto) return true;
                return context.Data.IsOneOf(valueList);
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用来断定元素存在
        /// </summary>
        /// <returns></returns>
        public static Func<AssertContext<IWebElement>, bool> Exist()
        {
            return delegate (AssertContext<IWebElement> context)
            {
                context.Command += "Exist";
                if (!EnvManager.Auto) return true;
                return context.Data != null;
            };
        }
    }
}

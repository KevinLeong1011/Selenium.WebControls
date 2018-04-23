/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/5 2:02:55
 * ***********************************************/
using OpenQA.Selenium;
using Selenium.WebControls.Commands;
using Selenium.WebControls.Environments;
using Selenium.WebControls.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.WebControls.Constraints
{
    /// <summary>
    /// 
    /// </summary>
    public class Not
    {
        /// <summary>
        /// 不等校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static Func<AssertContext<T>, bool> Equals<T>(T expected)
        {
            return delegate (AssertContext<T> context)
            {
                context.Command += "NotEquals";
                context.Parameters.Add(expected);
                return EnvManager.Auto ? !context.Data.Equals(expected) : true;
            };
        }

        /// <summary>
        /// 不等校验
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static Func<AssertContext, bool> Equals(string expected, bool ignoreCase = true)
        {
            return delegate (AssertContext context)
            {
                context.Command += "NotEquals";
                context.Parameters.Add(expected);
                if (!EnvManager.Auto) return true;
                return !context.Data.Equals(expected, 
                    ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
            };
        }

        /// <summary>
        /// 校验源数据字符串不包含期望值
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static Func<AssertContext, bool> Contains(string expected)
        {
            return delegate (AssertContext context)
            {
                context.Command += "NotContains";
                context.Parameters.Add(expected);
                return EnvManager.Auto ? !context.Data.Contains(expected) : true;
            };
        }

        /// <summary>
        /// 校验不包含
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<T>>, bool> Contains<T>(T expected)
        {
            return delegate (AssertContext<IEnumerable<T>> context)
            {
                context.Command += "NotContains";
                context.Parameters.Add(expected);
                return EnvManager.Auto ? !context.Data.Contains(expected) : true;
            };
        }

        /// <summary>
        /// 元素是否不可见
        /// </summary>
        /// <returns></returns>
        public static Func<AssertContext<IWebElement>, bool> Visible()
        {
            return delegate (AssertContext<IWebElement> context)
            {
                context.Command += "Invisible";
                return EnvManager.Auto ? !context.Data.Displayed : true;
            };
        }

        /// <summary>
        /// 返回一个委托，用来断定元素不存在
        /// </summary>
        /// <returns></returns>
        public static Func<AssertContext<IWebElement>, bool> Exist() => 
            delegate (AssertContext<IWebElement> context)
            {
                context.Command += "NotExist";
                return EnvManager.Auto ? context.Data != null : true;
            };

        /// <summary>
        /// 所有元素的文本内容都不包含给定的关键字
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<IWebElement>>, bool> Contains(params string[] keywords) =>
            delegate (AssertContext<IEnumerable<IWebElement>> context)
            {
                context.Command += "AllContains";
                context.Parameters.Add(string.Join(", ", keywords));
                if (!EnvManager.Auto) return true;
                foreach (IWebElement element in context.Data)
                {
                    string value = element.GetValue().Default(element.Text);
                    if (!value.ContainsAny(keywords)) return false;
                }
                return true;
            };

        /// <summary>
        /// 返回一个委托，用来断定数据不在给定的序列中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Func<AssertContext<T>, bool> In<T>(params T[] values)
        {
            return delegate (AssertContext<T> context)
            {
                context.Command += "NotIn";
                context.Parameters.Add(values);
                if (!EnvManager.Auto) return true;
                return !context.Data.IsOneOf(values);
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Func<AssertContext, bool> In(params string[] values)
        {
            return delegate (AssertContext context)
            {
                context.Command += "NotIn";
                context.Parameters.Add(string.Join(", ", values));
                if (!EnvManager.Auto) return true;
                return !context.Data.IsOneOf(values);
            };
        }
    }
}

/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/4 20:56:15
 * ***********************************************/
using Selenium.WebControls.Commands;
using Selenium.WebControls.Environments;
using System;
using System.Collections.Generic;

namespace Selenium.WebControls.Constraints
{
    /// <summary>
    /// 
    /// </summary>
    public class Contains
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<TData>>, bool> All<TData>(params TData[] keywords)
        {
            return delegate (AssertContext<IEnumerable<TData>> context)
            {
                context.Command += "ContainsAll";
                context.Parameters.Add(string.Join(", ", keywords));
                return EnvManager.Auto ? context.Data.ContainsAll(keywords) : true;
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<string>>, bool> All(params string[] keywords)
        {
            return delegate (AssertContext<IEnumerable<string>> context)
            {
                context.Command += "ContainsAll";
                context.Parameters.Add(string.Join(", ", keywords));
                if (!EnvManager.Auto) return true;
                return context.Data.ContainsAll(keywords);
            };
        }

        /// <summary>
        /// 包含任意一个
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<TData>>, bool> Any<TData>(params TData[] keywords)
        {
            return delegate (AssertContext<IEnumerable<TData>> context)
            {
                context.Command += "ContainsAny";
                context.Parameters.Add(string.Join(", ", keywords));
                if (!EnvManager.Auto) return true;
                return context.Data.ContainsAny(keywords);
            };
        }

        /// <summary>
        /// 包含任意一个
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<string>>, bool> Any(params string[] keywords)
        {
            return delegate (AssertContext<IEnumerable<string>> context)
            {
                context.Command += "ContainsAny";
                context.Parameters.Add(string.Join(", ", keywords));
                if (!EnvManager.Auto) return true;
                return context.Data.ContainsAny(keywords);
            };
        }
    }
}

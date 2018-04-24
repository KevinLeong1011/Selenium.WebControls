/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/14 1:36:32
 * ***********************************************/
using OpenQA.Selenium;
using Selenium.WebControls.Commands;
using Selenium.WebControls.Environments;
using Selenium.WebControls.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.WebControls.Constraints
{
    /// <summary>
    /// 
    /// </summary>
    public static class Ele
    {
        /// <summary>
        /// 返回一个委托<see cref="Func{T, TResult}"/>，用来断言给定元素是否拥有给定的class，其中class以空格隔开，允许不限顺序
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        public static Func<AssertContext<IWebElement>, bool> HasClass(string classes)
        {
            string[] classArray = classes.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return delegate (AssertContext<IWebElement> context)
            {
                context.Command += "HasClass";
                context.Parameters.Add(classes);
                if (!EnvManager.Auto) return true;
                return context.Data.HasClass(classArray);
            };
        }

        /// <summary>
        /// 返回一个委托<see cref="Func{T, TResult}"/>，用来断言给定元素集中每一个的文本或值都包含所有指定的关键字
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<IWebElement>>, bool> OneHaveAll(params string[] keywords)
        {
            return delegate (AssertContext<IEnumerable<IWebElement>> context)
            {
                context.Command += "OneHaveAll";
                context.Parameters.Add(string.Join(", ", keywords));
                if (!EnvManager.Auto) return true;
                var elements = context.Data;
                List<string> list = new List<string>();
                foreach (string keyword in keywords)
                {
                    var element = elements.Where(x =>
                    {
                        string textOrValue = x.Text.Default(x.GetValue());
                        return textOrValue.Contains(keyword);
                    }).FirstOrDefault();
                    if (element == null)
                    {
                        list.Add(keyword);
                    }
                }
                if (list.Count > 0)
                {
                    context.Message = $"The elements {context.DataName} have no keywords: {string.Join(", ", list)}";
                    return false;
                }
                return true;
            };
        }

        /// <summary>
        /// 返回一个委托<see cref="Func{T, TResult}"/>，用来断言给定元素集中每一个的文本或值都包含指定的关键字
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static Func<AssertContext<IEnumerable<IWebElement>>, bool> AllHave(params string[] keywords)
        {
            return delegate (AssertContext<IEnumerable<IWebElement>> context)
            {
                context.Command += "AllHave";
                context.Parameters.Add(string.Join(", ", keywords));
                if (!EnvManager.Auto) return true;
                List<string> list = new List<string>();
                foreach (var element in context.Data)
                {
                    string textOrValue = element.Text.Default(element.GetValue());
                    if (!textOrValue.ContainsAll(keywords)) list.Add(textOrValue);
                }

                if (list.Count > 0)
                {
                    context.Message = $"The value or text of the elements do not have all keywords '{string.Join(", ", keywords)}'. And their value or text: \n {string.Join("\n", list)}";
                    return false;
                }
                return true;
            };
        }
    }
}

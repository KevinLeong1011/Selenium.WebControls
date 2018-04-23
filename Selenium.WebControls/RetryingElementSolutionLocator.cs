/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 22:46:36
 * ***********************************************/
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Selenium.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    /// <summary>
    /// 多方案元素定位器。使用多个不同的<see cref="By"/>对象不断尝试定位元素，直到超时
    /// </summary>
    public class RetryingElementSolutionLocator
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromMilliseconds(100);

        private ISearchContext searchContext;
        private TimeSpan timeout;
        private TimeSpan pollingInterval;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="searchContext"></param>
        public RetryingElementSolutionLocator(ISearchContext searchContext)
            : this(searchContext, DefaultTimeout)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="searchContext"></param>
        /// <param name="timeout"></param>
        public RetryingElementSolutionLocator(ISearchContext searchContext, TimeSpan timeout)
            : this(searchContext, timeout, DefaultPollingInterval)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="searchContext"></param>
        /// <param name="timeout"></param>
        /// <param name="pollingInterval"></param>
        public RetryingElementSolutionLocator(ISearchContext searchContext, TimeSpan timeout, TimeSpan pollingInterval)
        {
            this.searchContext = searchContext;
            this.timeout = timeout;
            this.pollingInterval = pollingInterval;
        }

        /// <summary>
        /// 搜索上下文
        /// </summary>
        public ISearchContext SearchContext
        {
            get { return searchContext; }
        }

        /// <summary>
        /// Locates an element using the given solutions list of <see cref="By"/> criteria.
        /// 使用给定的<see cref="By"/>方案列表定位元素，如果超时仍未找到该元素，将抛出<see cref="NoSuchElementException"/>异常。
        /// </summary>
        /// <param name="bys">The list of methods by which to search for the element.</param>
        /// <returns>An <see cref="IWebElement"/> which is the first match under the desired criteria.</returns>
        public IWebElement LocateElement(IEnumerable<By> bys)
        {
            if (bys == null || bys.Count() == 0)
            {
                return null;
            }

            string errorString = null;
            DateTime endTime = DateTime.Now.Add(this.timeout);
            bool timeoutReached = DateTime.Now > endTime;
            while (!timeoutReached)
            {
                foreach (var by in bys)
                {
                    try
                    {
                        var elements = SearchContext.FindElements(by);
                        if (elements.Count == 1)
                        {
                            return elements[0];
                        }
                        var displayedEles = elements.Where(x => x.Displayed);
                        if (displayedEles.Count() == 1)
                        {
                            return displayedEles.ElementAt(0);
                        }
                        Thread.Sleep(100); // 防止Selenium出现“Element is not clickable at point”导致的点击问题
                    }
                    catch (NoSuchElementException)
                    {
                    }
                }

                timeoutReached = DateTime.Now > endTime;
                if (!timeoutReached)
                {
                    Thread.Sleep(this.pollingInterval);
                }
            }

            errorString = $"Could not find element by: " + string.Join(", or ", bys);

            throw new NoSuchElementException(errorString);
        }

        /// <summary>
        /// Locates an element using the given solutions list of <see cref="By"/> criteria.
        /// 使用给定的<see cref="By"/>方案列表定位元素，如果凭借其中一个方案可以成功定位，则立即返回结果；如果超时仍未找到该元素，将抛出<see cref="NoSuchElementException"/>异常。
        /// </summary>
        /// <param name="bys">The list of methods by which to search for the element.</param>
        /// <returns>An <see cref="IWebElement"/> which is the first match under the desired criteria.</returns>
        public ReadOnlyCollection<IWebElement> LocateElements(IEnumerable<By> bys)
        {
            if (bys == null || bys.Count() == 0)
            {
                return null;
            }

            List<IWebElement> collection = new List<IWebElement>();
            DateTime endTime = DateTime.Now.Add(this.timeout);
            bool timeoutReached = DateTime.Now > endTime;
            while (!timeoutReached)
            {
                foreach (var by in bys)
                {
                    try
                    {
                        var elements = SearchContext.FindElements(by);
                        Thread.Sleep(100); //防止出现点击问题
                        return elements;
                    }
                    catch (NoSuchElementException)
                    {

                    }
                }

                timeoutReached = collection.Count != 0 || DateTime.Now > endTime;
                if (!timeoutReached)
                {
                    Thread.Sleep(this.pollingInterval);
                }
            }

            return collection.AsReadOnly();
        }
    }
}

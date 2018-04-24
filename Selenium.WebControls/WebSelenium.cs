/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 22:45:26
 * ***********************************************/
using Selenium.WebControls.Controls;
using Selenium.WebControls.Environments;
using Selenium.WebControls.Extensions;
using Selenium.WebControls.Tracking;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Selenium.WebControls.Commands;
using OpenQA.Selenium.Support.Extensions;

namespace Selenium.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    public class WebSelenium
    {
        #region Constructors

        public WebSelenium()
        {

        }

        public WebSelenium(IWebDriver driver)
        {
            Driver = driver;
            CurrentContext = driver;
        }

        #endregion Constructors

        #region Attributes

        /// <summary>
        /// <see cref="IWebDriver"/>对象
        /// </summary>
        public IWebDriver Driver { get; private set; }

        /// <summary>
        /// 搜索上下文
        /// </summary>
        public virtual ISearchContext CurrentContext { get; private set; }

        #endregion Attributes

        #region Public Methods

        /// <summary>
        /// 从文件模板创建<see cref="WebControl"/>对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controlName"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public T Handle<T>(string controlName, By by = null)
            where T : WebControl<T>
        {
            T control = Activator.CreateInstance(typeof(T), this) as T;
            if (by != null) control.Locators.Add(by);
            if (!EnvManager.Auto) return control;
            string filePath = EnvManager.FindTemplateFile(controlName);
            string html = File.ReadAllText(filePath);
            control.ParseFromHtml(html);
            control.Name = controlName;
            ISearchContext context = Driver.RetryingSolutionLocator().LocateElement(control.Locators) ?? (Driver as ISearchContext);
            control.SetSearchContext(context);
            return control;
        }

        /// <summary>
        /// 执行前置条件
        /// </summary>
        /// <typeparam name="TAction"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public WebSelenium When<TAction>(params object[] args)
            where TAction : UserAction
        {
            List<object> list = new List<object>();
            list.Add(this);
            list.AddRange(args);
            TAction action = Activator.CreateInstance(typeof(TAction), list.ToArray()) as TAction;
            Tracker.Log(TrackTag.Condition, action.Description);
            if (EnvManager.Auto)
            {
                action.Run();
            }
            Tracker.Log(TrackTag.UserEnd, "");
            return this;
        }

        /// <summary>
        /// 执行前置条件
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public WebSelenium When(string desc, Action action)
        {
            Tracker.Log(TrackTag.Condition, desc);
            if (EnvManager.Auto)
            {
                action?.Invoke();
            }
            return this;
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <typeparam name="TAction"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public WebSelenium Do<TAction>(params object[] args)
            where TAction : UserAction
        {
            List<object> list = new List<object> { this };
            list.AddRange(args);
            TAction action = Activator.CreateInstance(typeof(TAction), list.ToArray()) as TAction;
            return Do(action.Description, () => action.Run());
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="description">行为描述</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public WebSelenium Do(string description, Action action)
        {
            Tracker.Log(TrackTag.UserActionStart, description);
            action?.Invoke();
            Tracker.Log(TrackTag.UserEnd, "");
            return this;
        }

        /// <summary>
        /// 执行校验
        /// </summary>
        /// <typeparam name="TAction"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public WebSelenium Check<TAction>(params object[] args)
            where TAction : UserAction
        {
            TAction action = Activator.CreateInstance(typeof(TAction), this, args) as TAction;
            Tracker.Log(TrackTag.Assert, action.Description);
            if (EnvManager.Auto)
            {
                action.Run();
            }
            Tracker.Log(TrackTag.UserEnd, "");
            return this;
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="description">校验描述</param>
        /// <param name="check">校验行为</param>
        /// <returns></returns>
        public WebSelenium Check(string description, Action check)
        {
            Tracker.Log(TrackTag.Assert, description);
            if (EnvManager.Auto)
            {
                check?.Invoke();
            }
            return this;
        }

        /// <summary>
        /// 根据ID查找<see cref="IWebElement"/>元素
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IWebElement FindElementById(string id)
        {
            return FindElement(By.Id(id));
        }

        /// <summary>
        /// 根据名称查找<see cref="IWebElement"/>元素
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual IWebElement FindElementByName(string name, int index = 1)
        {
            index = Math.Max(1, index);
            return FindElements(By.Name(name))[index - 1];
        }

        /// <summary>
        /// 根据<see cref="By"/>对象查找<see cref="IWebElement"/>元素
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public virtual IWebElement FindElement(By by)
        {
            return CurrentContext.RetryingSolutionLocator().LocateElement(new By[] { by });
        }

        /// <summary>
        /// Finds all <see cref="IWebElement"/> within the current context using the given mechanism.
        /// 使用指定的定位方式查找当前上下文中的所有<see cref="IWebElement"/>
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public virtual ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return CurrentContext.RetryingSolutionLocator().LocateElements(new By[] { by });
        }

        /// <summary>
        /// 进入Frame
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public virtual WebSelenium EnterFrame(By by)
        {
            Driver.SwitchTo().Frame(Driver.RetryingSolutionLocator().LocateElement(new By[] { by }));
            return this;
        }

        /// <summary>
        /// 离开当前Frame
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public virtual WebSelenium ExitFrame(By by)
        {
            Driver.SwitchTo().ParentFrame();
            return this;
        }

        /// <summary>
        /// Click the element.
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public virtual WebSelenium Click(By by)
        {
            FindElement(by).Click();
            return this;
        }

        /// <summary>
        /// Click link.
        /// 点击链接
        /// </summary>
        /// <param name="linkText"></param>
        /// <returns></returns>
        public virtual WebSelenium ClickLink(string linkText)
        {
            FindElement(By.PartialLinkText(linkText)).Click();
            return this;
        }

        /// <summary>
        /// Double click element.
        /// 双击元素
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public virtual WebSelenium DoubleClick(By by)
        {
            IWebElement element = FindElement(by);
            element.Click();
            element.Click();
            return this;
        }

        public virtual WebSelenium DragDrop(By source, By target)
        {
            IWebElement dragElement = FindElement(source);
            IWebElement dropElement = FindElement(target);
            new Actions(Driver).DragAndDrop(dragElement, dropElement).Perform();
            return this;
        }

        public virtual WebSelenium DragDrop(By by, int x, int y)
        {
            IWebElement element = FindElement(by);
            Actions actionBuilder = new Actions(Driver);
            actionBuilder.DragAndDropToOffset(element, x, y).Perform();
            return this;
        }

        public virtual WebSelenium Input(By by, string value, bool append = false)
        {
            IWebElement element = FindElement(by);
            if (!append)
            {
                element.Clear();
            }
            element.SendKeys(value);
            return this;
        }

        public virtual WebSelenium ClearValue(By by)
        {
            FindElement(by).Clear();
            return this;
        }

        public virtual bool Editable(By by)
        {
            IWebElement element = FindElement(by);
            return element.Editable();
        }

        public virtual bool IsChecked(By by)
        {
            IWebElement element = FindElement(by);
            if (element.TagName == "input" && element.GetAttribute("type") == "checkbox")
            {
                return element.GetAttribute("checked") != null;
            }
            throw new InvalidSelectorException("Not checkbox.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public virtual string GetValue(By by)
        {
            return FindElement(by).GetValue();
        }

        public virtual string GetInnerHtml(By by)
        {
            return FindElement(by).GetInnerHtml();
        }

        public virtual WebSelenium Open(string url)
        {
            Driver.Navigate().GoToUrl(url);
            return this;
        }

        public virtual WebSelenium Forward()
        {
            Driver.Navigate().Forward();
            return this;
        }

        public virtual WebSelenium Back()
        {
            Driver.Navigate().Back();
            return this;
        }

        public virtual WebSelenium Refresh()
        {
            Driver.Navigate().Refresh();
            return this;
        }

        public virtual WebSelenium Maximize()
        {
            Driver.Manage().Window.Maximize();
            return this;
        }

        public virtual WebSelenium Minimize()
        {
            Driver.Manage().Window.Minimize();
            return this;
        }

        public virtual void Close()
        {
            Driver.Close();
        }

        public virtual void Quit()
        {
            Driver.Quit();
        }


        #endregion Public Methods

        #region Fields



        #endregion Fields




    }
}

/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:43:43
 * ***********************************************/
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Selenium.WebControls.Commands;
using Selenium.WebControls.Environments;
using Selenium.WebControls.Exceptions;
using Selenium.WebControls.Extensions;
using Selenium.WebControls.Tracking;
using Selenium.WebControls.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace Selenium.WebControls.Controls
{
    /// <summary>
    /// 控件类
    /// </summary>
    public class WebControl : WebControl<WebControl>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="selenium"></param>
        public WebControl(WebSelenium selenium) :
            base(selenium)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WebControl<T>
        where T : WebControl<T>
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="selenium"></param>
        protected WebControl(WebSelenium selenium)
        {
            Selenium = selenium;
        }

        #endregion Constructors

        #region Attributes

        /// <summary>
        /// 控件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 控件定位值
        /// </summary>
        public List<By> Locators { get; private set; } = new List<By>();

        /// <summary>
        /// <see cref="WebSelenium"/>对象
        /// </summary>
        protected WebSelenium Selenium { get; private set; }

        /// <summary>
        /// 断言对象（依赖注入）
        /// </summary>
        protected IAssert Assert => DataFactory.ResolveAssert<IAssert>();

        /// <summary>
        /// 标记列表
        /// </summary>
        protected Dictionary<string, Mark> Marks { get; } = new Dictionary<string, Mark>();

        #endregion Attributes

        #region Public Methods

        #region  Utils

        /// <summary>
        /// 设置元素搜索上下文
        /// </summary>
        /// <param name="context"></param>
        public void SetSearchContext(ISearchContext context)
        {
            SolutionLocator = context.RetryingSolutionLocator();
        }

        /// <summary>
        /// 从html中解析标记
        /// </summary>
        /// <param name="html"></param>
        public void ParseFromHtml(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode root = doc.DocumentNode.FirstChild;
            if (!string.IsNullOrWhiteSpace(root.Id))
            {
                Locators.Add(By.Id(root.Id));
            }
            if (!root.IsW3cNode())
            {
                Locators.Add(By.TagName(root.Name));
            }
            if (root.Attributes["name"] != null)
            {
                Locators.Add(By.Name(root.Attributes["name"].Value));
            }
            if (root.HasAttr("class"))
            {
                string classes = root.Attributes["class"].Value;
                string cssSelector = classes.Trim().Replace(" ", ".").Replace(" ", "");
                Locators.Add(By.CssSelector($"{root.Name}.{cssSelector}"));
                Locators.Add(By.XPath($"//{root.Name}[@class='{classes}']"));
            }
            HtmlNodeExtensions.ParseMarks(html).ForEach(m => Marks.Put(m.Value, m));
        }

        /// <summary>
        /// 处理模板中嵌套的组件标记，并返回相应的<see cref="WebControl"/>对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public WebControl Handle(string name)
        {
            return Handle<WebControl>(name);
        }

        /// <summary>
        /// 处理模板中嵌套的组件标记，并返回相应的<see cref="WebControl{T}"/>对象
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public TControl Handle<TControl>(string name)
            where TControl : WebControl<TControl>
        {
            TControl control = Activator.CreateInstance(typeof(TControl), Selenium) as TControl;
            control.Name = name;
            if (!EnvManager.Auto) return control;

            Mark mark = Marks.GetValueOrDefault(name);
            if (mark == null || mark.Type != MarkType.Component)
            {
                // TODO 记录日志
                throw new MarkTypeException($"Cannot find the mark of template which name is {name}.");
            }
            string filePath = EnvManager.FindTemplateFile(name);
            string html = File.ReadAllText(filePath);
            control.ParseFromHtml(html);
            control.Locators.AddRange(mark.Locators);
            control.SetSearchContext(SolutionLocator.LocateElement(control.Locators));
            return control;
        }

        /// <summary>
        /// 使用模板处理模板中给定名称的元素
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tplName"></param>
        /// <returns></returns>
        public WebControl Handle(string name, string tplName)
        {
            return Handle<WebControl>(name, tplName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="name"></param>
        /// <param name="tplName"></param>
        /// <returns></returns>
        public TControl Handle<TControl>(string name, string tplName)
            where TControl : WebControl<TControl>
        {
            TControl control = Activator.CreateInstance(typeof(TControl), Selenium) as TControl;
            control.Name = name;
            if (!EnvManager.Auto) return control;
            Mark mark = Marks.GetValueOrDefault(name);
            if (mark == null || mark.Type != MarkType.Element)
            {
                // TODO 记录日志
                throw new MarkTypeException($"Cannot find the mark of template which name is {name}.");
            }
            string filePath = EnvManager.FindTemplateFile(tplName);
            string html = File.ReadAllText(filePath);
            control.ParseFromHtml(html);
            control.Locators.AddRange(mark.Locators);
            control.SetSearchContext(SolutionLocator.LocateElement(control.Locators));
            return control;
        }

        /// <summary>
        /// 处理模板中的组件标记
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public T Handle<TControl>(string name, Action<TControl> action)
            where TControl : WebControl<TControl>
        {
            return Handle<TControl>(name, name, action);
        }

        /// <summary>
        /// 对模板中的元素使用指定的模板来处理
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="name"></param>
        /// <param name="tplName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public T Handle<TControl>(string name, string tplName, Action<TControl> action)
            where TControl : WebControl<TControl>
        {
            TControl control = Activator.CreateInstance(typeof(TControl), Selenium) as TControl;
            control.Name = name;
            if (!EnvManager.Auto)
            {
                action?.Invoke(control);
                return this as T;
            }

            string filePath = EnvManager.FindTemplateFile(tplName);
            string html = File.ReadAllText(filePath);
            control.ParseFromHtml(html);
            Mark mark = Marks.GetValueOrDefault(name);
            if (mark == null)
            {
                throw new MarkNotFoundException(name);
            }
            control.Locators.AddRange(mark.Locators);
            control.SetSearchContext(SolutionLocator.LocateElement(control.Locators));
            action?.Invoke(control);
            return this as T;
        }

        /// <summary>
        /// 执行测试行为
        /// </summary>
        /// <param name="description">行为描述</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public T Do(string description, Action<T> action)
        {
            T @this = this as T;
            Tracker.Log(TrackTag.UserActionStart, description);
            action?.Invoke(@this);
            Tracker.Log(TrackTag.UserEnd, "");
            return @this;
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="description">校验描述</param>
        /// <param name="check">校验行为</param>
        /// <returns></returns>
        public T Check(string description, Action<T> check)
        {
            T @this = this as T;
            Tracker.Log(TrackTag.UserAssertStart, description);
            check?.Invoke(@this);
            Tracker.Log(TrackTag.UserEnd,"");
            return @this;
        }

        #endregion

        #region Info

        /// <summary>
        /// 获取给定名称的<see cref="IWebElement"/>元素对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual IWebElement FindElement(string name)
        {
            if (!EnvManager.Auto) return null;
            Mark mark = Marks.GetValueOrDefault(name);
            if (mark == null || mark.Type != MarkType.Element) return null;
            string[] markArray = mark.Name.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            var locator = this.SolutionLocator;
            if (markArray.Length < 2) return locator.LocateElement(mark.ClusterLocators);
            IWebElement result = null;
            foreach (string markName in markArray)
            {
                Mark m = Marks.Where(x => x.Value.Name == markName).FirstOrDefault().Value;
                if (m == null) throw new Exception($"Please make sure you have marked an element with \"{EnvManager.MarkPrefix}-{markName}\".");
                result = locator.LocateElement(m.Locators);
                locator = result.RetryingSolutionLocator();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual List<IWebElement> FindElements(string name)
        {
            if (!EnvManager.Auto) return new List<IWebElement>();
            Mark mark = Marks.GetValueOrDefault(name);
            if (mark == null) return null;
            string[] markArray = mark.Name.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);


            if (markArray.Length < 2) return SolutionLocator.LocateElements(mark.ClusterLocators).ToList();
            List<IWebElement> elements = new List<IWebElement>();
            foreach (string markName in markArray)
            {
                List<IWebElement> temp = new List<IWebElement>();
                Mark m = Marks.Where(x => x.Value.Name == markName).FirstOrDefault().Value;
                if (m == null) throw new Exception($"Please make sure you have marked an element with \"{EnvManager.MarkPrefix}-{markName}\".");
                if (elements.Count == 0)
                {
                    elements.AddRange(SolutionLocator.LocateElements(m.ClusterLocators));
                }
                else
                {
                    foreach (var element in elements)
                    {
                        var eles = element.RetryingSolutionLocator().LocateElements(m.ClusterLocators);
                        elements.AddRange(eles);
                        elements.Remove(element);
                    }
                }
            }
            return elements;
        }

        ///// <summary>
        ///// 定位并获取元素
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public virtual IWebElement FindElement(string name)
        //{
        //    if (!EnvManager.Auto) return null;
        //    Mark mark = GetMark(name);
        //    return Locator.LocateElement(mark.Locators);
        //}

        ///// <summary>
        ///// 定位并获取元素
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public virtual ReadOnlyCollection<IWebElement> FindElements(string name)
        //{
        //    if (!EnvManager.Auto) return null;
        //    Mark mark = GetMark(name);
        //    return Locator.LocateElements(mark.Locators);
        //}

        /// <summary>
        /// 获取元素值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValue(string name)
        {
            if (!EnvManager.Auto) return null;
            var value = FindElement(name).GetValue();
            return value;
        }

        /// <summary>
        /// 用于获取未在模板中标记的元素的值
        /// </summary>
        /// <param name="name">元素命名</param>
        /// <param name="by"></param>
        /// <returns></returns>
        public string GetValue(string name, By by)
        {
            if (!EnvManager.Auto) return null;
            var result = SolutionLocator.LocateElement(new By[] { by }).GetValue();
            return result;
        }

        /// <summary>
        /// 获取元素文本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetText(string name)
        {
            if (!EnvManager.Auto) return null;
            var value = FindElement(name).Text;
            return value;
        }

        /// <summary>
        /// 用于获取未在模板中标记的元素的文本值
        /// </summary>
        /// <param name="name">元素命名</param>
        /// <param name="by"></param>
        /// <returns></returns>
        public string GetText(string name, By by)
        {
            if (!EnvManager.Auto) return null;
            var result = SolutionLocator.LocateElement(new By[] { by }).Text;
            return result;
        }

        /// <summary>
        /// 获取元素值或文本中包含的数值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double GetNumber(string name)
        {
            if (!EnvManager.Auto) return 0;
            double[] numbers = FindElement(name).GetValue().GetNumbers();
            if (numbers.Length == 0)
            {
                throw new Exception($"名称为{name}的元素文本中不包含数值");
            }
            return numbers[0];
        }

        /// <summary>
        /// 获取指定名称的元素个数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int Count(string name)
        {
            if (!EnvManager.Auto) return 0;
            return FindElements(name).Count;
        }

        #endregion

        #region Actions

        /// <summary>
        /// 点击一个或多个元素
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public virtual T Click(params string[] names)
        {
            TrackAction("Click", string.Join(", ", names));
            IfAuto(() =>
            {
                foreach (string name in names)
                {
                    FindElement(name).Click();
                }
            });
            return this as T;
        }

        /// <summary>
        /// 右键元素
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual T RightClick(string name)
        {
            TrackAction("RightClick", name);
            IfAuto(() =>
            {
                new Actions(Selenium.Driver).ContextClick(FindElement(name));
            });
            return this as T;
        }

        /// <summary>
        /// 双击一个或多个元素
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public virtual T DoubleClick(params string[] names)
        {
            TrackAction("DoubleClick", string.Join(", ", names));
            IfAuto(() =>
            {
                foreach (string name in names)
                {
                    FindElement(name).DoubleClick();
                }
            });
            return this as T;
        }

        /// <summary>
        /// 向指定名称的控件输入值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        public virtual T Input(string name, string value, bool append = false)
        {
            TrackAction("Input", name, value);
            IfAuto(() =>
            {
                var element = FindElement(name);
                if (!append) element.Clear();
                element.SendKeys(value);
            });
            return this as T;
        }

        /// <summary>
        /// 清空输入框
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual T ClearValue(string name)
        {
            TrackAction("ClearValue", name);
            IfAuto(() => FindElement(name).Clear());
            return this as T;
        }

        /// <summary>
        /// 点击指定名称的元素集中包含相应内容的元素
        /// </summary>
        /// <param name="name"></param>
        /// <param name="keyword"></param>
        /// <param name="extraKeywords"></param>
        /// <returns></returns>
        public T ClickItem(string name, string keyword, params string[] extraKeywords)
        {
            TrackAction("ClickItemByText", name, keyword + (extraKeywords.Length > 0 ? ", " : "") + string.Join(", ", extraKeywords));
            if (!EnvManager.Auto) return this as T;
            var elements = FindElements(name);
            foreach (var element in elements)
            {
                if (element.HasContent(keyword) && element.HasContent(extraKeywords))
                {
                    element.Click();
                    break;
                }
            }
            return this as T;
        }

        /// <summary>
        /// 点击所有文本或值包含关键字的元素
        /// </summary>
        /// <param name="name"></param>
        /// <param name="keyword"></param>
        /// <param name="extraKeywords"></param>
        /// <returns></returns>
        public T ClickItems(string name, string keyword, params string[] extraKeywords)
        {
            TrackAction("ClickItemsByText", name, keyword + (extraKeywords.Length > 0 ? ", " : "") + string.Join(", ", extraKeywords));
            if (!EnvManager.Auto) return this as T;
            var elements = FindElements(name);
            foreach (var element in elements)
            {
                if (element.HasContent(keyword) && element.HasContent(extraKeywords))
                {
                    element.Click();
                }
            }
            return this as T;
        }

        /// <summary>
        /// 根据索引点击给定名称的元素集中的元素
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public T ClickItem(string name, int index)
        {
            TrackAction("ClickItemByIndex", name, index);
            if (!EnvManager.Auto) return this as T;
            var elements = FindElements(name);
            if (elements.Count == 0 || index > elements.Count)
            {
                // TODO: 输入Warning日志
                return this as T;
            }
            elements[Math.Max(1, index)].Click();
            return this as T;
        }

        /// <summary>
        /// 找出所有文本或值包含给定关键字的元素
        /// </summary>
        /// <param name="name"></param>
        /// <param name="keyword"></param>
        /// <param name="extraKeywords"></param>
        /// <returns></returns>
        public IEnumerable<IWebElement> FindItems(string name, string keyword, params string[] extraKeywords)
        {
            if (!EnvManager.Auto) yield break;
            var elements = FindElements(name);
            foreach (var element in elements)
            {
                if (element.HasContent(keyword) && element.HasContent(extraKeywords))
                {
                    yield return element;
                }
            }
            yield break;
        }

        /// <summary>
        /// 鼠标悬停于某元素之上，默认3秒
        /// </summary>
        /// <param name="name"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public T MouseOver(string name, int time = 3)
        {
            TrackAction("MouseOver", name, time);
            IfAuto(() =>
            {
                var element = FindElement(name);
                new Actions(Selenium.Driver).MoveToElement(element);
                Thread.Sleep(time * 1000);
            });
            return this as T;
        }

        /// <summary>
        /// 拖放元素
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="targetName"></param>
        /// <returns></returns>
        public virtual T DragDrop(string sourceName, string targetName)
        {
            TrackAction("DragDrop", sourceName, targetName);
            IfAuto(() =>
            {
                IWebElement source = FindElement(sourceName);
                IWebElement target = FindElement(targetName);
                new Actions(Selenium.Driver).DragAndDrop(source, target);
            });
            return this as T;
        }

        /// <summary>
        /// 按照偏移量拖放元素
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual T DragDropToOffset(string sourceName, int x, int y)
        {
            TrackAction("DragDropToOffset", sourceName, x, y);
            if (!EnvManager.Auto) return this as T;
            IWebElement source = FindElement(sourceName);
            new Actions(Selenium.Driver).DragAndDropToOffset(source, x, y);
            return this as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="controlName"></param>
        /// <param name="targetName"></param>
        /// <returns></returns>
        public T DragDropToAnotherControl(string sourceName, string controlName, string targetName)
        {
            TrackAction("DragDropToAnotherControl", sourceName, controlName, targetName);
            if (!EnvManager.Auto) return this as T;
            IWebElement source = FindElement(sourceName);
            IWebElement target = Selenium.Handle<WebControl>(controlName).FindElement(targetName);
            new Actions(Selenium.Driver).DragAndDrop(source, target);
            return this as T;
        }

        /// <summary>
        /// 勾选给定名称的元素
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Tick(string name)
        {
            TrackAction("Tick", name);
            if (!EnvManager.Auto) return this as T;
            IWebElement element = FindElement(name);
            if (element.TagName.ToUpper() == "INPUT" && element.GetAttribute("type").ToLower() == "checkbox")
            {
                if (!element.Selected) element.Click();
            }
            return this as T;
        }

        /// <summary>
        /// 取消勾选给定名称的元素
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Untick(string name)
        {
            TrackAction("Tick", name);
            if (!EnvManager.Auto) return this as T;
            IWebElement element = FindElement(name);
            if (element.TagName.ToUpper() == "INPUT" && element.GetAttribute("type").ToLower() == "checkbox")
            {
                if (element.Selected) element.Click();
            }
            return this as T;
        }

        #endregion

        #region Assert

        /// <summary>
        /// 断言
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="actual"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public virtual T Ensure<TData>(NamedData<TData> actual, Func<AssertContext<TData>, bool> validate)
        {
            return Ensure<TData>(() => actual, validate);
        }

        /// <summary>
        /// 断言
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="getter"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public virtual T Ensure<TData>(Func<NamedData<TData>> getter, Func<AssertContext<TData>, bool> validate)
        {
            return Ensure<TData>(c => getter(), validate);
        }

        /// <summary>
        /// 断言
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="getter"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public virtual T Ensure<TData>(Func<T, NamedData<TData>> getter, Func<AssertContext<TData>, bool> validate)
        {
            AssertContext<TData> context = new AssertContext<TData>();
            T @this = this as T;
            getter.ThrowIfNull("getter");
            var data = getter(@this);
            context.Parameters.Add(data.Name);
            context.Data = data.Value;
            IfAuto(() =>
            {
                if (!validate(context))
                {
                    Assert.Fail(context.Message);
                }
            });
            TrackAssertion(context.Command, context.Parameters.ToArray());
            return this as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getter"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public virtual T Ensure(Func<T, NamedData> getter, Func<AssertContext, bool> validate)
        {
            AssertContext context = new AssertContext();
            T @this = this as T;
            getter.ThrowIfNull("getter");
            var data = getter(@this);
            context.Parameters.Add(data.Name);
            context.Data = data.Value;
            if (!validate(context))
            {
                Assert.Fail(context.Message);
            }
            TrackAssertion(context.Command, context.Parameters.ToArray());
            return this as T;
        }

        /// <summary>
        /// 保证元素符合给定的限定条件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public virtual T EnsureElements(string name, Func<AssertContext<IEnumerable<IWebElement>>, bool> validate)
        {
            AssertContext<IEnumerable<IWebElement>> context = new AssertContext<IEnumerable<IWebElement>>();
            T @this = this as T;
            context.Command += "Elements";
            context.Parameters.Add(name);
            context.Data = FindElements(name);
            if (!validate(context))
            {
                Assert.Fail();
            }
            TrackAssertion(context.Command, context.Parameters.ToArray());
            return this as T;
        }

        /// <summary>
        /// 保证元素符合给定的限定条件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public virtual T EnsureElement(string name, Func<AssertContext<IWebElement>, bool> validate)
        {
            AssertContext<IWebElement> context = new AssertContext<IWebElement>();
            T @this = this as T;
            context.Command += "Element";
            context.Parameters.Add(name);
            context.Data = FindElement(name);
            if (!validate(context))
            {
                Assert.Fail(context.Message);
            }
            TrackAssertion(context.Command, context.Parameters.ToArray());
            return this as T;
        }

        /// <summary>
        /// 保证元素的值符合给定的限定条件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public virtual T EnsureValue(string name, Func<AssertContext, bool> validate)
        {
            AssertContext context = new AssertContext();
            T @this = this as T;
            context.Command += "Value";
            context.Parameters.Add(name);
            context.Data = GetValue(name);
            if (!validate(context))
            {
                Assert.Fail(context.Message);
            }
            TrackAssertion(context.Command, context.Parameters.ToArray());
            return this as T;
        }

        /// <summary>
        /// 保证元素文本符合给定的限定条件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public virtual T EnsureText(string name, Func<AssertContext, bool> validate)
        {
            AssertContext context = new AssertContext();
            T @this = this as T;
            context.Command += "Text";
            context.Parameters.Add(name);
            context.Data = GetText(name);
            if (!validate(context))
            {
                Assert.Fail(context.Message);
            }
            TrackAssertion(context.Command, context.Parameters.ToArray());
            return this as T;
        }

        /// <summary>
        /// 等待n秒
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public virtual T Wait(int n)
        {
            Thread.Sleep(n * 1000);
            return this as T;
        }

        #endregion

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// 跟踪测试行为
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        protected void TrackAction(string cmd, params object[] args)
        {
            Tracker.Log(TrackTag.Action, CommandDescriptions.Format(cmd, args));
        }

        /// <summary>
        /// 跟踪校验
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        protected void TrackAssertion(string cmd, params object[] args)
        {
            Tracker.Log(TrackTag.Assert, CommandDescriptions.Format(cmd, args));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 自动化模式下运行<see cref="Action"/>
        /// </summary>
        /// <param name="action"></param>
        protected void IfAuto(Action action)
        {
            if (EnvManager.Auto)
            {
                action?.Invoke();
            }
        }

        protected Mark GetMark(string name)
        {
            Mark mark = Marks.GetValueOrDefault(name);
            if (mark == null) throw new MarkNotFoundException(name);
            return mark;
        }

        #endregion

        #region Fields

        //private IWebElement selfElement;

        /// <summary>
        /// 方案定位器
        /// </summary>
        protected RetryingElementSolutionLocator SolutionLocator { get; set; }

        #endregion Fields
    }
}

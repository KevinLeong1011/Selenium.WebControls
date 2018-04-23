/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 22:45:26
 * ***********************************************/
using HtmlAgilityPack;
using OpenQA.Selenium;
using Selenium.WebControls.Controls;
using Selenium.WebControls.Environments;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Selenium.WebControls.Extensions
{
    /// <summary>
    /// <see cref="HtmlNode"/>扩展
    /// </summary>
    public static class HtmlNodeExtensions
    {
        /// <summary>
        /// 获取<see cref="HtmlNode"/>的所有可能的定位方式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<By> GetLocators(this HtmlNode node)
        {
            List<By> list = new List<By>();
            if (!string.IsNullOrWhiteSpace(node.Id))
            {
                list.Add(By.Id(node.Id));
            }
            if (!node.IsW3cNode())
            {
                list.Add(By.TagName(node.Name));
            }
            if (node.HasAttr("name"))
            {
                list.Add(By.Name(node.Attributes["name"].Value));
            }
            if (node.HasAttr("class"))
            {
                string classes = node.Attributes["class"].Value;
                string cssSelector = classes.Trim().Replace(" ", ".").Replace(" ", "");
                list.Add(By.CssSelector($"{node.Name}.{cssSelector}"));
                list.Add(By.XPath($"//{node.Name}[@class='{classes}']"));
            }

            HtmlNode current = node;
            string xpath = "";
            string css = "";
            while (current.ParentNode != null && current.ParentNode.Name != "#document")
            {
                if (!string.IsNullOrWhiteSpace(current.Id) && current != node)
                {
                    string xpathLocator = $"//{current.Name}[@id='{current.Id}']{xpath}";
                    string cssLocator = $" {current.Name}#{current.Id}{css}";
                    list.Add(By.XPath(xpathLocator));
                    list.Add(By.CssSelector(cssLocator));
                }

                if (current.Name.ToLower() == "input" && current.Attributes["type"].Value == "text" && current.Attributes["placeholder"] != null)
                {
                    string xpath_placeholder = $"//{current.Name}[@placeholder='{current.Attributes["placeholder"].Value}']{xpath}";
                    list.Add(By.XPath(xpath_placeholder));
                }
                var eles = current.ParentNode.Elements(current.Name);
                int count = 1;
                foreach (HtmlNode ele in eles)
                {
                    if (ele == current) break;
                    count++;
                }
                xpath = $"/{ current.Name }[{count}]" + xpath;
                css = $" {current.Name}:nth-child({count}){css}";
                current = current.ParentNode;
            }
            list.Add(By.XPath($"//{current.Name}{xpath}"));
            list.Add(By.CssSelector($"{current.Name}{css}"));
            return list;
        }

        /// <summary>
        /// 获取<see cref="HtmlNode"/>的所有可能的簇定位方式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<By> GetClusterLocators(this HtmlNode node)
        {
            List<By> list = new List<By>();
            HtmlNode current = node;
            string xpath = "";
            string css = "";
            while (current.ParentNode != null && current.ParentNode.Name != "#document")
            {
                if (!string.IsNullOrWhiteSpace(current.Id) && current != node)
                {
                    string xpathLocator = $"//{current.Name}[@id='{current.Id}']{xpath}";
                    string cssLocator = $" {current.Name}#{current.Id}{css}";
                    list.Add(By.XPath(xpathLocator));
                    list.Add(By.CssSelector(cssLocator));
                }
                var eles = current.ParentNode.Elements(current.Name);
                int count = 1;
                foreach (HtmlNode ele in eles)
                {
                    if (ele == current) break;
                    count++;
                }
                xpath = xpath == "" ? $"/{current.Name}" : $"/{ current.Name }[{count}]" + xpath;
                css = css == "" ? $" {current.Name}" : $" {current.Name}:nth-child({count}){css}";
                current = current.ParentNode;
            }
            list.Add(By.XPath($"//{current.Name}{xpath}"));
            list.Add(By.CssSelector($"{current.Name}{css}"));
            return list;
        }

        /// <summary>
        /// 判断<see cref="HtmlNode"/>是否W3C标准标签
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsW3cNode(this HtmlNode node)
        {
            return "div,span,a,ul,li,ol,h1,h2,h3,h4,h5,h6,table,tbody,thead, tr,td,th,button,input,select,option,img,iframe,textarea,i,b,form".IndexOf(node.Name.ToLower()) > -1;
        }

        /// <summary>
        /// 判断节点是否有给定属性
        /// </summary>
        /// <param name="this"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasAttr(this HtmlNode @this, string name)
        {
            return @this.Attributes[name] != null;
        }

        /// <summary>
        /// 从html中解析标记
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static List<Mark> ParseMarks(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode root = doc.DocumentNode.FirstChild;
            NodeWrapper wrapper = new NodeWrapper { Node = root };
            wrapper.XpathList.Add($"//{root.Name}");
            wrapper.CssList.Add($"{root.Name}");
            return GetMarks(wrapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        public static List<Mark> GetMarks(NodeWrapper wrapper)
        {
            List<Mark> marks = new List<Mark>();
            HtmlNode node = wrapper.Node;
            if (!node.Id.NullOrWhiteSpace())
            {
                wrapper.XpathList.Add($"//{node.Name}[@id='{node.Id}']");
                wrapper.CssList.Add($"{node.Name}#{node.Id}");
            }
            foreach (HtmlNode child in node.ChildNodes)
            {
                bool hasMark = false;
                Mark mark = new Mark();
                if (!child.Id.NullOrWhiteSpace())
                {
                    mark.Locators.Add(By.Id(child.Id));
                }
                if (!child.IsW3cNode())
                {
                    mark.Locators.Add(By.TagName(child.Name));
                }
                if (child.HasAttr("name"))
                {
                    mark.Locators.Add(By.Name(child.Attributes["name"].Value));
                }
                if (child.HasAttr("class"))
                {
                    string classes = child.Attributes["class"].Value;
                    string cssSelector = classes.Trim().Replace(" ", ".").Replace(" ", "");
                    mark.Locators.Add(By.CssSelector($"{child.Name}.{cssSelector}"));
                    mark.Locators.Add(By.XPath($"//{child.Name}[@class='{classes}']"));
                    mark.ClusterLocators.Add(By.CssSelector($"{child.Name}.{cssSelector}"));
                    mark.ClusterLocators.Add(By.XPath($"//{child.Name}[@class='{classes}']"));
                }
                foreach (var attr in child.Attributes)
                {
                    if (attr.Name.StartsWith(EnvManager.MarkPrefix))
                    {
                        hasMark = true;
                        mark.Name = attr.Name.Replace($"{EnvManager.MarkPrefix}-", "");
                        mark.Value = attr.Value;
                    }
                }
                NodeWrapper childWrapper = new NodeWrapper() { Node = child };
                string childXpath = child.XPath;
                foreach (string xpath in wrapper.XpathList)
                {
                    string xpathValue = $"{xpath}{childXpath.Substring(childXpath.LastIndexOf('/'))}";
                    childWrapper.XpathList.Add(xpathValue);
                    if (hasMark)
                    {
                        mark.Locators.Add(By.XPath(xpathValue));
                        mark.ClusterLocators.Add(By.XPath(xpathValue.Substring(0, xpathValue.LastIndexOf('['))));
                        childWrapper.XpathList.Clear();
                        childWrapper.XpathList.Add($"//{child.Name}"); // 重新开始，以便计算后续标记节点的相对定位值
                    }
                }
                foreach (string css in wrapper.CssList)
                {
                    string xpathValue = $"{css}{childXpath.Substring(childXpath.LastIndexOf('/'))}";
                    string cssValue = xpathValue.Replace("/", " ").Replace("[", ":nth-child(").Replace("]", ")").Trim();
                    childWrapper.CssList.Add(cssValue);
                    if (hasMark)
                    {
                        mark.Locators.Add(By.CssSelector(cssValue));
                        mark.ClusterLocators.Add(By.CssSelector(cssValue.Substring(0, cssValue.LastIndexOf(':'))));
                        childWrapper.CssList.Clear();
                        childWrapper.CssList.Add($"{child.Name}"); // 重新开始，以便计算后续标记节点的相对定位值 
                    }
                }
                if (hasMark) marks.Add(mark); // 将当前标记节点计入MarkList
                marks.AddRange(GetMarks(childWrapper));
            }
            return marks;
        }
    }
}

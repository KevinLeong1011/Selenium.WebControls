using HtmlAgilityPack;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Selenium.WebControls.Test
{
    public class UnitTest1
    {
        public void TestMethod1()
        {
            var marks = C.ParseMarks("" +
                "<div>" +
                   "<form tpl-login=\"��¼\" id=\"login\">"+
                     "<div id=\"udiv\">" +
                        "<input id=\"uname\" tpl-login-item=\"�û���\" />" +
                     "</div>" +
                     "<div>" +
                        "<input id=\"pwd\" tpl-login-item=\"����\" />" +
                     "</div>" +
                   "</form>"+
                "</div>");         
        }
    }


    public static class C
    {
        public static bool HasAttr(this HtmlNode @this, string name)
        {
            return @this.Attributes[name] != null;
        }

        /// <summary>
        /// ��html�н������
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

        public static bool IsW3cNode(this HtmlNode node)
        {
            return "div,span,a,ul,li,ol,h1,h2,h3,h4,h5,h6,table,tbody,thead, tr,td,th,button,input,select,option,img,iframe,textarea,i,b,form".IndexOf(node.Name.ToLower()) > -1;
        }

        public static List<Mark> GetMarks(NodeWrapper wrapper)
        {
            List<Mark> marks = new List<Mark>();
            HtmlNode node = wrapper.Node;
            if (!string.IsNullOrWhiteSpace(node.Id))
            {
                wrapper.XpathList.Add($"//{node.Name}[@id='{node.Id}']");
                wrapper.CssList.Add($"{node.Name}#{node.Id}");
            }
            foreach (HtmlNode child in node.ChildNodes)
            {
                bool hasMark = false;
                Mark mark = new Mark();
                if (!string.IsNullOrWhiteSpace(child.Id))
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
                    if (attr.Name.StartsWith("tpl"))
                    {
                        hasMark = true;
                        mark.Name = attr.Name.Replace($"tpl-", "");
                        mark.ElementName = attr.Value;
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
                        childWrapper.XpathList.Add($"//{child.Name}"); // ���¿�ʼ���Ա���������ǽڵ����Զ�λֵ 
                    }
                }
                foreach(string css in wrapper.CssList)
                {
                    string xpathValue = $"{css}{childXpath.Substring(childXpath.LastIndexOf('/'))}";
                    string cssValue = xpathValue.Replace("/", " ").Replace("[", ":nth-child(").Replace("]", ")").Trim();
                    childWrapper.CssList.Add(cssValue);
                    if (hasMark)
                    {
                        mark.Locators.Add(By.CssSelector(cssValue));
                        mark.ClusterLocators.Add(By.CssSelector(cssValue.Substring(0, cssValue.LastIndexOf(':'))));
                        childWrapper.CssList.Clear();
                        childWrapper.CssList.Add($"{child.Name}"); // ���¿�ʼ���Ա���������ǽڵ����Զ�λֵ 
                    }
                }
                if (hasMark) marks.Add(mark); // ����ǰ��ǽڵ����MarkList

                marks.AddRange(GetMarks(childWrapper));
            }
            return marks;
        }
    }

    public class Mark
    {
        public Mark()
        {

        }

        /// <summary>
        /// The name of element mark
        /// �������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicate whether the element is multiple in web control.
        /// �Ƿ�����Ԫ��
        /// </summary>
        public bool Multiple { get; set; }

        /// <summary>
        /// Ԫ������
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// Ԫ�ض�λ��ʽ�б�
        /// </summary>
        public List<By> Locators { get; set; } = new List<By>();

        /// <summary>
        /// Ԫ�صĴض�λ��ʽ�б�
        /// </summary>
        public List<By> ClusterLocators { get; set; } = new List<By>();

        /// <summary>
        /// ǰһ��Mark���
        /// </summary>
        public Mark Prev { get; set; }
    }

    public class NodeWrapper
    {
        public HtmlNode Node { get; set; }

        public List<string> XpathList { get; set; } = new List<string>();

        public List<string> CssList { get; set; } = new List<string>();
    }
}

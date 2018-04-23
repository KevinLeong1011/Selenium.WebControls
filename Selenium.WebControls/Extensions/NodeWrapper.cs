/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/13 0:32:02
 * ***********************************************/
using HtmlAgilityPack;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.WebControls.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class NodeWrapper
    {
        public HtmlNode Node { get; set; }

        public List<string> XpathList { get; set; } = new List<string>();

        public List<string> CssList { get; set; } = new List<string>();
    }
}

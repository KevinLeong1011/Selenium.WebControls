/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:43:43
 * ***********************************************/
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Selenium.WebControls.Controls
{
    /// <summary>
    /// The mark of element.
    /// 元素标记
    /// </summary>
    public class Mark
    {
        public Mark()
        {

        }

        /// <summary>
        /// The name of element mark
        /// 标记名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标记类型
        /// </summary>
        public MarkType Type => Name.NullOrEmpty() ? MarkType.Component : MarkType.Element;

        /// <summary>
        /// 标记值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 元素定位方式列表
        /// </summary>
        public List<By> Locators { get; set; } = new List<By>();

        /// <summary>
        /// 元素的簇定位方式列表
        /// </summary>
        public List<By> ClusterLocators { get; set; } = new List<By>();

        public Mark Prev { get; set; }
    }
}
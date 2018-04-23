/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/8 23:53:43
 * ***********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.WebControls.Constraints
{
    /// <summary>
    /// 约束返回结果，用于循环控制逻辑中
    /// </summary>
    public class LoopResult
    {
        /// <summary>
        /// 最终的期望结果
        /// </summary>
        public bool Expected { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// 循环选择
        /// </summary>
        public LoopChoice Choice { get; set; }
    }

    /// <summary>
    /// 循环控制选择
    /// </summary>
    public enum LoopChoice
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 继续
        /// </summary>
        Continue,
        /// <summary>
        /// 跳出
        /// </summary>
        Break
    }
}

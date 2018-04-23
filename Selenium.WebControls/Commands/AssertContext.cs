/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/5 3:11:34
 * ***********************************************/
using System.Collections.Generic;

namespace Selenium.WebControls.Commands
{
    /// <summary>
    /// 文本数据的Assert上下文
    /// </summary>
    public class AssertContext : AssertContext<string>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AssertContext():base()
        {
        }
    }

    /// <summary>
    /// Assert上下文
    /// </summary>
    public class AssertContext<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AssertContext()
        {
            Command = "Assert";
            Parameters = new List<object>();
        }

        /// <summary>
        /// 所要校验的数据名称
        /// </summary>
        public string DataName { get; set; }

        /// <summary>
        /// 所要校验的数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 校验命令
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 用于得到参数
        /// </summary>
        public List<object> Parameters { get; private set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}

/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/5 21:52:26
 * ***********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.WebControls.Controls
{
    /// <summary>
    /// 命名的字符串数据
    /// </summary>
    public class NamedData : NamedData<string>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public NamedData(string name, string value) : base(name, value)
        {
        }
    }

    /// <summary>
    /// 命名数据
    /// </summary>
    public class NamedData<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public NamedData(string name, T value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// 数据名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 数据值
        /// </summary>
        public T Value { get; }
    }
}

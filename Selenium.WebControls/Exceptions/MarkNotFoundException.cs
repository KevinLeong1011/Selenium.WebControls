/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:43:43
 * ***********************************************/
using System;
using System.Runtime.Serialization;

namespace Selenium.WebControls.Exceptions
{
    /// <summary>
    /// 标记未找到异常
    /// </summary>
    [Serializable]
    public class MarkNotFoundException : Exception
    {
        public MarkNotFoundException()
        {
        }

        public MarkNotFoundException(string message) : base(message)
        {
        }

        public MarkNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MarkNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
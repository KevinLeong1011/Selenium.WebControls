/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/26 23:43:43
 * ***********************************************/
using System;
using System.Runtime.Serialization;

namespace Selenium.WebControls.Exceptions
{
    [Serializable]
    public class MarkTypeException : Exception
    {
        public MarkTypeException()
        {
        }

        public MarkTypeException(string message) : base(message)
        {
        }

        public MarkTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MarkTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/8 2:23:43
 * ***********************************************/
using System.Text;

namespace Selenium.WebControls.CaseGeneration
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexStringBuilder
    {
        public IndexStringBuilder()
        {
        }

        private StringBuilder builder = new StringBuilder();
        private int count = 1;

        public IndexStringBuilder AppendLine(string line)
        {
            builder.AppendLine($"{count}.{line}");
            count++;
            return this;
        }

        public IndexStringBuilder AppendFormat(string format, string line)
        {
            builder.AppendFormat($"{count}.{string.Format(format, line)}");
            count++;
            return this;
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }
}

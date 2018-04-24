/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/8 23:56:39
 * ***********************************************/
using Selenium.WebControls.Commands;
using Selenium.WebControls.Environments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.WebControls.Constraints
{
    /// <summary>
    /// 
    /// </summary>
    public class Table
    {
        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用于断定给定的数据表中能够找到表达式限定的数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static Func<AssertContext<DataTable>, bool> HasData(string filter)
        {
            return delegate (AssertContext<DataTable> context)
            {
                context.Command += "HasData";
                context.Parameters.Add(filter);
                if (!EnvManager.Auto)return true;
                return context.Data.Select(filter).Length > 0;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用于断定给定的数据表中找不到表达式限定的数据
        /// </summary>
        /// 参见<param name="filter"><see cref="DataTable.Select(string)"/></param>
        /// <returns></returns>
        public static Func<AssertContext<DataTable>, bool> NoData(string filter)
        {
            return delegate (AssertContext<DataTable> context)
            {
                context.Command += "HasNoData";
                context.Parameters.Add(filter);
                if (!EnvManager.Auto) return true;
                return context.Data.Select(filter).Length == 0;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用于断定给定的数据表表头包含给定项
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public static Func<AssertContext<DataTable>, bool> HasColumns(params string[] names)
        {
            return delegate (AssertContext<DataTable> context)
            {
                context.Command += "HasColumns";
                context.Parameters.Add(string.Join(", ", names));
                if (!EnvManager.Auto)
                    return true;
                DataTable table = context.Data;
                List<string> list = new List<string>();
                foreach (string name in names)
                {
                    DataColumn col = table.Columns[name];
                    if (col == null)
                    {
                        list.Add(name);

                    }
                }
                if (list.Count > 0)
                {
                    context.Message = $"Cannot find these columns: {string.Join(", ", list)}";
                }

                return list.Count == 0;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用于断定给定的数据表排序符合规则，暂不支持多字段
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Func<AssertContext<DataTable>, bool> SortBy(string expression)
        {
            return delegate (AssertContext<DataTable> context)
            {
                context.Command += "SortBy";
                context.Parameters.Add(expression);
                if (!EnvManager.Auto) return true;
                DataTable table = context.Data;
                string[] array = expression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string field = array[0];
                bool asc = array.Length == 0 || array[1].ToLower() == "asc" ? true : false;
                DataRow pre = null;
                int preIndex = 0;
                int curIndex = 1;
                foreach (DataRow row in table.Rows)
                {
                    curIndex++;
                    if (pre == null)
                    {
                        pre = row;
                        preIndex++;
                    }
                    else
                    {
                        int result = pre[field].ToString().CompareTo(row[field].ToString());
                        if ((result > 0 && asc) || (result < 0 && !asc))
                        {
                            context.Message = $"Data are not sorted correct: {preIndex} and {curIndex}"; // TODO
                            return false;
                        }
                        pre = row;
                    }
                }
                return true;
            };
        }

        /// <summary>
        /// 返回一个<see cref="Func{T, TResult}"/>委托，用于断定给定的数据表中所有数据都符合条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Func<AssertContext<DataTable>,bool> AllMatches(string condition)
        {
            return delegate (AssertContext<DataTable> context)
            {
                context.Command += "AllMatches";
                context.Parameters.Add(condition);
                if (!EnvManager.Auto) return true;
                var table = context.Data;
                var rows = table.Select(condition);
                return rows.Length == table.Rows.Count;
            };
        }
    }
}

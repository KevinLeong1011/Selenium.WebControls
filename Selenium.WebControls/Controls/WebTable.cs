/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/3/27 1:57:38
 * ***********************************************/
using OpenQA.Selenium;
using Selenium.WebControls.Commands;
using Selenium.WebControls.Constraints;
using Selenium.WebControls.Environments;
using Selenium.WebControls.Extensions;
using Selenium.WebControls.Tracking;
using System;
using System.Collections.Generic;
using System.Data;

namespace Selenium.WebControls.Controls
{
    /// <summary>
    /// 数据表
    /// </summary>
    public class WebTable : WebControl<WebTable>
    {
        #region Constructors

        /// <summary>
        /// <see cref="WebTable"/> 构造函数
        /// </summary>
        /// <param name="selenium"></param>
        public WebTable(WebSelenium selenium) :
            base(selenium)
        {
        }

        #endregion Constructors

        #region Attributes

        /// <summary>
        /// 复选框列号
        /// </summary>
        public int CheckboxColumnIndex { get; private set; } = 1;

        #endregion Attributes

        #region Public Methods

        /// <summary>
        /// 设置复选框的列号
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public WebTable SetCheckboxColIndex(int i)
        {
            CheckboxColumnIndex = Math.Max(1, i);
            return this;
        }

        /// <summary>
        /// 从表中找出对应条件的数据，并点击对应列的单元格
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public WebTable ClickCell(string filter, string colName)
        {
            TrackAction("TableClickCell", filter, colName);
            if (!EnvManager.Auto) return this;
            DataRow[] rows = FindData(filter);
            foreach (var row in rows)
            {
                int index = table.Rows.IndexOf(row);
                int colIndex = table.Columns.IndexOf(colName);
                SolutionLocator.LocateElement(new By[] { By.CssSelector($"table tbody tr:nth-child({index + 1}) td:nth-child({colIndex + 1})") }).Click();
            }
            return this;
        }

        /// <summary>
        /// 对于表中符合条件的数据，勾选给定字段
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public WebTable Tick(string filter, params string[] fields)
        {
            TrackAction("TableTickFields", filter, string.Join(", ", fields));
            if (!EnvManager.Auto) return this;
            DataRow[] rows = FindData(filter);
            List<int> colIndexList = new List<int>();
            foreach (string field in fields)
            {
                colIndexList.Add(table.Columns.IndexOf(field) + 1);
            }
            foreach (var row in rows)
            {
                int index = table.Rows.IndexOf(row);
                foreach (int colIndex in colIndexList)
                {
                    SolutionLocator.LocateElement(new By[] { By.CssSelector($"table tbody tr:nth-child({index + 1}) td:nth-child({colIndex})") }).Tick();
                }
            }
            return this;
        }

        /// <summary>
        /// 对于表中符合条件的数据，不勾选给定字段
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public WebTable Untick(string filter, params string[] fields)
        {
            TrackAction("TableTickFields", filter, string.Join(", ", fields));
            if (!EnvManager.Auto) return this;
            DataRow[] rows = FindData(filter);
            List<int> colIndexList = new List<int>();
            foreach (string field in fields)
            {
                colIndexList.Add(table.Columns.IndexOf(field) + 1);
            }
            foreach (var row in rows)
            {
                int index = table.Rows.IndexOf(row);
                foreach (int colIndex in colIndexList)
                {
                    SolutionLocator.LocateElement(new By[] { By.CssSelector($"table tbody tr:nth-child({index + 1}) td:nth-child({colIndex})") }).Untick();
                }
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual DataRow SelectData(int index)
        {
            DataTable table = GetPageData();
            TrackAction("TableSelectDataByIndex", index);
            if (!EnvManager.Auto) return null;
            if (table.Rows.Count > 0)
            {
                if (index > table.Rows.Count) throw new IndexOutOfRangeException($"There are not {index} data in the table.");
                SolutionLocator.LocateElement(new By[] { By.CssSelector($"table tbody tr:nth-child({Math.Max(1, index)}) td:nth-child({CheckboxColumnIndex})") }).Click();
                return table.Rows[Math.Max(0, index - 1)];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public virtual DataRow[] SelectData(string filter, int n)
        {
            List<DataRow> list = new List<DataRow>();
            TrackAction("TableSelectDataByFilter", filter, n);
            if (!EnvManager.Auto) return null;
            DataTable table = GetPageData();
            if (table.Rows.Count > 0)
            {
                if (n > table.Rows.Count) throw new IndexOutOfRangeException($"There are not {n} data in the table.");
                DataRow[] rows = table.Select(filter);
                int i = 0;
                foreach (DataRow row in rows)
                {
                    int index = table.Rows.IndexOf(row);
                    SolutionLocator.LocateElement(new By[] { By.CssSelector($"table tbody tr:nth-child({Math.Max(0, index)}) td:nth-child({CheckboxColumnIndex})") }).Click();
                    i++;
                    list.Add(row);
                    if (i >= n)
                    {
                        break;
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual DataRow[] SelectAllData(string filter)
        {
            TrackAction("TableSelectAllDataByFilter", filter);
            if (!EnvManager.Auto) return null;
            DataTable table = GetPageData();
            if (table.Rows.Count > 0)
            {
                DataRow[] rows = table.Select(filter);
                foreach (DataRow row in rows)
                {
                    int index = table.Rows.IndexOf(row);
                    SolutionLocator.LocateElement(new By[] { By.CssSelector($"table tbody tr:nth-child({Math.Max(0, index)}) td:nth-child({CheckboxColumnIndex})") }).Click();
                }
                return rows;
            }
            return null;
        }

        /// <summary>
        /// 获取指定索引行的数据，从1开始
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual DataRow FindData(int index)
        {
            if (!EnvManager.Auto) return null;
            DataTable table = GetPageData();
            if (index < 0 || index > table.Rows.Count) throw new IndexOutOfRangeException("index");
            return table.Rows[Math.Max(1, index)];
        }

        /// <summary>
        /// 从表中找出一条符合条件的数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual DataRow[] FindData(string filter)
        {
            DataTable table = GetPageData();
            var rows = table.Select(filter);
            return rows;
        }

        /// <summary>
        /// 从表中找出符合给定条件的数据的索引，从1开始
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual int FindIndex(string filter)
        {
            if (!EnvManager.Auto) return -1;
            DataTable table = GetPageData();
            DataRow[] rows = table.Select(filter);
            if (rows.Length > 0)
            {
                return table.Rows.IndexOf(rows[0]) + 1;
            }
            return -1;
        }

        /// <summary>
        /// 确保数据符合给定的规则
        /// </summary>
        /// <param name="validate"></param>
        /// <returns></returns>
        public virtual WebTable EnsureData(Func<AssertContext<DataTable>, bool> validate)
        {
            AssertContext<DataTable> context = new AssertContext<DataTable>();
            context.Command += "Table";
            DataTable table = GetPageData();
            context.Data = table;
            var result = validate(context);

            if (!result)
            {
                Assert.Fail(context.Message);
            }
            TrackAssertion(context.Command, context.Parameters.ToArray());
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validate"></param>
        /// <returns></returns>
        public virtual WebTable EnsureHead(Func<AssertContext<DataTable>, bool> validate)
        {
            AssertContext<DataTable> context = new AssertContext<DataTable>();
            context.Command += "Table";

            if (table == null) ParseStructure();
            context.Data = table;
            var result = validate(context);
            if (!result)
            {
                Assert.Fail(context.Message);
            }
            TrackAssertion(context.Command, context.Parameters.ToArray());
            return this;
        }

        /// <summary>
        /// 获取当前页的数据
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetPageData()
        {
            if (!EnvManager.Auto) return null; // 如果非自动化运行，应该返回null，以防止出现异常
            if (table == null) ParseStructure();
            table.Clear();
            var rows = SolutionLocator.LocateElements(new By[] { By.CssSelector("table tbody tr") });
            foreach (var row in rows)
            {
                DataRow dr = table.NewRow();
                var cols = row.FindElements(By.CssSelector("td"));
                int i = 0;
                List<string> colValues = new List<string>();
                foreach (var col in cols)
                {
                    string colValue = col.Text;
                    colValues.Add(colValue);
                    i++;
                }
                dr.ItemArray = colValues.ToArray();
                table.Rows.Add(dr);
            }
            return table;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// 解析表结构
        /// </summary>
        protected void ParseStructure()
        {
            if (!EnvManager.Auto) return;
            if (table == null) table = new DataTable();
            //var cols = SolutionLocator.LocateElements(new List<By> { By.CssSelector("table thead tr th") });
            var cols = SolutionLocator.LocateElements(new List<By> { By.CssSelector("table thead tr th") });
            foreach (var col in cols)
            {
                string colName = col.Text;
                if (colName.NullOrWhiteSpace()) table.Columns.Add(Guid.NewGuid().ToString()); // 列头名称为空，使用自己字符串占位
                else table.Columns.Add(colName, typeof(string));
            }
        }

        #endregion

        #region Fields

        private DataTable table;

        #endregion Fields
    }
}

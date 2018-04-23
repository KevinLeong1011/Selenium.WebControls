/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/8 1:10:27
 * ***********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Selenium.WebControls.Environments;
using Selenium.WebControls.Tracking;

namespace Selenium.WebControls.CaseGeneration
{
    /// <summary>
    /// 
    /// </summary>
    public class NormalCaseGenerator : ICaseGenerator
    {
        private IWorkbook workbook;
        private ISheet sheet;
        private Dictionary<string, int> colDict = new Dictionary<string, int>();
        private string fileName;
        public NormalCaseGenerator()
        {
            JObject obj = JObject.Parse(File.ReadAllText(@"Configs\CaseGeneration.json"));

            string template = IOHelper.Parse(obj["Normal"]["Template"].ToString());
            string extension = Path.GetExtension(template);
            fileName = $"{EnvManager.AppName}标准测试用例-{DateTime.Now.ToString("yyyyMMddhhmmss")}{extension}";
            File.Copy(template, Path.Combine(IOHelper.Parse("测试用例"), fileName));
            FileStream stream = File.OpenWrite(fileName);
            if (extension == ".xlsx")
                workbook = new XSSFWorkbook(stream);
            else if (extension == ".xls")
                workbook = new HSSFWorkbook(stream);
            stream.Close();
            workbook.ThrowIfNull("workbook");
            string sheetName = obj["Normal"]["SheetName"].ToString();
            sheet = workbook.GetSheet(sheetName);
            sheet.ThrowIfNull("sheet");
            int rowCount = sheet.LastRowNum;
            IRow row = sheet.GetRow(obj["Normal"]["HeadRowIndex"].ToObject<int>());

            int colCount = row.PhysicalNumberOfCells;
            for (int i = 0; i < colCount; i++)
            {
                if (row.GetCell(i).StringCellValue == obj["Normal"]["CaseName"].ToObject<string>())
                {
                    colDict.Add("CaseName", i);
                }
                if (row.GetCell(i).StringCellValue == obj["Normal"]["Precondition"].ToObject<string>())
                {
                    colDict.Add("Precondition", i);
                }
                if (row.GetCell(i).StringCellValue == obj["Normal"]["Steps"].ToObject<string>())
                {
                    colDict.Add("Steps", i);
                }
                if (row.GetCell(i).StringCellValue == obj["Normal"]["Expectation"].ToObject<string>())
                {
                    colDict.Add("Expectation", i);
                }
            }
        }

        private int currentRow = 1; // 当前行
        private int caseNameCol = 1;
        private int preconditionCol;
        private int stepsCol;
        private int expectationCol;

        public void Write(List<TrackText> logs)
        {
            string caseName = "";
            IndexStringBuilder conditions = new IndexStringBuilder();
            IndexStringBuilder steps = new IndexStringBuilder();
            IndexStringBuilder expectations = new IndexStringBuilder();
            bool userAction = false;
            foreach (TrackText text in logs)
            {
                if (text.Tag == TrackTag.UserActionStart)
                {
                    userAction = true;
                    steps.AppendLine(text.Text);
                }
                if (text.Tag == TrackTag.Case)
                {
                    caseName = text.Text;
                }
                else if (text.Tag == TrackTag.Condition)
                {
                    conditions.AppendLine(text.Text);
                }
                else if (text.Tag == TrackTag.Action && !userAction)
                {
                    steps.AppendLine(text.Text);
                }
                else if (text.Tag == TrackTag.Assert && !userAction)
                {
                    expectations.AppendLine(text.Text);
                }
                else if (text.Tag == TrackTag.UserEnd)
                {
                    userAction = false;
                }
            }

            IRow row = sheet.GetRow(currentRow);
            ICell cell = row.GetCell(caseNameCol);
            cell.SetCellValue(caseName);

            cell = row.GetCell(preconditionCol);
            cell.SetCellValue(conditions.ToString());

            cell = row.GetCell(stepsCol);
            cell.SetCellValue(steps.ToString());

            cell = row.GetCell(expectationCol);
            cell.SetCellValue(expectations.ToString());

            currentRow++;
            var fs = File.OpenWrite(fileName);
            workbook.Write(fs);
            fs.Close();
        }
    }
}

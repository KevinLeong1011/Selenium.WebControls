/* ***********************************************
 * Author : Kevin
 * Function : 
 * Created : 2018/4/8 1:11:05
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
    public class AgileCaseGenerator : ICaseGenerator
    {
        private IWorkbook workbook;
        private ISheet sheet;
        private Dictionary<string, int> colDict = new Dictionary<string, int>();
        private string fileName;
        public AgileCaseGenerator()
        {
            JObject obj = JObject.Parse(File.ReadAllText(@"Configs\CaseGeneration.json"));

            string template = IOHelper.Parse(obj["Agile"]["Template"].ToString());
            string extension = Path.GetExtension(template);
            fileName = $"{EnvManager.AppName}敏捷测试用例-{DateTime.Now.ToString("yyyyMMddhhmmss")}{extension}";
            File.Copy(template, Path.Combine(IOHelper.Parse("测试用例"), fileName));
            FileStream fs = File.OpenWrite(fileName);
            if (extension == ".xlsx")
                workbook = new XSSFWorkbook(fs);
            else if (extension == ".xls")
                workbook = new HSSFWorkbook(fs);
            fs.Close();
            workbook.ThrowIfNull("workbook");
            string sheetName = obj["Agile"]["SheetName"].ToString();
            sheet = workbook.GetSheet(sheetName);
            sheet.ThrowIfNull("sheet");
            int rowCount = sheet.LastRowNum;
            IRow row = sheet.GetRow(obj["Agile"]["HeadRowIndex"].ToObject<int>());

            int colCount = row.PhysicalNumberOfCells;
            for (int i = 0; i < colCount; i++)
            {
                if (row.GetCell(i).StringCellValue == obj["Normal"]["CaseName"].ToObject<string>())
                {
                    colDict.Add("CaseName", i);
                }
                if (row.GetCell(i).StringCellValue == obj["Normal"]["CaseDesc"].ToObject<string>())
                {
                    colDict.Add("CaseDesc", i);
                }
            }
        }

        private int currentRow = 1; // 当前行

        public void Write(List<TrackText> logs)
        {
            string caseName = "";
            List<string> conditions = new List<string>();
            List<string> steps = new List<string>();
            List<string> expectations = new List<string>();
            bool userAction = false;
            foreach (TrackText text in logs)
            {
                if (text.Tag == TrackTag.UserActionStart)
                {
                    userAction = true;
                    steps.Add(text.Text);
                }
                if (text.Tag == TrackTag.Case)
                {
                    caseName = text.Text;
                }
                else if (text.Tag == TrackTag.Condition)
                {
                    conditions.Add(text.Text);
                }
                else if (text.Tag == TrackTag.Action && !userAction)
                {
                    steps.Add(text.Text);
                }
                else if (text.Tag == TrackTag.Assert && !userAction)
                {
                    expectations.Add(text.Text);
                }
                else if (text.Tag == TrackTag.UserEnd)
                {
                    userAction = false;
                }
            }

            IRow row = sheet.GetRow(currentRow);
            ICell cell = row.GetCell(colDict["CaseName"]);
            cell.SetCellValue(caseName);

            cell = row.GetCell(colDict["CaseDesc"]);
            string desc = $"假如{string.Join("，", conditions)}，{string.Join("，", steps)}，于是，{string.Join("，", expectations)}";
            cell.SetCellValue(desc);

            currentRow++;

            var fs = File.OpenWrite(fileName);
            workbook.Write(fs);
            fs.Close();
        }
    }
}

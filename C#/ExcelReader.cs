// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelManager.cs" company="Newegg" Author="oz3t">
//   Copyright (c) 2016 Newegg.inc. All rights reserved.
// </copyright>
// <summary>
//   ExcelManager created at  10/8/2016 3:06:39 PM
// </summary>
//<Description>
//
//</Description>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using SP.Common.Batch.Handler;
using SP.Common.Utility;
using System.Reflection;

namespace SP.Common.Excel
{
    public class ExcelManager
    {
        private AsposeHandle handler;

        static ExcelManager()
        {
            if (File.Exists(string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, "Aspose.Cells.lic")))
            {
                Aspose.Cells.License license = new Aspose.Cells.License();
                license.SetLicense("Aspose.Cells.lic");
            }
        }

        /// <summary>
        /// excelFile Excel文件的路径(本地路径)
        /// </summary>
        public ExcelManager(string excelFile) 
        {
            try
            {
                handler = new AsposeHandle(SafeFile.GetExtension(excelFile)) { CurrentFileName = excelFile };
                handler.OpenFile();
            }
            catch (Exception e)
            {
                handler.Close();
                throw e;
            }
        }

        /// <summary>
        /// 返回实体数据
        /// </summary>
        public List<T> GetListFromExcel<T>(WorkSheetInfo info) where T : new() 
        {
            var result = new List<T>();

            var titleArray = GetExcelColumnName(info);
            var excelData = GetExcelContent(info);

            var propertInfoList = typeof(T).GetProperties();
            foreach (var item in excelData)
            {
                T temp = new T();
                foreach (var property in propertInfoList)
                {
                    if (!property.CanWrite) continue;
                    var excelMappingStr = property.Name;
                    var tempAttribute = property.GetCustomAttributes(typeof(ExcelMapAttribute), false);
                    if (tempAttribute.Length > 0) {
                        var mapAttribute = (ExcelMapAttribute)tempAttribute[0];
                        if (mapAttribute.IsIgnore) continue;
                        excelMappingStr = mapAttribute.Mapping;
                    }
                    if (!titleArray.ToList().Exists(excel => string.Equals(excel, excelMappingStr, StringComparison.OrdinalIgnoreCase)))
                        throw new Exception("Column  name is invalid");

                    SetPropertyValue(property, temp, GetDictionaryValue(item, excelMappingStr));
                }
                result.Add(temp);
            }
            return result;
        }


        /// <summary>
        /// 返回Excel的列名
        /// </summary>
        private string[] GetExcelColumnName(WorkSheetInfo info)
        {
            handler.SetCurrentWorksheet(info.WorkSheetIndex);
            var titleArray = new string[info.TitleCount];
            //读取Excel的Title
            for (int titleColumn = info.TitleStartColumnIndex, i = 0; titleColumn < info.TitleStartColumnIndex + info.TitleCount; titleColumn++)
            {
                var columnName = handler.GetCellValue(info.TitleStartRowIndex, titleColumn);
                if (columnName == null || string.IsNullOrWhiteSpace(columnName.ToString()))
                    throw new Exception("Template is invalid");

                titleArray[i++] = columnName.ToString();
            }
            return titleArray;
        }

        /// <summary>
        /// 读取Excel数据
        /// </summary>
        private List<Dictionary<string, string>> GetExcelContent(WorkSheetInfo info)
        {   
            handler.SetCurrentWorksheet(info.WorkSheetIndex);
            var titleArray = GetExcelColumnName(info);

            int rowCount = handler.RowCount;
            if (info.ReadRowCount > 0)
                rowCount = info.ReadRowCount;

            //读取数据到Dictionary
            var result = new List<Dictionary<string, string>>();
            for (int row = info.ContentRowStartIndex; row <= rowCount; row++)
            {
                var temp = new Dictionary<string, string>(info.TitleCount);
                for (int column = info.ContentColumnStartIndex, i = 0; column < info.ContentColumnStartIndex + info.TitleCount; column++)
                {
                    var cellValue = handler.GetCellValue(row, column);
                    temp.Add(titleArray[i++], cellValue == null ? string.Empty : cellValue.ToString().Trim());
                }
                result.Add(temp);
            }

            handler.Close();
            return result;
        }

        /// <summary>
        /// 时间有限，没有使用统一的方法去转换（如传入泛型）， 还有其它类型的转换可以加在后面
        /// </summary>
        private void SetPropertyValue<T>(PropertyInfo property, T temp, string value) where T : new()
        {
            if (property.PropertyType == typeof(int))
                property.SetValue(temp, ParseInt(value), null);
            else if (property.PropertyType == typeof(decimal))
                property.SetValue(temp, ParseDecimal(value), null);
            else if (property.PropertyType == typeof(string))
                property.SetValue(temp, string.IsNullOrWhiteSpace(value)? string.Empty : value.Trim(), null);
        }

        private int ParseInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;
            var tempValue = value.Replace("," , "");
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }

        private decimal ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;
            var tempValue = value.Replace(",", "");
            decimal result = 0;
            decimal.TryParse(value, out result);
            return result;
        }

        private string GetDictionaryValue(Dictionary<string, string> excelDic, string key)
        {
            if (!excelDic.ContainsKey(key))
                return null;
            return excelDic[key];
        }
    }
}


namespace SP.Common.Excel
{
    public class WorkSheetInfo
    {
        /// <summary>
        /// 一般操作的Sheet少，就没有使用string 作为index
        /// 要操作的工作sheet(第一个为1,第二个为2)
        /// </summary>
        public int WorkSheetIndex { get; set; }

        /// <summary>
        /// 列头的行开始位置(从1开始)
        /// </summary>
        public int TitleStartRowIndex { get; set; }

        /// <summary>
        /// 列头的列开始位置(从1开始)
        /// </summary>
        public int TitleStartColumnIndex { get; set; }

        /// <summary>
        /// 列头个数
        /// </summary>
        public int TitleCount { get; set; }

        /// <summary>
        /// 内容行开始位置(从1开始)
        /// </summary>
        public int ContentRowStartIndex { get; set; }

        /// <summary>
        /// 内容列开始位置(从1开始)
        /// </summary>
        public int ContentColumnStartIndex { get; set; }

        /// <summary>
        /// 要多读取多少行数据(如果输入<= 0, 处理成读取全部
        /// </summary>
        public int ReadRowCount { get; set; }
    }
}

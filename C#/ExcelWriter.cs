using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

using Aspose.Cells;
using System.Web;

namespace NE.MPS.Framework.Excel
{
    /// <summary>
    /// Write data to XLSX/XLS/CSV format file
    /// </summary>
    /// <typeparam name="T">Entity properties must use ExcelMap attribute to specify target column and ordinal.</typeparam>
    public class ExcelWriter<T> where T : class, new()
    {
        #region Cache Column Mappings

        // Cache column mapping for type
        protected static readonly Dictionary<Type, Dictionary<ColumnInfo, PropertyInfo>> __TypeColumnMappings;

        static ExcelWriter()
        {
            __TypeColumnMappings = new Dictionary<Type, Dictionary<ColumnInfo, PropertyInfo>>();
        }

        protected static Dictionary<ColumnInfo, PropertyInfo> GetTypeColumnMapping(Type type)
        {
            if (__TypeColumnMappings.ContainsKey(type))
            {
                return __TypeColumnMappings[type];
            }
            else
            {
                var columnMapping = new Dictionary<ColumnInfo, PropertyInfo>();
                var propertInfoList = type.GetProperties();
                var currentOrdinal = 1;

                foreach (var property in propertInfoList)
                {
                    var tempAttribute = property.GetCustomAttributes(typeof(ExcelMapAttribute), false);
                    if (tempAttribute.Length > 0)
                    {
                        var mapAttribute = tempAttribute[0] as ExcelMapAttribute;
                        if (mapAttribute.IsIgnore) continue;

                        var columnOrdinal = mapAttribute.Ordinal == 0 ? currentOrdinal : mapAttribute.Ordinal;
                        var columnInfo = new ColumnInfo() { Name = mapAttribute.Mapping, Ordinal = columnOrdinal };

                        if (mapAttribute.Width > 0) columnInfo.Width = mapAttribute.Width;
                        columnMapping.Add(columnInfo, property);
                        currentOrdinal++;
                    }
                }

                __TypeColumnMappings.Add(type, columnMapping);
                return columnMapping;
            }
        }

        #endregion

        private readonly Workbook _workbook;
        private readonly Dictionary<ColumnInfo, PropertyInfo> _columnMapping;
        private readonly ExcelCellStyle _defaultHeaderStyle;
        private readonly ExcelCellStyle _defaultCellStyle;
        private int _currentRowIndex = 0;
        private Worksheet currentSheet;

        public ExcelCellStyle DefaultHeaderStyle
        {
            get { return _defaultHeaderStyle; }
        }

        public ExcelCellStyle DefaultCellStyle
        {
            get { return _defaultCellStyle; }
        }

        /// <summary>
        /// sheetName 如果有值，就会将数据写到sheetName所在的sheet中，
        /// 如果没有就将第一个作为输入sheet,并将第一个名字改为输入的sheet Name
        /// </summary>
        public ExcelWriter(string sheetName)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/License/Aspose.Cells.lic");
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense(filePath);

            _workbook = new Workbook();
            currentSheet = _workbook.Worksheets[0];

            if (!string.IsNullOrWhiteSpace(sheetName))
            {
                var temp = _workbook.Worksheets[sheetName];
                if (temp != null)
                    currentSheet = temp;
                else
                    currentSheet.Name = sheetName;
            }

            // Set default row height and color palette
            currentSheet.Cells.StandardHeight = 15;

            _defaultHeaderStyle = new ExcelCellStyle()
            {
                IsBold = true,
                Color = System.Drawing.Color.FromArgb(242, 242, 242),
                FontFamily = "Calibri",
                FontSize = 12,
                BackgroundColor = System.Drawing.Color.FromArgb(0, 112, 192)
            };

            _defaultCellStyle = new ExcelCellStyle()
            {
                Color = System.Drawing.Color.Black,
                FontFamily = "Calibri",
                FontSize = 11
            };

            _columnMapping = GetTypeColumnMapping(typeof(T));
        }

        #region Append Header Row

        public void AppendHeader()
        {
            this.AppendHeader(null, null);
        }

        public void AppendHeader(ExcelCellStyle style)
        {
            this.AppendHeader(style, null);
        }

        public void AppendHeader(ExcelRowStyles styles)
        {
            this.AppendHeader(null, styles);
        }

        private void AppendHeader(ExcelCellStyle cellStyle, ExcelRowStyles rowStyles)
        {
            var sheet = currentSheet;
            foreach (var column in _columnMapping)
            {
                var cell = sheet.Cells[_currentRowIndex, column.Key.Ordinal - 1];
                cell.PutValue(column.Key.Name);

                this.SetCellStyle(cell, _defaultHeaderStyle, cellStyle, column.Key, rowStyles);
            }

            // move to next row
            _currentRowIndex++;
        }

        #endregion

        #region Append Data Row

        #region 单条数据加入

        public void AppendRow(T row)
        {
            this.AppendRow(row, null, null);
        }

        public void AppendRow(T row, ExcelCellStyle style)
        {
            this.AppendRow(row, style, null);
        }

        public void AppendRow(T row, ExcelRowStyles styles)
        {
            this.AppendRow(row, null, styles);
        }

        #endregion

        #region 批量加入数据

        /// <summary>
        /// 批量加入数据， 大家最用这个（设置样式时，可以参数化设置如： rowStyles：xxxx）
        /// cellStyle 每个单元格的样式， rowStyles 可以控制行内某些迁移的单元格的样式
        /// </summary>
        public void AppendRowList(List<T> rows, ExcelCellStyle cellStyle = null, ExcelRowStyles rowStyles = null)
        {
            foreach (var item in rows)
            {
                this.AppendRow(item, cellStyle, rowStyles);
            }
        }

        #endregion

        private void AppendRow(T row, ExcelCellStyle cellStyle, ExcelRowStyles rowStyles)
        {
            var sheet = currentSheet;
            foreach (var column in _columnMapping)
            {
                var cell = sheet.Cells[_currentRowIndex, column.Key.Ordinal - 1];
                cell.PutValue(column.Value.GetValue(row, null));

                this.SetCellStyle(cell, _defaultCellStyle, cellStyle, column.Key, rowStyles);
            }

            // move to next row
            _currentRowIndex++;
        }

        #endregion

        private void SetCellStyle(Cell cell, ExcelCellStyle baseStyle, ExcelCellStyle cellStyle, ColumnInfo column, ExcelRowStyles rowStyles)
        {
            if (baseStyle != null)
            {
                baseStyle.ApplyTo(cell);
            }

            if (cellStyle != null)
            {
                cellStyle.ApplyTo(cell);
            }

            if (column != null && rowStyles != null)
            {
                var style = rowStyles.GetStyle(column.Ordinal);
                if (style != null) style.ApplyTo(cell);
            }
        }

        private void SetColumnWidths()
        {
            var sheet = currentSheet;
            foreach (var column in _columnMapping)
            {
                if (column.Key.Width.HasValue)
                {
                    sheet.Cells.SetColumnWidth(column.Key.Ordinal - 1, column.Key.Width.Value);
                }
                else
                {
                    sheet.AutoFitColumn(column.Key.Ordinal - 1);
                }
            }
        }

        /// <summary>
        /// 将文件生成到指定的路径
        /// </summary>
        public void SaveToFile(string filePath, ExcelFormat format)
        {
            SetColumnWidths();
            _workbook.Save(filePath, GetSaveFormat(format));
        }

        /// <summary>
        /// 传入文件名，和生成的类型，返回文件的具体位置, 如果文件名没传, 默认生成一个
        /// </summary>
        public string SaveToFile(ExcelFormat format, string fileName = null)
        {
            SetColumnWidths();
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = string.Format("{0}.{1}", Guid.NewGuid().ToString(), format.ToString());

            string filePath = GenerateFilePath(fileName);
            _workbook.Save(filePath, GetSaveFormat(format));

            return filePath;
        }

        public Stream SaveToStream(ExcelFormat format)
        {
            SetColumnWidths();
            var memStream = new System.IO.MemoryStream();
            _workbook.Save(memStream, GetSaveFormat(format));

            return memStream;
        }

        private SaveFormat GetSaveFormat(ExcelFormat format)
        {
            var saveFormat = SaveFormat.Xlsx;
            if (format == ExcelFormat.Xls) saveFormat = SaveFormat.Excel97To2003;
            if (format == ExcelFormat.Csv) saveFormat = SaveFormat.CSV;
            return saveFormat;
        }

        /// <summary>
        /// 这里有点担心的是新增的文件夹是否有写的权限
        /// </summary>
        private string GenerateFilePath(string fileName)
        {
            //此处是为了在IIS和exe程序都能正常运行
            string tempFolderPath = string.Empty;

            if (HttpContext.Current != null)
                tempFolderPath = HttpContext.Current.Server.MapPath("~/Temp");
            else
                tempFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");

            if (!Directory.Exists(tempFolderPath))
                System.IO.Directory.CreateDirectory(tempFolderPath);

            return System.IO.Path.Combine(tempFolderPath, fileName);
        }

        protected class ColumnInfo
        {
            public string Name { get; set; }

            // Starts with 1
            public int Ordinal { get; set; }

            public double? Width { get; set; }
        }
    }

    public enum ExcelFormat
    {
        Xlsx,
        Xls,
        Csv
    }

    public enum ExcelCellAlignment
    {
        Left,
        Top,
        Right,
        Bottom,
        Center,
        Justify
    }

    public class ExcelRowStyles
    {
        private readonly Dictionary<int, ExcelCellStyle> _styleCollection;

        public ExcelRowStyles()
        {
            _styleCollection = new Dictionary<int, ExcelCellStyle>();
        }

        public void SetStyle(ExcelCellStyle style, params int[] columnOrdinals)
        {
            foreach (var ordinal in columnOrdinals)
            {
                if (_styleCollection.ContainsKey(ordinal))
                {
                    _styleCollection[ordinal] = style;
                }
                else
                {
                    _styleCollection.Add(ordinal, style);
                }
            }
        }

        public ExcelCellStyle GetStyle(int columnOrdinal)
        {
            if (_styleCollection.ContainsKey(columnOrdinal))
            {
                return _styleCollection[columnOrdinal];
            }

            return null;
        }
    }

    public class ExcelCellStyle
    {
        public string FontFamily { get; set; }
        public int? FontSize { get; set; }
        public System.Drawing.Color? Color { get; set; }
        public System.Drawing.Color? BackgroundColor { get; set; }
        public System.Drawing.Color? BorderColor { get; set; }
        public bool? IsBold { get; set; }
        public bool? IsItalic { get; set; }
        public bool? IsUnderline { get; set; }
        public ExcelCellAlignment? HorizontalAlignment { get; set; }
        public ExcelCellAlignment? VerticalAlignment { get; set; }

        public ExcelCellStyle()
        {
        }

        public ExcelCellStyle(ExcelCellStyle baseStyle)
        {
            this.FontFamily = baseStyle.FontFamily;
            this.FontSize = baseStyle.FontSize;
            this.Color = baseStyle.Color;
            this.BackgroundColor = baseStyle.BackgroundColor;
            this.BorderColor = baseStyle.BorderColor;
            this.IsBold = baseStyle.IsBold;
            this.IsItalic = baseStyle.IsItalic;
            this.IsUnderline = baseStyle.IsUnderline;
            this.HorizontalAlignment = baseStyle.HorizontalAlignment;
            this.VerticalAlignment = baseStyle.VerticalAlignment;
        }

        public void ApplyTo(Cell cell)
        {
            var style = cell.GetStyle();
            if (this.Color.HasValue)
            {
                style.Font.Color = this.Color.Value;
            }
            if (this.FontFamily != null) style.Font.Name = this.FontFamily;
            if (this.FontSize.HasValue) style.Font.Size = this.FontSize.Value;
            if (this.IsBold.HasValue) style.Font.IsBold = this.IsBold.Value;
            if (this.IsItalic.HasValue) style.Font.IsItalic = this.IsItalic.Value;
            if (this.IsUnderline.HasValue) style.Font.Underline = FontUnderlineType.Single;
            if (this.BackgroundColor.HasValue)
            {
                style.ForegroundColor = this.BackgroundColor.Value;
                style.Pattern = BackgroundType.Solid;
                // style.BackgroundColor = this.BackgroundColor.Value; // won't work
            }

            if (this.BorderColor.HasValue)
            {
                if (this.BorderColor.Value == System.Drawing.Color.Transparent)
                {
                    style.Borders.SetStyle(CellBorderType.None);
                }
                else
                {
                    style.Borders.SetStyle(CellBorderType.Thin);
                    style.Borders.SetColor(this.BorderColor.Value);
                }
            }

            style.BackgroundColor = System.Drawing.Color.Red;

            if (this.HorizontalAlignment.HasValue)
            {
                switch (this.HorizontalAlignment.Value)
                {
                    case ExcelCellAlignment.Left:
                        style.HorizontalAlignment = TextAlignmentType.Left;
                        break;
                    case ExcelCellAlignment.Center:
                        style.HorizontalAlignment = TextAlignmentType.Center;
                        break;
                    case ExcelCellAlignment.Right:
                        style.HorizontalAlignment = TextAlignmentType.Right;
                        break;
                    case ExcelCellAlignment.Justify:
                        style.HorizontalAlignment = TextAlignmentType.Justify;
                        break;
                }
            }

            if (this.VerticalAlignment.HasValue)
            {
                switch (this.VerticalAlignment.Value)
                {
                    case ExcelCellAlignment.Top:
                        style.VerticalAlignment = TextAlignmentType.Top;
                        break;
                    case ExcelCellAlignment.Center:
                        style.VerticalAlignment = TextAlignmentType.Center;
                        break;
                    case ExcelCellAlignment.Bottom:
                        style.VerticalAlignment = TextAlignmentType.Bottom;
                        break;
                    case ExcelCellAlignment.Justify:
                        style.VerticalAlignment = TextAlignmentType.Justify;
                        break;
                }
            }

            cell.SetStyle(style);
        }
    }
}


// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelMapAttribute.cs" company="Newegg" Author="oz3t">
//   Copyright (c) 2016 Newegg.inc. All rights reserved.
// </copyright>
// <summary>
//   ExcelMapAttribute created at  10/8/2016 3:55:30 PM
// </summary>
//<Description>
//
//</Description>
// --------------------------------------------------------------------------------------------------------------------
using System;

namespace NE.MPS.Framework.Excel
{
    public class ExcelMapAttribute : Attribute
    {
        string m_Mapping;

        public ExcelMapAttribute(string mapping)
        {
            m_Mapping = mapping;
        }

        /// <summary>
        /// 对应Excel的列名(这个是为了处理Excel的列名和实体属性不一样时)
        /// </summary>
        public string Mapping
        {
            get
            {
                return m_Mapping;
            }
        }

        /// <summary>
        /// 是否忽略此属性
        /// </summary>
        public bool IsIgnore { get; set; }

        /// <summary>
        /// 控制生成的 Excel 的列顺序 (从1开始)
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// 控制生成的 Excel 的列宽 (不指定则自动适应)
        /// </summary>
        public double Width { get; set; }
    }
}


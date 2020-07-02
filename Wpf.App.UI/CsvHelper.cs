using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wpf.App.UI
{
    public static class CsvHelper
    {
        public static DataView GetCSVContent(string filePath)
        {
            DataTable dt = new DataTable();
            string[] lines = System.IO.File.ReadAllLines(filePath);

            if (lines.Length > 0)
            {
                string firstLine = lines[0];
                string[] headerLabels = firstLine.Split(';');
                foreach (string headerWord in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(Regex.Replace(headerWord, @"\s+", "")));
                }
                dt.Columns.Add(new DataColumn("Color"));
                for (int r = 1; r < lines.Length; r++) 
                {
                    string[] dataWords = lines[r].Split(';');
                    DataRow dr = dt.NewRow();
                    int columnIndex = 0;
                    foreach (string headerWord in dt.Columns.Cast<DataColumn>().Select(col=> col.ColumnName).ToArray())
                    {
                        if (headerWord == "InStock")
                        {
                            if (dataWords[columnIndex] == "yes")
                                dr[headerWord] = true;
                            else if(dataWords[columnIndex] == "no")
                                dr[headerWord] = false;
                        }
                        else if(headerWord == "Color")
                        {
                            Random randomGen = new Random();
                            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
                            KnownColor randomColorName = names[randomGen.Next(names.Length)];
                            dr[headerWord] = randomColorName.ToString();

                        }
                        else
                            dr[headerWord] = dataWords[columnIndex];
                        columnIndex++;
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt.AsDataView();
        }
    }
}

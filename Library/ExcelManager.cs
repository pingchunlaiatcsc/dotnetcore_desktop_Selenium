using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace dotnetcore_desktop_app.Library
{
    public class ExcelManager
    {
        public static List<List<object>> readExcel(string filePath)
        {
            IWorkbook wk = null;
            string extension = Path.GetExtension(filePath);
            List<List<object>> ReturnList = new List<List<object>>();

            try
            {
                FileStream fs = File.OpenRead(filePath);
                if (extension.Equals(".xls"))
                {
                    //把xls檔案中的資料寫入wk中
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    //把xlsx檔案中的資料寫入wk中
                    wk = new XSSFWorkbook(fs);
                }

                fs.Close();
                //讀取當前表資料
                ISheet sheet = wk.GetSheetAt(0);
                var yy = sheet.SheetName;
                IRow row = sheet.GetRow(0);  //讀取當前行資料



                for (int i = 1; i <= sheet.LastRowNum; i++)  //LastRowNum 是當前表的總行數-1（注意）
                {
                    List<object> tmpList = new List<object>();

                    row = sheet.GetRow(i);  //讀取當前行資料
                    if (row != null)
                    {
                        //LastCellNum 是當前行的總列數
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            ICell cell = row.GetCell(j);
                            if (cell.CellType == CellType.Numeric || cell.CellType == CellType.Formula)
                            {
                                //NPOI中數字和日期都是NUMERIC類型的，這裏對其進行判斷是否是日期類型
                                if (HSSFDateUtil.IsCellDateFormatted(cell))//日期類型
                                {
                                    //讀取該行的第j列資料
                                    object value = cell.DateCellValue.ToShortDateString();
                                    tmpList.Add(value);
                                }
                                else//其他數字類型
                                {
                                    object value = cell.NumericCellValue;
                                    tmpList.Add(value);
                                }
                            }
                            else
                            {
                                object value = cell.ToString();
                                tmpList.Add(value);
                            }

                            //Console.Write(value.ToString() + " ");
                        }

                        //Console.WriteLine("\t");
                    }
                    ReturnList.Add(tmpList);
                }
                //Console.ReadKey();
                return ReturnList;
            }
            catch (Exception e)
            {
                //只在Debug模式下才輸出
                Console.WriteLine(e.Message);
                return ReturnList;
            }
        }


    }
}
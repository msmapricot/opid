using Excel;
using MSM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using MSM.DAL;
using OPIDEntities;

namespace MSM.Utils
{
    public class ExcelDataReader
    {
        public static List<DispositionRow> GetResearchRows (string filePath)
        {
            try
            {
                List<DispositionRow> resRows = new ExcelData(filePath).GetData().Select(dataRow => new DispositionRow
                {
                    RecordID = Convert.ToInt32(dataRow["Record ID"].ToString()),
                    Lname = dataRow["Last Name"].ToString(),
                    Fname = dataRow["First Name"].ToString(),
                    InterviewRecordID = Convert.ToInt32(dataRow["Interview Record ID"].ToString()),
                    Date = Convert.ToDateTime(dataRow["OPID Interview Date"].ToString()),
                    LBVDCheckNum = Convert.ToInt32(dataRow["LBVD Check Number"].ToString()),
                    LBVDCheckDisposition = dataRow["LBVD Check Disposition"].ToString(),
                    LBVDCheckNum2 = Convert.ToInt32(dataRow["LBVD Check Number Two"].ToString()),
                    LBVDCheck2Disposition = dataRow["LBVD Check Two Disposition"].ToString(),
                    LBVDCheckNum3 = Convert.ToInt32(dataRow["LBVD Check Number Three"].ToString()),
                    LBVDCheck3Disposition = dataRow["LBVD Check Three Disposition"].ToString(),
                    TIDCheckNum = Convert.ToInt32(dataRow["TID Check Number"].ToString()),
                    TIDCheckDisposition = dataRow["TID Check Disposition"].ToString(),
                    TIDCheckNum2 = Convert.ToInt32(dataRow["TID Check Number Two"].ToString()),
                    TIDCheck2Disposition = dataRow["TID Check Two Disposition"].ToString(),
                    TIDCheckNum3 = Convert.ToInt32(dataRow["TID Check Number Three"].ToString()),
                    TIDCheck3Disposition = dataRow["TID Check Three Disposition"].ToString(),   
                    TDLCheckNum = Convert.ToInt32(dataRow["TDL Check Number"].ToString()),
                    TDLCheckDisposition = dataRow["TDL Check Disposition"].ToString(),
                    TDLCheckNum2 = Convert.ToInt32(dataRow["TDL Check Number Two"].ToString()),
                    TDLCheck2Disposition = dataRow["TDL Check Two Disposition"].ToString(),
                    TDLCheckNum3 = Convert.ToInt32(dataRow["TDL Check Number Three"].ToString()),
                    TDLCheck3Disposition = dataRow["TDL Check Three Disposition"].ToString(),   
                    MBVDCheckNum = Convert.ToInt32(dataRow["MBVD Check Number"].ToString()),
                    MBVDCheckDisposition = dataRow["MBVD Check Disposition"].ToString(),
                    MBVDCheckNum2 = Convert.ToInt32(dataRow["MBVD Check Number Two"].ToString()),
                    MBVDCheck2Disposition = dataRow["MBVD Check Two Disposition"].ToString(),   
                    MBVDCheckNum3 = Convert.ToInt32(dataRow["MBVD Check Number Three"].ToString()),  
                    MBVDCheck3Disposition = dataRow["MBVD Check Three Disposition"].ToString(),   
                    //    SDCheckNum = Convert.ToInt32(dataRow["SD Check Number"].ToString()),
                    //    SDCheckDisposition = dataRow["SD Check Disposition"].ToString()
                }).ToList();

                return resRows;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<DispositionRow> OldGetResearchRows(string filePath)
        {
            try
            {
                List<DispositionRow> resRows = new ExcelData(filePath).GetData().Select(dataRow => new DispositionRow
                {
                    RecordID = Convert.ToInt32(dataRow["Record ID"].ToString()),
                    Lname = dataRow["Last Name"].ToString(),
                    Fname = dataRow["First Name"].ToString(),
                    InterviewRecordID = Convert.ToInt32(dataRow["Interview Record ID"].ToString()),
                    Date = Convert.ToDateTime(dataRow["OPID Interview Date"].ToString()),
                    LBVDCheckNum = Convert.ToInt32(dataRow["LBVD Check Number"].ToString()),
                    LBVDCheckDisposition = dataRow["LBVD Check Disposition"].ToString(),
                   
                    TIDCheckNum = Convert.ToInt32(dataRow["TID Check Number"].ToString()),
                    TIDCheckDisposition = dataRow["TID Check Disposition"].ToString(),
                    
                    TDLCheckNum = Convert.ToInt32(dataRow["TDL Check Number"].ToString()),
                    TDLCheckDisposition = dataRow["TDL Check Disposition"].ToString(),
                    
                    MBVDCheckNum = Convert.ToInt32(dataRow["MBVD Check Number"].ToString()),
                    MBVDCheckDisposition = dataRow["MBVD Check Disposition"].ToString(),
                  
                  //  SDCheckNum = Convert.ToInt32(dataRow["SD Check Number"].ToString()),
                  //  SDCheckDisposition = dataRow["SD Check Disposition"].ToString()
                }).ToList();

                return resRows;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<ModificationRow> GetModificationRows(string filePath)
        {
            List<ModificationRow> modRows = new ExcelData(filePath).GetData().Select(dataRow => new ModificationRow
            {
                RecordID = Convert.ToInt32(dataRow["Record ID"].ToString()),
                Lname = dataRow["Last Name"].ToString(),
                Fname = dataRow["First Name"].ToString(),
                Date = Convert.ToDateTime(dataRow["OPID Modification Date"].ToString()),
           
                LBVDModificationReason = dataRow["LBVD Modification Reason"].ToString(),
                LBVDCheckNum = Convert.ToInt32(dataRow["LBVD Modified Check Number"].ToString()),
                LBVDCheckDisposition = dataRow["LBVD Modified Check Disposition"].ToString(),

                TIDModificationReason = dataRow["TID Modification Reason"].ToString(),
                TIDCheckNum = Convert.ToInt32(dataRow["TID Modified Check Number"].ToString()),
                TIDCheckDisposition = dataRow["TID Modified Check Disposition"].ToString(),

                TDLModificationReason = dataRow["TDL Modification Reason"].ToString(),
                TDLCheckNum = Convert.ToInt32(dataRow["TDL Modified Check Number"].ToString()),
                TDLCheckDisposition = dataRow["TDL Modified Check Disposition"].ToString(),

                MBVDModificationReason = dataRow["MBVD Modification Reason"].ToString(),
                MBVDCheckNum = Convert.ToInt32(dataRow["MBVD Modified Check Number"].ToString()),
                MBVDCheckDisposition = dataRow["MBVD Modified Check Disposition"].ToString(),

                SDMReason = dataRow["SDM Reason"].ToString(),
                SDCheckNum = Convert.ToInt32(dataRow["SDM Check Number"].ToString()),
                SDCheckDisposition = dataRow["SDM Check Disposition"].ToString()
            }).ToList();

            return modRows;
        }

        public static Check GetClearedCheck(System.Data.DataRow row, int k)
        {
            Check check = null;

            try
            {
                check = new Check
                {
                    Date = GetDateValue(row),
                    Num = GetCheckNum(row),
                    // PLB 10/12/ 2017 No longer have Memo field since Bill is not providing it. 
                    // PLB 10/12/2017 Check for blank row by 0 value in Num field instead of NoCheck value in Memo field.
                    //    Memo = GetMemo(dataRow), 
                  //  Clr = GetCheckStatus(row),  // PLB 1/16/18 Simplified file does not contain Clr column
                    Amount = GetCheckAmount(row)
                };
            }
            catch (Exception e)
            {
                throw new Exception("Bad cleared check");
            }

            return check;
            
        }

        public static List<Check> GetQuickbooksChecks(string filePath)
        {
            int k = 0;
            List<Check> rowChecks = new ExcelData(filePath).GetData().Select(dataRow =>  
                 GetClearedCheck(dataRow, k++)
                ).ToList();

            List<Check> quickbooksChecks = new List<Check>();

            // Remove checks corresponding to blank rows in Excel file.
            foreach(Check check in rowChecks)
            {
                if (check.Num != 0)  // check.Num == 0 denotes blank row. Could be because Num = EFT. See GetCheckNum.
                //if (!check.Memo.Equals("NoCheck"))
                {
                    quickbooksChecks.Add(check);
                }
            }

            return quickbooksChecks;
        }

        public static List<Check> GetVoidedChecks(string filePath)
        {
            List<Check> rowChecks = new ExcelData(filePath).GetData().Select(dataRow =>
                new Check
                {
                    Date = GetDateValue(dataRow),  // PLB 10/12/2017 Used when clicking on Inspect tab.
                    Num = GetCheckNum(dataRow),
                    Memo = "Voided check" //GetMemo(dataRow),
                }).ToList();

            List<Check> voidedChecks = new List<Check>();

            // Remove checks corresponding to blank rows in Excel file.
            foreach (Check check in rowChecks)
            {
                if (check.Num != 0)  // if (!check.Memo.Equals("NoCheck"))
                {
                    voidedChecks.Add(check);
                }
            }

            return voidedChecks;
        }

        public static List<Check> GetImportMeChecks(string filePath)
        {
            List<Check> importedChecks = new List<Check>();
            List<ImportRow> rowChecks = new ExcelData(filePath).GetData().Select(dataRow =>
                new ImportRow
                {
                    Date = DateTime.Today,  // PLB 12/19/2018 Used when clicking on Inspect tab.
                    RecordID = GetRowRID(dataRow),

                    // LBVD
                    LBVDCheckNum = GetRowCheckNum(dataRow, "LBVD"),  
                    LBVDCheckDisposition = GetRowCheckDisposition(dataRow, "LBVDD"),
                    LBVDCheckNum2 = GetRowCheckNum(dataRow, "LBVD2"),  
                    LBVDCheck2Disposition = GetRowCheckDisposition(dataRow, "LBVD2D"),
                    LBVDCheckNum3 = GetRowCheckNum(dataRow, "LBVD3"),
                    LBVDCheck3Disposition = GetRowCheckDisposition(dataRow, "LBVD3D"),

                    // TID
                    TIDCheckNum = GetRowCheckNum(dataRow, "TID"),
                    TIDCheckDisposition = GetRowCheckDisposition(dataRow, "TIDD"),
                    TIDCheckNum2 = GetRowCheckNum(dataRow, "TID2"),
                    TIDCheck2Disposition = GetRowCheckDisposition(dataRow, "TID2D"),
                    TIDCheckNum3 = GetRowCheckNum(dataRow, "TID3"),
                    TIDCheck3Disposition = GetRowCheckDisposition(dataRow, "TID3D"),

                    // TDL
                    TDLCheckNum = GetRowCheckNum(dataRow, "TDL"),
                    TDLCheckDisposition = GetRowCheckDisposition(dataRow, "TDLD"),
                    TDLCheckNum2 = GetRowCheckNum(dataRow, "TDL2"),
                    TDLCheck2Disposition = GetRowCheckDisposition(dataRow, "TDL2D"),
                    TDLCheckNum3 = GetRowCheckNum(dataRow, "TDL3"),
                    TDLCheck3Disposition = GetRowCheckDisposition(dataRow, "TDL3D"),

                    // MBVD
                    MBVDCheckNum = GetRowCheckNum(dataRow, "MBVD"),
                    MBVDCheckDisposition = GetRowCheckDisposition(dataRow, "MBVDD"),
                    MBVDCheckNum2 = GetRowCheckNum(dataRow, "MBVD2"),
                    MBVDCheck2Disposition = GetRowCheckDisposition(dataRow, "MBVD2D"),
                    MBVDCheckNum3 = GetRowCheckNum(dataRow, "MBVD3"),
                    MBVDCheck3Disposition = GetRowCheckDisposition(dataRow, "MBVD3D")
                }).ToList();

            foreach (ImportRow row in rowChecks)
            {
                // LBVD
                if (row.LBVDCheckNum != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.LBVDCheckNum,
                        Service = "LBVD",
                        Disposition = row.LBVDCheckDisposition
                    });
                }
                if (row.LBVDCheckNum2 != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.LBVDCheckNum2,
                        Service = "LBVD2",
                        Disposition = row.LBVDCheck2Disposition
                    });
                }
                if (row.LBVDCheckNum3 != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.LBVDCheckNum3,
                        Service = "LBVD3",
                        Disposition = row.LBVDCheck3Disposition
                    });
                }

                // TID
                if (row.TIDCheckNum != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.TIDCheckNum,
                        Service = "TID",
                        Disposition = row.TIDCheckDisposition
                    });
                }
                if (row.TIDCheckNum2 != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.TIDCheckNum2,
                        Service = "TID2",
                        Disposition = row.TIDCheck2Disposition
                    });
                }
                if (row.TIDCheckNum3 != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.TIDCheckNum3,
                        Service = "TID3",
                        Disposition = row.TIDCheck3Disposition
                    });
                }

                // TDL
                if (row.TDLCheckNum != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.TDLCheckNum,
                        Service = "TDL",
                        Disposition = row.TDLCheckDisposition
                    });
                }
                if (row.TDLCheckNum2 != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.TDLCheckNum2,
                        Service = "TDL2",
                        Disposition = row.TDLCheck2Disposition
                    });
                }
                if (row.TDLCheckNum3 != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.TDLCheckNum3,
                        Service = "TDL3",
                        Disposition = row.TDLCheck3Disposition
                    });
                }

                // MBVD
                if (row.MBVDCheckNum != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.MBVDCheckNum,
                        Service = "MBVD",
                        Disposition = row.MBVDCheckDisposition
                    });
                }
                if (row.MBVDCheckNum2 != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.MBVDCheckNum2,
                        Service = "MBVD2",
                        Disposition = row.MBVDCheck2Disposition
                    });
                }
                if (row.MBVDCheckNum3 != 0)
                {
                    importedChecks.Add(new Check
                    {
                        InterviewRecordID = row.RecordID,
                        Date = row.Date,
                        Num = row.MBVDCheckNum3,
                        Service = "MBVD3",
                        Disposition = row.MBVDCheck3Disposition
                    });
                }
            }

            return importedChecks;
        }

        /*
        public static List<ResearchCheck> GetResearchChecks(string filePath)
        {
            List<ResearchCheck> resChecks = new ExcelData(filePath).GetData().Select(dataRow => new ResearchCheck
            {
                Date = Convert.ToDateTime(dataRow["Date"].ToString()),
                RecordID = Convert.ToInt32(dataRow["Record ID"].ToString()),
                InterviewRecordID = (DBNull.Value.Equals(dataRow["Interview Record ID"]) ? 0 : Convert.ToInt32(dataRow["Interview Record ID"].ToString())),
                Name = dataRow["Name"].ToString(),
                Num = Convert.ToInt32(dataRow["Check Number"].ToString()),
                Service = dataRow["Service"].ToString(),
                Disposition = dataRow["Disposition"].ToString()
            }).ToList();

            return resChecks;
        }
        */


        public static List<UnresolvedCheck> GetUnresolvedChecks(string filePath)
        {
            List<UnresolvedCheck> unresolvedChecks = new ExcelData(filePath).GetData().Select(dataRow => new UnresolvedCheck
            {
                Date = Convert.ToDateTime(dataRow["Date"].ToString()),
                RecordID = Convert.ToInt32(dataRow["Record ID"].ToString()),
                InterviewRecordID = (DBNull.Value.Equals(dataRow["Interview Record ID"]) ? 0 : Convert.ToInt32(dataRow["Interview Record ID"].ToString())),
                Name = dataRow["Name"].ToString(),
                Num = Convert.ToInt32(dataRow["Check Number"].ToString()),
                Service = dataRow["Service"].ToString(),
                Disposition = dataRow["Disposition"].ToString()
            }).ToList();

            return unresolvedChecks;
        }

        public static List<EmptyCol> GetEmptyFile(string filePath)
        {
            List<EmptyCol> emptyCols = new ExcelData(filePath).GetData().Select(dataRow =>
                new EmptyCol
                {
                    Empty = GetEmpty(dataRow)
                    
                }).ToList();

            return emptyCols;
        }

        private static DateTime GetDateValue(System.Data.DataRow row)
        {
            string dvalue;
            DateTime rdate = (DateTime)row["Date"];

          //  if (DBNull.Value.Equals(row["Date of Check"]))  //if (DBNull.Value.Equals(row["Date"]))

            if (DBNull.Value.Equals(row["Date"]))  // For File1 and File2 read on Mach 30, 2018 
            { 
                // This is a blank row. Provide a dummy value.
                dvalue = "12/12/1900";
            }
            else
            {
             //   dvalue = row["Date of Check"].ToString();  //dvalue = row["Date"].ToString();
                dvalue = row["Date"].ToString();  // For File1 and File2 read on March 30, 2018
            }

            DateTime dtime = DateTime.Now;   

            try
            {
                dtime = Convert.ToDateTime(dvalue);
            }
            catch (Exception e)
            { 
               throw new Exception("Bad date value");
            }
           
            return dtime;
        }

        private static int GetRowRID(System.Data.DataRow row)
        {
            string cvalue;

            //if (DBNull.Value.Equals(row["Check Number"]))  // if (DBNull.Value.Equals(row["Num"]))
            if (DBNull.Value.Equals(row["RID"]))  // For File1 and File2 read on March 30, 2018
            {
                // This is a blank row. Provide a dummy value.
                cvalue = "0";
            }
            else
            {
                // cvalue = row["Check Number"].ToString();  // cvalue = row["Num"].ToString();
                cvalue = row["RID"].ToString();  // For File1 and File2 read on March 30, 2018
                 
            }

            int cnum = 0;

            try
            {
                cnum = Convert.ToInt32(cvalue);
            }
            catch (Exception e)
            {
                throw new Exception("Bad RID value");
            }

            return cnum;
        }

        private static int GetRowCheckNum(System.Data.DataRow row, string field)
        {
            string dvalue;

            //if (DBNull.Value.Equals(row["Check Number"]))  // if (DBNull.Value.Equals(row["Num"]))
            if (DBNull.Value.Equals(row[field]))  // For File1 and File2 read on March 30, 2018
            {
                // This is a blank row. Provide a defaultValue.
                dvalue = "0";
            }
            else
            {
                // cvalue = row["Check Number"].ToString();  // cvalue = row["Num"].ToString();
                dvalue = row[field].ToString();  // For File1 and File2 read on March 30, 2018
            }

            int cnum = 0;

            try
            {
                cnum = Convert.ToInt32(dvalue);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Bad {0} value", field));
            }

            return cnum;
        }

        private static string GetRowCheckDisposition(System.Data.DataRow row, string field)
        {
            string dvalue;

            //if (DBNull.Value.Equals(row["Check Number"]))  // if (DBNull.Value.Equals(row["Num"]))
            if (DBNull.Value.Equals(row[field]))  // For File1 and File2 read on March 30, 2018
            {
                // This is a blank row. Provide a defaultValue.
                dvalue = string.Empty;
            }
            else
            {
                // cvalue = row["Check Number"].ToString();  // cvalue = row["Num"].ToString();
                dvalue = row[field].ToString();  // For File1 and File2 read on March 30, 2018

            }
 
            return dvalue;
        }

        private static int GetCheckNum(System.Data.DataRow row)
        {
            string cvalue;

            //if (DBNull.Value.Equals(row["Check Number"]))  // if (DBNull.Value.Equals(row["Num"]))
            if (DBNull.Value.Equals(row["Num"]))  // For File1 and FIle2 read on March 30, 2018
            {
                // This is a blank row. Provide a dummy value.
                cvalue = "0";
            }
            else
            {
               // cvalue = row["Check Number"].ToString();  // cvalue = row["Num"].ToString();
                cvalue = row["Num"].ToString();  // For FIle1 and File2 read on March 30, 2018
                if (cvalue.Equals("EFT") || cvalue.Equals("Debit"))  // PLB 10/12/2017. Bill's file may have EFT or Debit in Num field. Treat as blank line.
                {
                    cvalue = "0";
                }
            }

            int cnum = 0;

            try
            {
                cnum = Convert.ToInt32(cvalue);
            }
            catch (Exception e)
            {
                throw new Exception("Bad number value");
            }

            return cnum;
        }

        private static string GetMemo(System.Data.DataRow row)
        {
            string mvalue;

            if (DBNull.Value.Equals(row["Memo"]))
            {
                // This is a blank row. Provide a dummy value.
                mvalue = "NoCheck";
            }
            else
            {
                mvalue = row["Memo"].ToString();
            }

            return mvalue;
        }

        private static string GetCheckStatus(System.Data.DataRow row)
        {
            string svalue;

            if (DBNull.Value.Equals(row["Clr"]))
            {
                svalue = "Unknown";
            }
            else
            {
                svalue = row["Clr"].ToString();
            }

            return svalue;
        }

        private static string GetCheckAmount(System.Data.DataRow row)
        {
            string svalue;

            /*
            if (DBNull.Value.Equals(row["Clr"]))
            {
                svalue = "Unknown";
            }
            else
            {
                svalue = row["Amount"].ToString();
            }
            */

           // svalue = row["Amount of Check"].ToString();  //svalue = row["Amount"].ToString();
            svalue = row["Amount"].ToString(); // For File1 and File2 read on March 30, 2018
            return svalue;
        }

        private static string GetEmpty(System.Data.DataRow row)
        {
            return "Empty";
        }
    }
}
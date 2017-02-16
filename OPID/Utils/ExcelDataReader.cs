using Excel;
using MSM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using MSM.DAL;

namespace MSM.Utils
{
    public class ExcelDataReader
    {
        public static List<DispositionRow> GetResearchRows (string filePath)
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
                //    SDCheckNum = Convert.ToInt32(dataRow["SD Check Number"].ToString()),
                //    SDCheckDisposition = dataRow["SD Check Disposition"].ToString()
                }).ToList();

            return resRows;
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

        public static List<Check> GetQuickbooksChecks(string filePath)
        {
            List<Check> rowChecks = new ExcelData(filePath).GetData().Select(dataRow => 
                new Check
                {
                    Date = GetDateValue(dataRow),
                    Num = GetCheckNum(dataRow),  
                    Memo = GetMemo(dataRow),  
                    Clr = GetCheckStatus(dataRow),
                    Amount = GetCheckAmount(dataRow)
                }).ToList();

            List<Check> quickbooksChecks = new List<Check>();

            // Remove checks corresponding to blank rows in Excel file.
            foreach(Check check in rowChecks)
            {
                if (!check.Memo.Equals("NoCheck"))
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
                    Date = GetDateValue(dataRow),
                    Num = GetCheckNum(dataRow),
                    Memo = GetMemo(dataRow),
                }).ToList();

            List<Check> voidedChecks = new List<Check>();

            // Remove checks corresponding to blank rows in Excel file.
            foreach (Check check in rowChecks)
            {
                if (!check.Memo.Equals("NoCheck"))
                {
                    voidedChecks.Add(check);
                }
            }

            return voidedChecks;
        }

        public static List<ResearchCheck> GetResearchChecks(string filePath)
        {
            List<ResearchCheck> resChecks = new ExcelData(filePath).GetData().Select(dataRow => new ResearchCheck
            {
                Date = Convert.ToDateTime(dataRow["Date"].ToString()),
                RecordID = Convert.ToInt32(dataRow["Record ID"].ToString()),
                InterviewRecordID = Convert.ToInt32(dataRow["Interview Record ID"].ToString()),
                Stale = Convert.ToBoolean(dataRow["Stale"]),
                Name = dataRow["Name"].ToString(),
                Num = Convert.ToInt32(dataRow["Check Number"].ToString()),
                Service = dataRow["Service"].ToString()
            }).ToList();

            return resChecks;
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

            if (DBNull.Value.Equals(row["Date"]))
            {
                // This is a blank row. Provide a dummy value.
                dvalue = "12/12/1900";
            }
            else
            {
                dvalue = row["Date"].ToString();
            }

            DateTime dtime = Convert.ToDateTime(dvalue);
            return dtime;
        }

        private static int GetCheckNum(System.Data.DataRow row)
        {
            string cvalue;

            if (DBNull.Value.Equals(row["Num"]))
            {
                // This is a blank row. Provide a dummy value.
                cvalue = "0";
            }
            else
            {
                cvalue = row["Num"].ToString();
            }

            return Convert.ToInt32(cvalue);
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

            if (DBNull.Value.Equals(row["Clr"]))
            {
                svalue = "Unknown";
            }
            else
            {
                svalue = row["Amount"].ToString();
            }

            return svalue;
        }

        private static string GetEmpty(System.Data.DataRow row)
        {
            return "Empty";
        }
    }
}
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

            if (DBNull.Value.Equals(row["Date of Check"]))  //if (DBNull.Value.Equals(row["Date"]))
            { 
                // This is a blank row. Provide a dummy value.
                dvalue = "12/12/1900";
            }
            else
            {
                dvalue = row["Date of Check"].ToString();  //dvalue = row["Date"].ToString();
            }

            DateTime dtime = Convert.ToDateTime(dvalue);
            return dtime;
        }

        private static int GetCheckNum(System.Data.DataRow row)
        {
            string cvalue;

            if (DBNull.Value.Equals(row["Check Number"]))  // if (DBNull.Value.Equals(row["Num"]))
            {
                // This is a blank row. Provide a dummy value.
                cvalue = "0";
            }
            else
            {
                cvalue = row["Check Number"].ToString();  // cvalue = row["Num"].ToString();
                if (cvalue.Equals("EFT") || cvalue.Equals("Debit"))  // PLB 10/12/2017. Bill's file may have EFT or Debit in Num field. Treat as blank line.
                {
                    cvalue = "0";
                }
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

            svalue = row["Amount of Check"].ToString();  //svalue = row["Amount"].ToString();
            return svalue;
        }

        private static string GetEmpty(System.Data.DataRow row)
        {
            return "Empty";
        }
    }
}
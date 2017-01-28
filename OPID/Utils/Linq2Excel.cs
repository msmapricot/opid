using LinqToExcel;
using MSM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSM.Utils
{
    public class Linq2Excel
    {
        public static ExcelQueryFactory GetFactory(string filePath)
        {
            try
            {
                var eqf = new ExcelQueryFactory(filePath);

                // See: http://www.codeproject.com/Articles/659643/Csharp-Query-Excel-and-CSV-Files-Using-LinqToExcel
                // From: http://stackoverflow.com/questions/15741303/64-bits-alternatives-to-linq-to-excel
                eqf.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;

                return eqf;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static void PrepareApricotMapping(ExcelQueryFactory eqf)
        {
            eqf.AddMapping("RecordID", "Record ID");
            eqf.AddMapping("Lname", "Last Name");
            eqf.AddMapping("Fname", "First Name");
            eqf.AddMapping("InterviewRecordID", "Interview Record ID");
            eqf.AddMapping("Date", "OPID Interview Date");
            eqf.AddMapping("LBVDCheckNum", "LBVD Check Number");
            eqf.AddMapping("LBVDCheckDisposition", "LBVD Check Disposition");

            eqf.AddMapping("TIDCheckNum", "TID Check Number");
            eqf.AddMapping("TIDCheckDisposition", "TID Check Disposition");

            eqf.AddMapping("TDLCheckNum", "TDL Check Number");
            eqf.AddMapping("TDLCheckDisposition", "TDL Check Disposition");

            eqf.AddMapping("MBVDCheckNum", "MBVD Check Number");
            eqf.AddMapping("MBVDCheckDisposition", "MBVD Check Disposition");

            eqf.AddMapping("SDCheckNum", "SD Check Number");
            eqf.AddMapping("SDCheckDisposition", "SD Check Disposition");
        }

        public static List<DispositionRow> GetDispositionRows(string filePath)
        {
            ExcelQueryFactory eqf = GetFactory(filePath);
            PrepareApricotMapping(eqf);

            var rows = from c in eqf.Worksheet<DispositionRow>("Sheet1") select c;

            return rows.ToList();
        }
    }
}
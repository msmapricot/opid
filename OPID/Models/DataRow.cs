using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSM.Models
{
    public class DataRow
    {
        public int RecordID { get; set; }

        public string Lname { get; set; }
        public string Fname { get; set; }
        public string Name { get; set; }

        public int InterviewRecordID { get; set; }

        public DateTime Date { get; set; }

        public int LBVDCheckNum { get; set; }
        public string LBVDCheckDisposition { get; set; }

        public int LBVDCheckNum2 { get; set; }
        public string LBVDCheck2Disposition { get; set; }

        public int LBVDCheckNum3 { get; set; }
        public string LBVDCheck3Disposition { get; set; }

        public int TIDCheckNum { get; set; }
        public string TIDCheckDisposition { get; set; }

        public int TIDCheckNum2 { get; set; }
        public string TIDCheck2Disposition { get; set; }

        public int TIDCheckNum3 { get; set; }
        public string TIDCheck3Disposition { get; set; }

        public int TDLCheckNum { get; set; }
        public string TDLCheckDisposition { get; set; }

        public int TDLCheckNum2 { get; set; }
        public string TDLCheck2Disposition { get; set; }

        public int TDLCheckNum3 { get; set; }
        public string TDLCheck3Disposition { get; set; }

        public int MBVDCheckNum { get; set; }
        public string MBVDCheckDisposition { get; set; }

        public int MBVDCheckNum2 { get; set; }
        public string MBVDCheck2Disposition { get; set; }

        public int MBVDCheckNum3 { get; set; }
        public string MBVDCheck3Disposition { get; set; }

        public int SDCheckNum { get; set; }
        public string SDCheckDisposition { get; set; }

    }
}
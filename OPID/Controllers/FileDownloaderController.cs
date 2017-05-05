using MSM.DAL;
using MSM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace MSM.Controllers
{
    public class FileDownloaderController : ApiController
    {
        private static string timestamp;

        private HttpResponseMessage DownloadSpecifiedImportMe(string fname, string filePath)
        {
            Byte[] bytes = null;
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            bytes = br.ReadBytes((Int32)fs.Length);
            br.Close();
            fs.Close();

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            System.IO.MemoryStream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            // result.Content.Headers.ContentType = new MediaTypeHeaderValue(fileType);
            //  result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/force-download");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fname
            };
            return (result);
        }

        // Strictly for testing by Postman.
        [HttpGet]
        public string DownloadPath()
        {
            string downloadPath = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Downloads/{0}.csv", "importme"));
            return downloadPath;
        }

         
        [HttpGet]
        public HttpResponseMessage DownloadImportMe(string fileName, string fileType)
        {
            List<ImportRow> importRows = DataManager.GetImportRows();

            switch (fileName)
            {
                case "interview":
                    return DownloadInterviewImportMe(importRows);

                case "modifications":
                    return DownloadModificationsImportMe(importRows);

                default:
                    return null;
            }
        }

        public HttpResponseMessage DownloadInterviewImportMe(List<ImportRow> importRows)
        {
            if (importRows != null)
            {
                // Static variable timestamp will be set by this point, because GetTimestamp will have been called.
                string fname = string.Format("interview-importme-{0}", timestamp);
                PrepareInterviewImportFile(fname, importRows);

                string filePath = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Downloads/{0}.csv", fname));

                return DownloadSpecifiedImportMe(fname, filePath);
            }

            return null;
        }

        public HttpResponseMessage DownloadModificationsImportMe(List<ImportRow> importRows)
        {
            if (importRows != null)
            {
                // Static variable timestamp will be set by this point, because GetTimestamp will have been called.
                string fname = string.Format("modifications-importme-{0}", timestamp);
                PrepareModificationsImportFile(fname, importRows);

                string filePath = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Downloads/{0}.csv", fname));

                return DownloadSpecifiedImportMe(fname, filePath);
            }

            return null;
        }

        [HttpGet]
        // For PostMan testing only.
        public List<ImportRow> GetStaleImportRows()
        {
            List<Check> staleChecks = DataManager.GetStaleChecks();

            List<ImportRow> importRows = DataManager.GetStaleRows(staleChecks);

            return importRows;
        }

        [HttpGet]
        public HttpResponseMessage DownloadStaleChecks(string fileName, string fileType)
        {
            List<Check> staleChecks = DataManager.GetStaleChecks();

            List<ImportRow> importRows = DataManager.GetStaleRows(staleChecks);

            switch (fileName)
            {
                case "interviewstale":
                    return DownloadInterviewStaleChecks(importRows);

                case "modificationsstale":
                    return DownloadModificationsStaleChecks(importRows);

                default:
                    return null;
            }
        }

        public HttpResponseMessage DownloadInterviewStaleChecks(List<ImportRow> importRows)
        {
            if (importRows != null)
            {
                // Static variable timestamp will be set by this point, because GetTimestamp will have been called.
                string fname = string.Format("interview-stalechecks-{0}", timestamp);
                PrepareInterviewImportFile(fname, importRows);

                string filePath = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Downloads/{0}.csv", fname));

                return DownloadSpecifiedImportMe(fname, filePath);
            }

            return null;
        }

        public HttpResponseMessage DownloadModificationsStaleChecks(List<ImportRow> importRows)
        {
            if (importRows != null)
            {
                // Static variable timestamp will be set by this point, because GetTimestamp will have been called.
                string fname = string.Format("modifications-stalechecks-{0}", timestamp);
                PrepareModificationsImportFile(fname, importRows);

                string filePath = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Downloads/{0}.csv", fname));

                return DownloadSpecifiedImportMe(fname, filePath);
            }

            return null;
        }

        private static void PrepareInterviewImportFile(string fname, List<ImportRow> updatedRows)
        {
            var csv = new StringBuilder();

            string pathToDispositionHeader = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/App_Data/Interview Import Me Header.csv"));

            using (StreamReader reader = new StreamReader(pathToDispositionHeader))
            {
                string header = reader.ReadToEnd();
                csv.Append(header);
            }
            
            foreach (ImportRow d in updatedRows)
            {
                if (d.LBVDCheckNum > 0 || d.TIDCheckNum > 0 || d.TDLCheckNum > 0 || d.MBVDCheckNum > 0 || d.SDCheckNum > 0)
                {
                    // Only create a row if it contains a modified check number.
                    string csvRow = string.Format(",{0},{1},{2},{3},{4},{5},{6},{7},{8}", // ,{9},{10}",
                        d.InterviewRecordID,
                        (d.LBVDCheckNum > 0 ? d.LBVDCheckNum : 0),
                        (d.LBVDCheckNum > 0 ? d.LBVDCheckDisposition : string.Empty),
                        (d.TIDCheckNum > 0 ? d.TIDCheckNum : 0),
                        (d.TIDCheckNum > 0 ? d.TIDCheckDisposition : string.Empty),
                        (d.TDLCheckNum > 0 ? d.TDLCheckNum : 0),
                        (d.TDLCheckNum > 0 ? d.TDLCheckDisposition : string.Empty),
                        (d.MBVDCheckNum > 0 ? d.MBVDCheckNum : 0),
                        (d.MBVDCheckNum > 0 ? d.MBVDCheckDisposition : string.Empty));
                    //     (d.SDCheckNum > 0 ? d.SDCheckNum : 0),
                    //     (d.SDCheckNum > 0 ? d.SDCheckDisposition : string.Empty));

                    csv.AppendLine(csvRow);
                }
            }

           
            string pathToImportMeFile = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Downloads/{0}.csv", fname));

            File.WriteAllText(pathToImportMeFile, csv.ToString());
        }
 
        private static void PrepareModificationsImportFile(string fname, List<ImportRow> updatedRows)
        {
            var csv = new StringBuilder();

            string pathToModificationsHeader = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/App_Data/Modifications Import Me Header.csv"));

            using (StreamReader reader = new StreamReader(pathToModificationsHeader))
            {
                string header = reader.ReadToEnd();
                csv.Append(header);
            }

            foreach (ImportRow d in updatedRows)
            {
                if (d.LBVDCheckNum < 0 || d.TIDCheckNum < 0 || d.TDLCheckNum < 0 || d.MBVDCheckNum < 0 || d.SDCheckNum < 0)
                {
                    // Only create a row if it contains a modified check number
                    string csvRow = string.Format(",{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                        d.RecordID,
                        (d.LBVDCheckNum < 0 ? -d.LBVDCheckNum : 0),
                        (d.LBVDCheckNum < 0 ? d.LBVDCheckDisposition : string.Empty),
                        (d.TIDCheckNum < 0 ? -d.TIDCheckNum : 0),
                        (d.TIDCheckNum < 0 ? d.TIDCheckDisposition : string.Empty),
                        (d.TDLCheckNum < 0 ? -d.TDLCheckNum : 0),
                        (d.TDLCheckNum < 0 ? d.TDLCheckDisposition : string.Empty),
                        (d.MBVDCheckNum < 0 ? -d.MBVDCheckNum : 0),
                        (d.MBVDCheckNum < 0 ? d.MBVDCheckDisposition : string.Empty),
                        (d.SDCheckNum < 0 ? -d.SDCheckNum : 0),
                        (d.SDCheckNum < 0 ? d.SDCheckDisposition : string.Empty));

                    csv.AppendLine(csvRow);
                }
            }

            // Static variable timestamp will be set by this point, because GetTimestamp will have been called.
         //   string pathToImportMeFile = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Downloads/modifications-importme-{0}.csv", timestamp));

            string pathToImportMeFile = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Downloads/{0}.csv", fname));
            File.WriteAllText(pathToImportMeFile, csv.ToString());
        }

        [HttpGet]
        public string GetTimestamp()
        {
            // Set timestamp when resolvedController is loaded. This allows
            // the timestamp to be made part of the page title, which allows
            // the timestamp to appear in the printed file and also as part
            // of the Excel file name of both the angular datatable and
            // the importme file.

            // This compensates for the fact that DateTime.Now on the AppHarbor server returns
            // the time in the timezone of the server.
            // Here we convert UTC to Central Standard Time to get the time in Houston.
            // It also properly handles daylight savings time.
            DateTime now = DateTime.Now.ToUniversalTime();
            DateTime cst = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(now, "UTC", "Central Standard Time");
            timestamp = cst.ToString("dd-MMM-yyyy-hhmm");

            return timestamp;
        }
    }
}
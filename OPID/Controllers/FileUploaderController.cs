using LinqToExcel;
using MSM.Models;
using MSM.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
  

namespace MSM.Controllers
{
    public class FileUploaderController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage UploadFile()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                List<string> docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var ftype = httpRequest.Form["ftype"];
                   
                    string filePath = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Uploads/{0}", postedFile.FileName));
                                       
                    postedFile.SaveAs(filePath);

                    docfiles.Add(filePath);
                }
              
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);  
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
        }

        // Strictly for testing by Postman.
        [HttpGet]
        public string UploadPath()
        {
            string uploadPath = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Uploads/{0}", "BP.xlsx"));
            return uploadPath;
        }

        [HttpGet]
        public bool Checkvalidity(string ftype, string fname, string fext)
        {
            string fpath = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/Uploads/{0}.{1}", fname, fext));
           
            switch(ftype)
            {
                case "VC":
                    return ValidateVCFile(fpath);
                case "AP":
                    return ValidateAPFile(fpath);
                case "MD":
                    return ValidateMDFile(fpath);
                case "QB":
                    return ValidateQBFile(fpath);
                case "RT":  // Research Table
                    return ValidateRTFile(fpath);
                default:
                    return false;
            }
        }

        
        private bool ValidateAPFile(string fpath)
        {
            bool valid = true;
 
            try
            {
                List<DispositionRow> rows = ExcelDataReader.GetResearchRows(fpath);

                int zeroRecords = rows.Count(r => r.InterviewRecordID == 0);
 
                valid = (zeroRecords == 0);

            } catch
            {
                valid = false;
            }

            return valid;
           // return true;
        }

        private bool ValidateMDFile(string fpath)
        {
            bool valid = true;

            try
            {
                List<ModificationRow> rows = ExcelDataReader.GetModificationRows(fpath);

                int zeroRecords = rows.Count(r => r.InterviewRecordID == 0);

                valid = (zeroRecords != 0);

            }
            catch
            {
                valid = false;
            }

            return valid;
        }

        private bool ValidateVCFile(string fpath)
        {
            bool valid = true;

            try
            {
                var checks = ExcelDataReader.GetVoidedChecks(fpath);

                var clr = (from c in checks
                           where (string.IsNullOrEmpty(c.Clr))
                           select c).FirstOrDefault();

                if (clr == null)
                {
                    valid = false;
                }
            }
            catch
            {
                valid = false;
            }

            return valid;
        }

        private bool ValidateQBFile(string fpath)
        {
            bool valid = true;
 
            try
            {
                var checks = ExcelDataReader.GetQuickbooksChecks(fpath);

                //  var clr = checks.Count(c => c.Clr.Equals("Unknown"));

                //   var clr = 0;  // PLB 1/19/17 If column Clr has no value it is not an error!

                //   valid = clr == 0;
                valid = true;
            }
            catch
            {
                valid = false;
            }

            return valid;
        }

        private bool ValidateRTFile(string fpath)
        {
            return true;
        }
    }   
}
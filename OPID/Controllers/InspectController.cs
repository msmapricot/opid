using LinqToExcel;
using MSM.DAL;
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
    public class InspectController : ApiController
    {
        [HttpGet]
        // The parameter names matter here because if the first parameter of this method is called fileName
        // and the first parameter of GetApricotFile is also called fileName, then the routing system will
        // say there are multiple methods that match the call
        //  http://localhost/msm/api/qbfile?fileName=QB&fileType=XLSX
        // even though there are 2 distinct route templates
        //    routeTemplate: "api/qbfile"
        //    routeTemplate: "api/apfile
        // on WebAPIConfig.cs. The routing system is very hard to work with!
        // Use Postman when debugging routing.
        // This method is used to return the Quickbooks file for inspection on the Inspect tab.
        public List<Check> GetQuickbooksFile(string qbFile, string fileType)
        {
            return DataManager.GetQuickbooksChecks(qbFile, fileType);
        }

        public List<Check> GetImportMeFile(string imFile, string fileType)
        {
            return DataManager.GetImportMeChecks(imFile, fileType);
        }

        public List<Check> GetMistakenlyResolvedFile(string mrFile, string fileType)
        {
            return DataManager.GetQuickbooksChecks(mrFile, fileType);
        }

        [HttpGet]
        // This method is used to return the Voided Checks file for inspection on the Inspect tab.
        public List<Check> GetVoidedchecksFile(string vcFile, string fileType)
        {
            return DataManager.GetVoidedChecks(vcFile, fileType);
        }
        
        [HttpGet]
        // This method is used to return the Interview Research File for inspection on the Inspect tab.
        public List<DispositionRow> GetResearchFile(string resFile, string fileType)
        {
            return DataManager.GetResearchRows(resFile, fileType);
        }

        [HttpGet]
        // This method is used to return the Modifications Research File for inspection on the Inspect tab.
        public List<ModificationRow> GetModificationFile(string modFile, string fileType)
        {
            return DataManager.GetModificationRows(modFile, fileType);
        }

        [HttpGet]
        // This method is used to make sure that angular datatables don't crash if no file is supplied:
        // at least supply an empty file!
        public List<EmptyCol> GetEmptyFile(string emptyFile, string fileType)
        {
            
           // string filePath = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/App_Data/Private/{0}.{1}", emptyFile, fileType));
            string filePath = System.Web.HttpContext.Current.Request.MapPath(string.Format("~/App_Data/{0}.{1}", emptyFile, fileType));

            return ExcelDataReader.GetEmptyFile(filePath);
        }
    }   
}
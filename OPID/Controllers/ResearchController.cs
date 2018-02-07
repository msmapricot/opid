using MSM.DAL;
using MSM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MSM.Controllers
{
    public class ResearchController : ApiController
    {
        // This method returns the table displayed on the Research tab.
        /*
        [HttpGet]
        public List<Check> GetResearchChecks()
        {
            return DataManager.GetResearchChecks();
        }
        */

        [HttpGet]
        public List<Check> GetUnresolvedChecks()
        {
            return DataManager.GetUnresolvedChecks();
        }

        [HttpGet]
        public string IsEmpty()
        {
            return DataManager.IsEmpty();
        }

        [HttpGet]
        public void RestoreResearchTable(string rtFileName, string rtFileType)
        {
            DataManager.RestoreResearchTable(rtFileName, rtFileType);
        }

        [HttpGet]
        public string ResolveCheck(int checkNum)
        {
            return DataManager.ResolveCheck(checkNum);
        }

        [HttpGet]
        public void MarkStaleChecks(string type)
        {
            DataManager.MarkStaleChecks(type);
        }

        [HttpGet]
        public string GetTimestamp()
        {
            // Set timestamp when researchController is loaded. This allows
            // the timestamp to be made part of the page title, which allows
            // the timestamp to appear in the printed file and also as part
            // of the Excel file name of the angular datatable.

            // This compensates for the fact that DateTime.Now on the AppHarbor server returns
            // the the time in the timezone of the server.
            // Here we convert UTC to Central Standard Time to get the time in Houston.
            // It also properly handles daylight savings time. 
            DateTime now = DateTime.Now.ToUniversalTime();
            DateTime cst = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(now, "UTC", "Central Standard Time");
           
            return cst.ToString("dd-MMM-yyyy-hhmm"); 
        }
    }
}

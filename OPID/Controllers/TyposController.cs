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
    public class TyposController : ApiController
    {
        
        [HttpGet]
        public string GetTimestamp()
        {
            // Set timestamp when researchController is loaded. This allows
            // the timestamp to be made part of the page title, which allows
            // the timestamp to appear in the printed file and also as part
            // of the Excel file name of the Angular datatable.

            // This compensates for the fact that DateTime.Now on the AppHarbor server returns
            // the the time in the timezone of the server.
            // Here we convert UTC to Central Standard Time to get the time in Houston.
            // This is supposed to handle daylight savings time also. We will have to
            // wait and see about this.
            DateTime now = DateTime.Now.ToUniversalTime();
            DateTime cst = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(now, "UTC", "Central Standard Time");
           
            return cst.ToString("dd-MMM-yyyy-hhmm"); 
        }

        [HttpGet]
        public List<Check> GetTypoChecks()
        {
            return DataManager.GetTypoChecks();
        }
    }
}

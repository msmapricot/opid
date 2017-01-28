using MSM.DAL;
using MSM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MSM.Controllers
{
    public class ResolvedController : ApiController
    {
        [HttpGet]
        public List<Check> GetResolvedChecks()
        {
            return DataManager.GetResolvedChecks();
        }
    }
}
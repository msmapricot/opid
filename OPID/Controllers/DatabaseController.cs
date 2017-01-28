using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MSM.Controllers
{
    public class DatabaseController : ApiController
    {
        // Use Postman to check on the connection string!
        public string Get(int id)
        {
            return ConfigurationManager.ConnectionStrings["MSMEntities"].ConnectionString;
        }
    }
}
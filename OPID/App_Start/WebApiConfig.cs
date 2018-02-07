using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace MSM
{
    public static class WebApiConfig
    {
        public static string UrlPrefix
        {
          get { return "api"; } 
        }

        public static string UrlPrefixRelative
        {
            get { return "~/api"; }
        }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            
            // This route is used by datatableController.js
            config.Routes.MapHttpRoute(
             name: "GetQBFile",
             routeTemplate: "api/qbfile",
                  defaults: new { controller = "Inspect", action="GetQuickbooksFile"}
            );

            // This route is used by datatableController.js
            config.Routes.MapHttpRoute(
            name: "GetVCFile",
            routeTemplate: "api/vcfile",
                 defaults: new { controller = "Inspect", action="GetVoidedchecksFile"}
           );

          // This route is used by datatableController.js
          config.Routes.MapHttpRoute(
           name: "GetRESFile",
           routeTemplate: "api/resfile",
                defaults: new { controller = "Inspect", action = "GetResearchFile" }
          );

          // This route is used by datatableController.js
          config.Routes.MapHttpRoute(
             name: "GetMODFile",
             routeTemplate: "api/modfile",
                  defaults: new { controller = "Inspect", action = "GetModificationFile" }
            );

          config.Routes.MapHttpRoute(
             name: "GetEmptyFile",
             routeTemplate: "api/emptyfile",
             defaults: new { controller = "Inspect" }
          );

          config.Routes.MapHttpRoute(
             name: "Merge",
             routeTemplate: "api/merge",
             defaults: new { controller = "Merge", action="PerformMerge"}
          );

          config.Routes.MapHttpRoute(
                           name: "Typos",
                           routeTemplate: "api/typos",
                           defaults: new { controller = "Typos", action = "GetTypoChecks" }
                        );

          config.Routes.MapHttpRoute(
                name: "Resolved",
                routeTemplate: "api/resolved",
                defaults: new { controller = "Resolved", action="GetResolvedChecks" }
             );

          config.Routes.MapHttpRoute(
                     name: "TyposTimestamp",
                     routeTemplate: "api/typostimestamp",
                     defaults: new { controller = "Typos", action = "GetTimestamp" }
                  );

            config.Routes.MapHttpRoute(
                  name: "DownloadTimestamp",
                  routeTemplate: "api/downloadtimestamp",
                  defaults: new { controller = "FileDownloader", action = "GetTimestamp" }
               );

          /*
          config.Routes.MapHttpRoute(
                   name: "ResearchTimestamp",
                   routeTemplate: "api/researchtimestamp",
                   defaults: new { controller = "Research", action = "GetTimestamp" }
                );
         */

          config.Routes.MapHttpRoute(
               name: "Unmatched",
               routeTemplate: "api/research",
               defaults: new { controller = "Research", action="GetUnresolvedChecks" }
            );

            config.Routes.MapHttpRoute(
                     name: "IsEmpty",
                     routeTemplate: "api/isempty",
                     defaults: new { controller = "Research", action = "IsEmpty" }
                  );


            config.Routes.MapHttpRoute(
                  name: "Restore",
                  routeTemplate: "api/restore",
                  defaults: new { controller = "Research", action = "RestoreResearchTable" }
               );

            // Test using Postman
            config.Routes.MapHttpRoute(
                    name: "UploadPath",
                    routeTemplate: "api/uploadpath",
                    defaults: new { controller = "FileUploader", action="UploadPath" }
                );

          config.Routes.MapHttpRoute(
                  name: "Upload",
                  routeTemplate: "api/upload/{action}",
                  defaults: new { controller = "FileUploader"}
              );

          // Test using Postman
          config.Routes.MapHttpRoute(
                    name: "DownloadPath",
                    routeTemplate: "api/downloadpath",
                    defaults: new { controller = "FileDownloader", action = "DownloadPath" }
                );

          config.Routes.MapHttpRoute(
                 name: "DownloadImportMe",
                 routeTemplate: "api/downloadimportme",
                 defaults: new {  controller = "FileDownloader", action="DownloadImportMe"} 
             );

            // For PostMan testing only
            config.Routes.MapHttpRoute(
              name: "GetStaleImportRows",
              routeTemplate: "api/getstaleimportrows",
              defaults: new { controller = "FileDownloader", action = "GetStaleImportRows" }
          );

            config.Routes.MapHttpRoute(
               name: "DownloadStaleChecks",
               routeTemplate: "api/downloadstalechecks",
               defaults: new { controller = "FileDownloader", action = "DownloadStaleChecks" }
           );

            config.Routes.MapHttpRoute(
                   name: "Resolve",
                   routeTemplate: "api/resolvecheck/{checkNum}",
                   defaults: new { controller = "Research", action = "ResolveCheck" }
               );

            config.Routes.MapHttpRoute(
                  name: "MarkStaleChecks",
                  routeTemplate: "api/markstalechecks/{type}",
                  defaults: new { controller = "Research", action = "MarkStaleChecks" }
                );

          config.Routes.MapHttpRoute(
               name: "ValidFile",
               routeTemplate: "api/checkvalidity",
               defaults: new { controller = "FileUploader", action="CheckValidity" }
           );

          config.Routes.MapHttpRoute(
                name: "HaveResolvedChecks",
                routeTemplate: "api/haveresolvedchecks",
                defaults: new { controller = "Resolved", action="HaveResolvedChecks" }
            );

          // Frr testing with Postman
          config.Routes.MapHttpRoute(
            name: "ConnectionString",
            routeTemplate: "api/cs/{id}",
            defaults: new { controller = "Database" }
          );

          config.Routes.MapHttpRoute(
             name: "Linq2ExcelFactory",
             routeTemplate: "api/factory",
             defaults: new { controller = "MyLinqToExcel", action="GetFactory" }
           );

          config.Routes.MapHttpRoute(
               name: "Linq2ExcelDispositionRows",
               routeTemplate: "api/dispositionrows",
               defaults: new { controller = "MyLinqToExcel", action = "GetDispositionRows" }
          );

            // Used when uploading a file
            /*
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            */

            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(
              config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml"));

           // GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(new  QueryStringMapping("json", "true", "application/json"));

        }
    }
}


var desktop = false;
var server;

if (desktop == true) {
    server = "http://localhost/msm/";
} else {
   // server = "https://myopid.apphb.com/";
    server = "";
}

 
var FileServices = angular.module('FileServices', ['ngResource']);
var MergeServices = angular.module('MergeServices', ['ngResource']);
var ResearchServices = angular.module('ResearchServices', ['ngResource']);

//var MSMApp = angular.module('MSMApp', ['ngRoute', 'FileServices', 'datatables', 'linqtoexcel']);

var MSMApp = angular.module('MSMApp', ['ngRoute', 'ngSanitize', 'FileServices', 'MergeServices', 'ResearchServices', 'datatables', 'datatables.bootstrap', 'datatables.buttons']);
   
 
    



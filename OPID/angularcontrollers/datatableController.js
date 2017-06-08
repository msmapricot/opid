MSMApp.controller('datatableController', ['$scope', '$http', '$q', '$filter', 'FileManager', 'MergeManager', 'DTOptionsBuilder', 'DTColumnBuilder',
     function ($scope, $http, $q, $filter, FileManager, MergeManager, DTOptionsBuilder, DTColumnBuilder) {

         var vm = this;
         var someResolved  = false;
         $scope.pleaseAct = false;
        
         if ($scope.tab == 'inspect' && FileManager.getSelectedFile() == "Quickbooks") {
             vm.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                 var defer = $q.defer();
                 $http.get(server + "api/qbfile",
                     { params: { "qbFile": FileManager.getQBFileName(), "fileType": FileManager.getQBFileType() } }).then(function (result) {
                         defer.resolve(result.data);
                     });
                 return defer.promise;
             }).withPaginationType('full_numbers')
               .withDisplayLength(10)
               .withOption('lengthChange', false);

             vm.dtColumns = [
                     DTColumnBuilder.newColumn('Date').withTitle('Date').renderWith(function (data, type) {
                         return $filter('date')(data, 'MM/dd/yyyy')
                     }), 
                     DTColumnBuilder.newColumn('Num').withTitle('Check Number'),
                     DTColumnBuilder.newColumn('Memo').withTitle('Memo'),
                     DTColumnBuilder.newColumn('Clr').withTitle('Status')
                 ];
         }
         else if ($scope.tab == 'inspect' && FileManager.getSelectedFile() == "Voidedchecks")
         {
             vm.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                 var defer = $q.defer();
                 //  alert("datatableController.js: FileManager.getVCFileName() = " + FileManager.getVCFileName());
                 $http.get(server + "api/vcfile", 
                     { params: { "vcFile": FileManager.getVCFileName(), "fileType": FileManager.getVCFileType() } }).then(function (result) {
                         defer.resolve(result.data);
                     });
                 return defer.promise;
             }).withPaginationType('full_numbers')
               .withDisplayLength(10)
               .withOption('lengthChange', false);

             vm.dtColumns = [
                 DTColumnBuilder.newColumn('Date').withTitle('Date').renderWith(function (data, type) {
                     return $filter('date')(data, 'MM/dd/yyyy')
                 }),
                 DTColumnBuilder.newColumn('Num').withTitle('Check Number'),
                 DTColumnBuilder.newColumn('Memo').withTitle('Memo')
             ];
         }
         else if ($scope.tab == 'inspect' && FileManager.getSelectedFile() == "Research")
         {
             vm.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                 var defer = $q.defer();
              
                $http.get(server + "api/resfile",
                    { params: { "resFile": FileManager.getAPFileName(), "fileType": FileManager.getAPFileType() } }).then(function (result) {
                             defer.resolve(result.data);
                     });
                 return defer.promise;
             }).withPaginationType('full_numbers')
               .withDisplayLength(10)
               .withOption('lengthChange', false);

             vm.dtColumns = [
                 DTColumnBuilder.newColumn('RecordID').withTitle('Record ID'),
                 DTColumnBuilder.newColumn('InterviewRecordID').withTitle('Interview Record ID'),
                 DTColumnBuilder.newColumn('Lname').withTitle("Last Name"),
                 DTColumnBuilder.newColumn('Fname').withTitle("First Name"),
                 DTColumnBuilder.newColumn('LBVDCheckNum').withTitle('LBVD Check Number'),
                 DTColumnBuilder.newColumn('LBVDCheckDisposition').withTitle('LBVD Check Disposition'),
                 DTColumnBuilder.newColumn('TIDCheckNum').withTitle('TID Check Number'),
                 DTColumnBuilder.newColumn('TIDCheckDisposition').withTitle('TID Check Disposition'),
                 DTColumnBuilder.newColumn('TDLCheckNum').withTitle('TDL Check Number'),
                 DTColumnBuilder.newColumn('TDLCheckDisposition').withTitle('TDL Check Disposition'),
                 DTColumnBuilder.newColumn('MBVDCheckNum').withTitle('MBVD Check Number'),
                 DTColumnBuilder.newColumn('MBVDCheckDisposition').withTitle('MBVD Check Disposition'),
                 DTColumnBuilder.newColumn('SDCheckNum').withTitle('SD Check Number'),
                 DTColumnBuilder.newColumn('SDCheckDisposition').withTitle('SD Check Disposition')
             ];
         }
         else if ($scope.tab == 'inspect' && FileManager.getSelectedFile() == "Modifications") {
             vm.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                 var defer = $q.defer();
                 //   alert("datatableController.js: FileManager.getAPFileName() = " + FileManager.getAPFileName());
                 $http.get(server + "api/modfile",
                     { params: { "modFile": FileManager.getMDFileName(), "fileType": FileManager.getMDFileType() } }).then(function (result) {
                         defer.resolve(result.data);
                     });
                 return defer.promise;
             }).withPaginationType('full_numbers')
               .withDisplayLength(10)
               .withOption('lengthChange', false);

             vm.dtColumns = [
                 DTColumnBuilder.newColumn('RecordID').withTitle('Record ID'),
                 DTColumnBuilder.newColumn('Lname').withTitle("Last Name"),
                 DTColumnBuilder.newColumn('Fname').withTitle("First Name"),

                 DTColumnBuilder.newColumn('LBVDModificationReason').withTitle('LBVD Modification Reason'),
                 DTColumnBuilder.newColumn('LBVDCheckNum').withTitle('LBVD Modified Check Number'),
                 DTColumnBuilder.newColumn('LBVDCheckDisposition').withTitle('LBVD Modified Check Disposition'),

                 DTColumnBuilder.newColumn('TIDModificationReason').withTitle('TID Modification Reason'),
                 DTColumnBuilder.newColumn('TIDCheckNum').withTitle('TID Modified Check Number'),
                 DTColumnBuilder.newColumn('TIDCheckDisposition').withTitle('TID Modified Check Disposition'),

                 DTColumnBuilder.newColumn('TDLModificationReason').withTitle('TDL Modification Reason'),
                 DTColumnBuilder.newColumn('TDLCheckNum').withTitle('TDL Modified Check Number'),
                 DTColumnBuilder.newColumn('TDLCheckDisposition').withTitle('TDL Check Disposition'),

                 DTColumnBuilder.newColumn('MBVDModificationReason').withTitle('MBVD Modification Reason'),
                 DTColumnBuilder.newColumn('MBVDCheckNum').withTitle('MBVD Modified Check Number'),
                 DTColumnBuilder.newColumn('MBVDCheckDisposition').withTitle('MBVD Modified Check Disposition'),
                 
                 DTColumnBuilder.newColumn('SDMReason').withTitle('SDM Reason'),
                 DTColumnBuilder.newColumn('SDCheckNum').withTitle('SDM Check Number'),
                 DTColumnBuilder.newColumn('SDCheckDisposition').withTitle('SDM Check Disposition')
             ];
         }

         else if ($scope.tab == 'typos') {
             DisplayTypoChecks();
         }

         else if ($scope.tab == 'resolved') {
             DisplayResolvedChecks();
         }

         else if ($scope.tab == 'research') {
             vm.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                 var defer = $q.defer();
                 $http.get(server + "api/research").then(function (result) {
                     defer.resolve(result.data);
                 });
                 return defer.promise;
             }).withPaginationType('full_numbers')
               .withDisplayLength(10)
               .withOption('lengthChange', false)
               .withButtons(['excel', 'print']); // changed order of Excel and Print buttons

             vm.dtColumns = [
                 DTColumnBuilder.newColumn('Date').withTitle('Date').renderWith(function (data, type) {
                     return $filter('date')(data, 'MM/dd/yyyy')
                 }),
                 DTColumnBuilder.newColumn('RecordID').withTitle('Record ID'),
                 DTColumnBuilder.newColumn('InterviewRecordID').withTitle('Interview Record ID'),
                 DTColumnBuilder.newColumn('Name').withTitle('Name'),
                 DTColumnBuilder.newColumn('Num').withTitle('Check Number'),
                 DTColumnBuilder.newColumn('Service').withTitle('Service')
             ];
         }
         else { // If this final "else" clause is removed a controller error will occur. Do not remove!
             //alert("Load the empty file to avoid a controller error");
             LoadTheEmptyFile();
         }

         FileManager.setSelectedFile("Empty");

         function DisplayTypoChecks() {  // There may not be any. This is handled by returning an empty list of checks from the API.
             vm.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                 var defer = $q.defer();
                 $http.get(server + "api/typos").then(function (result) {
                     defer.resolve(result.data);
                 });
                 return defer.promise;
             }).withPaginationType('full_numbers')
               .withDisplayLength(10)
               .withOption('lengthChange', false)
               .withButtons(['excel', 'print']); // changed order of Excel and Print buttons

             vm.dtColumns = [
                      DTColumnBuilder.newColumn('Date').withTitle('Date').renderWith(function (data, type) {
                          return $filter('date')(data, 'MM/dd/yyyy')
                      }),
                      DTColumnBuilder.newColumn('RecordID').withTitle('Record ID'),
                      DTColumnBuilder.newColumn('InterviewRecordID').withTitle('Interview Record ID'),
                      DTColumnBuilder.newColumn('Name').withTitle('Name'),
                      DTColumnBuilder.newColumn('Num').withTitle('Check Number'),
                      DTColumnBuilder.newColumn('Service').withTitle('Service'),
                      DTColumnBuilder.newColumn('Clr').withTitle('Status')
             ];
         }

         function DisplayResolvedChecks() {  // There may not be any. This is handled by returning an empty list of checks from the API.
             vm.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                 var defer = $q.defer();
                 $http.get(server + "api/resolved").then(function (result) {
                     defer.resolve(result.data);
                 });
                 return defer.promise;
             }).withPaginationType('full_numbers')
               .withDisplayLength(10)
               .withOption('lengthChange', false)
               .withButtons(['excel', 'print']);  // changed order of Excel and Print buttons
             
             vm.dtColumns = [
                      DTColumnBuilder.newColumn('Date').withTitle('Date').renderWith(function (data, type) {
                          return $filter('date')(data, 'MM/dd/yyyy')
                      }),
                      DTColumnBuilder.newColumn('RecordID').withTitle('Record ID'),
                      DTColumnBuilder.newColumn('InterviewRecordID').withTitle('Interview Record ID'),
                      DTColumnBuilder.newColumn('Name').withTitle('Name'),
                      DTColumnBuilder.newColumn('Num').withTitle('Check Number'),
                      DTColumnBuilder.newColumn('Service').withTitle('Service'),
                      DTColumnBuilder.newColumn('Clr').withTitle('Status')
             ];
         }

         function LoadTheEmptyFile()
         {
             $scope.pleaseAct = true;
             vm.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                 var defer = $q.defer();
                 $http.get(server + "api/emptyfile",
                     { params: { "emptyFile": "Empty", "fileType": "xlsx" } }).then(function (result) {
                         defer.resolve(result.data);
                     });
                 return defer.promise;
             }).withPaginationType('full_numbers');
             vm.dtColumns = [
                 DTColumnBuilder.newColumn('Empty').withTitle('Empty'),
             ];
         }
     }
])
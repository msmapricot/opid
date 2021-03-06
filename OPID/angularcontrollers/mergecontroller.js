﻿
// Technique for this controller found at 
//  http://www.c-sharpcorner.com/uploadfile/1d3119/file-upload-and-download-using-html5-file-uploader-control-a851/
// Moved downloading to angular service called fileservices. Was not able to move
// the uploading process to this service.
// Added to the download solution using code found at Stack Overflow. See link below.
MSMApp.controller('mergeController', ['$rootScope', '$scope', '$http', 'FileManager', 'MergeManager',
        function ($rootScope, $scope, $http, FileManager, MergeManager) {
            $rootScope.pageTitle = "Main Street Ministries - Merge";
            $scope.mergeStatus = "";
            $scope.files = [];
            $scope.VCUploadedFile = FileManager.getVCFileName() + "." + FileManager.getVCFileType();
            $scope.APUploadedFile = FileManager.getAPFileName() + "." + FileManager.getAPFileType();
            $scope.MDUploadedFile = FileManager.getMDFileName() + "." + FileManager.getMDFileType();
            $scope.QBUploadedFile = FileManager.getQBFileName() + "." + FileManager.getQBFileType();
            $scope.IMUploadedFile = FileManager.getIMFileName() + "." + FileManager.getIMFileType();
            $scope.MRUploadedFile = FileManager.getMRFileName() + "." + FileManager.getMRFileType();
            $scope.RRCUploadedFile = FileManager.getRRCFileName() + "." + FileManager.getRRCFileType();
            $scope.RRVUploadedFile = FileManager.getRRVFileName() + "." + FileManager.getRRVFileType();

            $scope.VCUpload = function () {
                var fd = new FormData()
                for (var i in $scope.files) {
                    // i is an array index
                    if ($scope.files[i].ftype == 'VC' && $scope.files[i].seen == "false") {
                        //  console.log("Upload a VC file");
                        fd.append("uploadedFile", $scope.files[i].file);
                        fd.append("ftype", "VC");
                       
                        var xhr = new XMLHttpRequest();
                        xhr.addEventListener("load", VCUploadComplete, false);
                        xhr.open("POST", server + "api/upload/UploadFile");
                        $scope.progressVisible = true;
                        xhr.send(fd);
                    }
                }
            }

            function VCUploadComplete(evt) {
                $scope.progressVisible = false;
                if (evt.target.status == 201) {
                    $scope.FilePath = evt.target.responseText;

                    $scope.$apply(function (scpe) {
                        $scope.VCUploadStatus = "Upload Complete";
                        //   alert("AP scope.files[0].name = " + $scope.files[0].name);
                        for (var i in $scope.files) {
                            var jsonObj = $scope.files[i];
                            if (jsonObj.ftype == 'VC' & jsonObj.seen == "false") {
                                FileManager.getValidFile('VC', jsonObj.file).then(function (v) {
                                    jsonObj.seen = "true";
                                    $scope.VCUploadedFile = jsonObj.file.name;  // this include the extension
                                    
                                    // Don't know why have to set variable valid, but does not work otherwise.
                                    var valid = (v === "true" ? true : false);

                                    if (!valid)
                                    {
                                        $scope.VCUploadedFile = "Bad format. " + jsonObj.file.name + " does not look like a Voided Checks file.";
                                        FileManager.setVCFileName($scope.VCUploadedFile);
                                    }
                                    else
                                    {
                                        FileManager.setVCUploadFile(jsonObj.file);
                                    }
                                })
                            }
                        }
                    })
                }
                else {
                    $scope.$apply(function (scpe) {
                        $scope.UploadStatus = evt.target.responseText;
                    })
                }
            }

            $scope.QBUpload = function () {
                var fd = new FormData()
                for (var i in $scope.files) {
                    // i is an array index
                    if ($scope.files[i].ftype == 'QB') {
                        fd.append("uploadedFile", $scope.files[i].file);
                        fd.append("ftype", "QB");

                        var xhr = new XMLHttpRequest();
                        xhr.addEventListener("load", QBUploadComplete, false);
                        xhr.open("POST", server + "api/upload/UploadFile", true);
                        $scope.progressVisible = true;
                        xhr.send(fd);
                    }
                }
            }

            function QBUploadComplete(evt) {
                $scope.progressVisible = false;
                if (evt.target.status == 201) {
                    $scope.FilePath = evt.target.responseText;

                    $scope.$apply(function (scpe) {
                        $scope.QBUploadStatus = "Upload Complete";
                        for (var i in $scope.files) {
                            var jsonObj = $scope.files[i];
                            if (jsonObj.ftype == 'QB' & jsonObj.seen == "false") {
                                FileManager.getValidFile('QB', jsonObj.file).then(function (v) {
                                    jsonObj.seen = "true";
                                    $scope.QBUploadedFile = jsonObj.file.name;  // this includes the extension

                                    // Don't know why have to set variable valid, but does not work otherwise.
                                    var valid = (v === "true" ? true : false);

                                    if (!valid) {
                                        $scope.QBUploadedFile = "Bad format. " + jsonObj.file.name + " does not look like a Cleared Checks File.";
                                        FileManager.setQBFileName($scope.QBUploadedFile);
                                    }
                                    else {
                                        FileManager.setQBUploadFile(jsonObj.file);
                                    }
                                })
                            }
                        }
                    })
                }
                else {
                    $scope.$apply(function (scpe) {
                        $scope.UploadStatus = evt.target.responseText;
                    })
                }
            }

            $scope.IMUpload = function () {
                var fd = new FormData()
                for (var i in $scope.files) {
                    // i is an array index
                    if ($scope.files[i].ftype == 'IM') {
                        fd.append("uploadedFile", $scope.files[i].file);
                        fd.append("ftype", "IM");

                        var xhr = new XMLHttpRequest();
                        xhr.addEventListener("load", IMUploadComplete, false);
                        xhr.open("POST", server + "api/upload/UploadFile", true);
                        $scope.progressVisible = true;
                        xhr.send(fd);
                    }
                }
            }

            function IMUploadComplete(evt) {
                $scope.progressVisible = false;
                if (evt.target.status == 201) {
                    $scope.FilePath = evt.target.responseText;

                    $scope.$apply(function (scpe) {
                        $scope.IMUploadStatus = "Upload Complete";
                        for (var i in $scope.files) {
                            var jsonObj = $scope.files[i];
                            if (jsonObj.ftype == 'IM' & jsonObj.seen == "false") {
                                FileManager.getValidFile('IM', jsonObj.file).then(function (v) {
                                    jsonObj.seen = "true";
                                    $scope.IMUploadedFile = jsonObj.file.name;  // this includes the extension

                                    // Don't know why have to set variable valid, but does not work otherwise.
                                    var valid = (v === "true" ? true : false);

                                    if (!valid) {
                                        $scope.IMUploadedFile = "Bad format. " + jsonObj.file.name + " does not look like a ImportMe File.";
                                        FileManager.setIMFileName($scope.IMUploadedFile);
                                    }
                                    else {
                                        FileManager.setIMUploadFile(jsonObj.file);
                                    }
                                })
                            }
                        }
                    })
                }
                else {
                    $scope.$apply(function (scpe) {
                        $scope.UploadStatus = evt.target.responseText;
                    })
                }
            }

            $scope.MRUpload = function () {
                var fd = new FormData()
                for (var i in $scope.files) {
                    // i is an array index
                    if ($scope.files[i].ftype == 'MR') {
                        fd.append("uploadedFile", $scope.files[i].file);
                        fd.append("ftype", "MR");

                        var xhr = new XMLHttpRequest();
                        xhr.addEventListener("load", MRUploadComplete, false);
                        xhr.open("POST", server + "api/upload/UploadFile", true);
                        $scope.progressVisible = true;
                        xhr.send(fd);
                    }
                }
            }

            function MRUploadComplete(evt) {
                $scope.progressVisible = false;
                if (evt.target.status == 201) {
                    $scope.FilePath = evt.target.responseText;

                    $scope.$apply(function (scpe) {
                        $scope.MRUploadStatus = "Upload Complete";
                        for (var i in $scope.files) {
                            var jsonObj = $scope.files[i];
                            if (jsonObj.ftype == 'MR' & jsonObj.seen == "false") {
                                FileManager.getValidFile('MR', jsonObj.file).then(function (v) {
                                    jsonObj.seen = "true";
                                    $scope.MRUploadedFile = jsonObj.file.name;  // this includes the extension

                                    // Don't know why have to set variable valid, but does not work otherwise.
                                    var valid = (v === "true" ? true : false);

                                    if (!valid) {
                                        $scope.MRUploadedFile = "Bad format. " + jsonObj.file.name + " does not look like a Mistakenly Resolved Checks File.";
                                        FileManager.setMRFileName($scope.MRUploadedFile);
                                    }
                                    else {
                                        FileManager.setMRUploadFile(jsonObj.file);
                                    }
                                })
                            }
                        }
                    })
                }
                else {
                    $scope.$apply(function (scpe) {
                        $scope.UploadStatus = evt.target.responseText;
                    })
                }
            }

            $scope.RRCUpload = function () {
                var fd = new FormData()
                for (var i in $scope.files) {
                    // i is an array index
                    if ($scope.files[i].ftype == 'RRC') {
                        fd.append("uploadedFile", $scope.files[i].file);
                        fd.append("ftype", "RRC");

                        var xhr = new XMLHttpRequest();
                        xhr.addEventListener("load", RRCUploadComplete, false);
                        xhr.open("POST", server + "api/upload/UploadFile", true);
                        $scope.progressVisible = true;
                        xhr.send(fd);
                    }
                }
            }

            function RRCUploadComplete(evt) {
                $scope.progressVisible = false;
                if (evt.target.status == 201) {
                    $scope.FilePath = evt.target.responseText;

                    $scope.$apply(function (scpe) {
                        $scope.RRCUploadStatus = "Upload Complete";
                        for (var i in $scope.files) {
                            var jsonObj = $scope.files[i];
                            if (jsonObj.ftype == 'RRC' & jsonObj.seen == "false") {
                                FileManager.getValidFile('RRC', jsonObj.file).then(function (v) {
                                    jsonObj.seen = "true";
                                    $scope.RRCUploadedFile = jsonObj.file.name;  // this includes the extension

                                    // Don't know why have to set variable valid, but does not work otherwise.
                                    var valid = (v === "true" ? true : false);

                                    if (!valid) {
                                        $scope.RRCUploadedFile = "Bad format. " + jsonObj.file.name + " does not look like a Re-Resolve Cleared Checks File.";
                                        FileManager.setRRCFileName($scope.RRCUploadedFile);
                                    }
                                    else {
                                        FileManager.setRRCUploadFile(jsonObj.file);
                                    }
                                })
                            }
                        }
                    })
                }
                else {
                    $scope.$apply(function (scpe) {
                        $scope.UploadStatus = evt.target.responseText;
                    })
                }
            }

            $scope.RRVUpload = function () {
                var fd = new FormData()
                for (var i in $scope.files) {
                    // i is an array index
                    if ($scope.files[i].ftype == 'RRV') {
                        fd.append("uploadedFile", $scope.files[i].file);
                        fd.append("ftype", "RRV");

                        var xhr = new XMLHttpRequest();
                        xhr.addEventListener("load", RRVUploadComplete, false);
                        xhr.open("POST", server + "api/upload/UploadFile", true);
                        $scope.progressVisible = true;
                        xhr.send(fd);
                    }
                }
            }

            function RRVUploadComplete(evt) {
                $scope.progressVisible = false;
                if (evt.target.status == 201) {
                    $scope.FilePath = evt.target.responseText;

                    $scope.$apply(function (scpe) {
                        $scope.RRVUploadStatus = "Upload Complete";
                        for (var i in $scope.files) {
                            var jsonObj = $scope.files[i];
                            if (jsonObj.ftype == 'RRV' & jsonObj.seen == "false") {
                                FileManager.getValidFile('RRV', jsonObj.file).then(function (v) {
                                    jsonObj.seen = "true";
                                    $scope.RRVUploadedFile = jsonObj.file.name;  // this includes the extension

                                    // Don't know why have to set variable valid, but does not work otherwise.
                                    var valid = (v === "true" ? true : false);

                                    if (!valid) {
                                        $scope.RRVUploadedFile = "Bad format. " + jsonObj.file.name + " does not look like a Re-Resolve Voided Checks File.";
                                        FileManager.setRRVFileName($scope.RRVUploadedFile);
                                    }
                                    else {
                                        FileManager.setRRVUploadFile(jsonObj.file);
                                    }
                                })
                            }
                        }
                    })
                }
                else {
                    $scope.$apply(function (scpe) {
                        $scope.UploadStatus = evt.target.responseText;
                    })
                }
            }

            $scope.APUpload = function () {
                var fd = new FormData()
                for (var i in $scope.files) {
                    // i as an arry index
                    if ($scope.files[i].ftype == 'AP')
                    {
                        fd.append("uploadedFile", $scope.files[i].file);
                        fd.append("ftype", "AP");
                        var xhr = new XMLHttpRequest();
                        xhr.addEventListener("load", APUploadComplete, false);
                        xhr.open("POST", server + "api/upload/UploadFile", true);
                        $scope.progressVisible = true;
                        xhr.send(fd);
                    }
                }
            }

            function APUploadComplete(evt) {
                $scope.progressVisible = false;
                if (evt.target.status == 201) {
                    $scope.FilePath = evt.target.responseText;

                    $scope.$apply(function (scpe) {
                        $scope.APUploadStatus = "Upload Complete";
                        for (var i in $scope.files) {
                            var jsonObj = $scope.files[i];
                            if (jsonObj.ftype == 'AP' & jsonObj.seen == "false") {
                                FileManager.getValidFile('AP', jsonObj.file).then(function (v) {
                                    jsonObj.seen = "true";
                                    $scope.APUploadedFile = jsonObj.file.name;  // this includes the extension

                                    // Don't know why have to set variable valid, but does not work otherwise.
                                    var valid = (v === "true" ? true : false);

                                    if (!valid) {
                                        $scope.APUploadedFile = "Bad format. " + jsonObj.file.name + " does not look like an Interview Research File.";
                                        FileManager.setAPFileName($scope.APUploadedFile);
                                    }
                                    else {
                                        FileManager.setAPUploadFile(jsonObj.file);
                                    }
                                })
                            }
                        }
                    })
                }
                else {
                    $scope.$apply(function (scpe) {
                        $scope.UploadStatus = evt.target.responseText;
                    })
                }
            }

            $scope.MDUpload = function () {
                var fd = new FormData()
                for (var i in $scope.files) {
                    // i as an arry index
                    if ($scope.files[i].ftype == 'MD') {
                        fd.append("uploadedFile", $scope.files[i].file);
                        fd.append("ftype", "MD");

                        var xhr = new XMLHttpRequest();
                        xhr.addEventListener("load", MDUploadComplete, false);
                        xhr.open("POST", server + "api/upload/UploadFile", true);
                        $scope.progressVisible = true;
                        xhr.send(fd);
                    }
                }
            }

            function MDUploadComplete(evt) {
                $scope.progressVisible = false;
                if (evt.target.status == 201) {
                    $scope.FilePath = evt.target.responseText;

                    $scope.$apply(function (scpe) {
                        $scope.MDUploadStatus = "Upload Complete";
                        for (var i in $scope.files) {
                            var jsonObj = $scope.files[i];
                            if (jsonObj.ftype == 'MD' & jsonObj.seen == "false") {
                                FileManager.getValidFile('MD', jsonObj.file).then(function (v) {
                                    jsonObj.seen = "true";
                                    $scope.MDUploadedFile = jsonObj.file.name;  // this includes the extension

                                    // Don't know why have to set variable valid, but does not work otherwise.
                                    var valid = (v === "true" ? true : false);

                                    if (!valid) {
                                        $scope.MDUploadedFile = "Bad format. " + jsonObj.file.name + " does not look like a Modifications Research File.";
                                        FileManager.setMDFileName($scope.MDUploadedFile);
                                    }
                                    else {
                                        FileManager.setMDUploadFile(jsonObj.file);
                                    }
                                })
                            }
                        }
                    })
                }
                else {
                    $scope.$apply(function (scpe) {
                        $scope.UploadStatus = evt.target.responseText;
                    })
                }
            }

            $scope.GetFileType = function (fileExtension) {
                switch (fileExtension.toLowerCase()) {
                    case "doc":
                    case "docx":
                        $scope.FileType = "application/msword";
                        break;
                    case "xls":
                    case "xlsx":
                        $scope.FileType = "application/vnd.ms-excel";
                        break;
                    case "pps":
                    case "ppt":
                        $scope.FileType = "application/vnd.ms-powerpoint";
                        break;
                    case "txt":
                        $scope.FileType = "text/plain";
                        break;
                    case "rtf":
                        $scope.FileType = "application/rtf";
                        break;
                    case "pdf":
                        $scope.FileType = "application/pdf";
                        break;
                    case "msg":
                    case "eml":
                        $scope.FileType = "application/vnd.ms-outlook";
                        break;
                    case "gif":
                    case "bmp":
                    case "png":
                    case "jpg":
                        $scope.FileType = "image/JPEG";
                        break;
                    case "dwg":
                        $scope.FileType = "application/acad";
                        break;
                    case "zip":
                        $scope.FileType = "application/x-zip-compressed";
                        break;
                    case "rar":
                        $scope.FileType = "application/x-rar-compressed";
                        break;
                }
            }

            $scope.setFiles = function (type, element) {
                // alert("setFiles: type = " + type);
                $scope.$apply(function (scpe) {
                    for (var i = 0; i < element.files.length; i++) {
                        $scope.files.push({ ftype: type, file: element.files[i], seen: "false" });
                    };
                    $scope.progressVisible = false
                });
            }

            $scope.Merge = function () { // called when the Merge button is clicked on file merge.html
               
                var apFileName = FileManager.getAPFileName();
                var apFileType;

                if (apFileName == 'unknown') {
                    apFileType = "xslx";
                }
                else {
                    apFileType = FileManager.getAPFileType();
                }

                var mdFileName = FileManager.getMDFileName();
                var mdFileType;

                if (mdFileName == 'unknown') {
                    mdFileType = "xslx";
                }
                else {
                    mdFileType = FileManager.getMDFileType();
                }

                var vcFileName = FileManager.getVCFileName();
                var vcFileType;

                if (vcFileName == 'unknown')
                {
                    vcFileType = "xlsx";
                }
                else
                {
                   vcFileType = FileManager.getVCFileType();
                }
                
                var qbFileName = FileManager.getQBFileName();
                var qbFileType;

                if (qbFileName == 'unknown') {
                    qbFileType = "xlsx";
                }
                else {
                    qbFileType = FileManager.getQBFileType();
                }

                var imFileName = FileManager.getIMFileName();
                var imFileType;

                if (imFileName == 'unknown') {
                    imFileType = "xlsx";
                }
                else {
                    imFileType = FileManager.getIMFileType();
                }

                var mrFileName = FileManager.getMRFileName();
                var mrFileType;

                if (mrFileName == 'unknown') {
                    mrFileType = "xlsx";
                }
                else {
                    mrFileType = FileManager.getMRFileType();
                }

                var rrcFileName = FileManager.getRRCFileName();
                var rrcFileType;

                if (rrcFileName == 'unknown') {
                    rrcFileType = "xlsx";
                }
                else {
                    rrcFileType = FileManager.getRRCFileType();
                }

                var rrvFileName = FileManager.getRRVFileName();
                var rrvFileType;

                if (rrvFileName == 'unknown') {
                    rrvFileType = "xlsx";
                }
                else {
                    rrvFileType = FileManager.getRRVFileType();
                }

                if (apFileName != 'unknown' && mdFileName != 'unknown') {
                    alert("Cannot merge two Research Files at the same time. Please use the browser reload button to reset.")
                }
                else if (apFileName != 'unknown' && (vcFileName != 'unknown' || qbFileName != 'unknown' || imFileName != 'unknown')) {
                    alert("Cannot merge both a Research File and a Disposition File at the same time. Please use the browser reload button to reset.")
                }
                else {
                    $scope.mergeStatus = "Merging...";

                    MergeManager.merge(vcFileName, vcFileType, apFileName, apFileType, mdFileName, mdFileType, qbFileName, qbFileType, imFileName, imFileType, mrFileName, mrFileType, rrcFileName, rrcFileType, rrvFileName, rrvFileType).then(function (ms) {
                        $scope.mergeStatus = "Merge completed";
                  });
                }
            }
        }
]);

 



MSMApp.controller('researchController', ['$rootScope', '$scope', '$http', '$window', '$route', 'FileManager', 'ResearchManager', 'DTOptionsBuilder', 'DTColumnBuilder',
        function ($rootScope, $scope, $http, $window, $route, FileManager, ResearchManager, DTOptionsBuilder, DTColumnBuilder) {
            $scope.tab = 'research';

            var timestampPromise = FileManager.getDownloadTimestamp();

            timestampPromise.then(function (d) {
                // Example: d = ""22-Nov-2016-0941""
                // Yes, really - d is a string inside a string!
                // Use the substr operator to extract the inside string.
                // This is safe since by construction the string will always have the same length.
                $scope.timestamp = d.substr(1, 16);

                $rootScope.pageTitle = "Research " + $scope.timestamp;
                $scope.mergeStatus = "";
                $scope.files = [];
                $scope.RTUploadedFile = FileManager.getRTFileName() + "." + FileManager.getRTFileType();
            })

            $scope.integerval = /^-?\d*$/;
            $scope.resolvedCheck = "";
            $scope.ResolvedStatus = ResearchManager.getResolvedStatus();

            $scope.ResolveCheck = function () {
                //  console.log("Resolved check: " + $scope.resolvedCheck);
                ResearchManager.resolve($scope.resolvedCheck).then(function (r) {
                    ResearchManager.setResolvedStatus(r);
                    $route.reload();
                })   
            }

            $scope.InterviewStaleChecks = function () {
                var filePromise = FileManager.getDownloadStaleChecks("interviewstale", "csv");

                filePromise.then(function (result) {

                    ResearchManager.markStaleChecks("interview");

                    var textToWrite = result;
                    // alert("download = " + textToWrite);
                    // $window.open(textToWrite);

                    // From: http://stackoverflow.com/questions/34870711/download-a-file-at-different-location-using-html5
                    var textFileAsBlob = new Blob([textToWrite], { type: 'text/plain' });

                    var downloadLink = document.createElement("a");
                    downloadLink.download = "interview-stalechecks-" + $scope.timestamp + ".csv";

                    downloadLink.innerHtml = "Download Interview Stale Checks";

                    if ($window.URL != null) {
                        //  console.log("Download using Chrome");
                        downloadLink.href = window.URL.createObjectURL(textFileAsBlob);
                    }
                    else {
                        // alert("Firefox!");
                        // Firefox requires the link to be added to the DOM
                        // before it can be clicked.
                        downloadLink.href = window.URL.createObjectURL(textFileAsBlob);
                        downloadLink.onclick = destroyClickeElement;
                        downloadLink.style.display = "none";
                        document.body.appendChild(downloadLink);
                    }

                    downloadLink.click();
                })
            }

            $scope.ModificationsStaleChecks = function () {
                var filePromise = FileManager.getDownloadStaleChecks("modificationsstale", "csv");

                filePromise.then(function (result) {

                    ResearchManager.markStaleChecks("modification");

                    var textToWrite = result;
                    // alert("download = " + textToWrite);
                    // $window.open(textToWrite);

                    // From: http://stackoverflow.com/questions/34870711/download-a-file-at-different-location-using-html5
                    var textFileAsBlob = new Blob([textToWrite], { type: 'text/plain' });

                    var downloadLink = document.createElement("a");
                    downloadLink.download = "modifications-stalechecks-" + $scope.timestamp + ".csv";

                    downloadLink.innerHtml = "Download Modifications IMPORTME File";

                    if ($window.URL != null) {
                        //  console.log("Download using Chrome");
                        downloadLink.href = window.URL.createObjectURL(textFileAsBlob);
                    }
                    else {
                        // alert("Firefox!");
                        // Firefox requires the link to be added to the DOM
                        // before it can be clicked.
                        downloadLink.href = window.URL.createObjectURL(textFileAsBlob);
                        downloadLink.onclick = destroyClickeElement;
                        downloadLink.style.display = "none";
                        document.body.appendChild(downloadLink);
                    }

                    downloadLink.click();
                })
            }

            $scope.RTUpload = function () {
                var fd = new FormData()
                for (var i in $scope.files) {
                    // i as an arry index
                    if ($scope.files[i].ftype == 'RT') {
                        fd.append("uploadedFile", $scope.files[i].file);
                        fd.append("ftype", "RT");
                        var xhr = new XMLHttpRequest();
                        xhr.addEventListener("load", RTUploadComplete, false);
                        xhr.open("POST", server + "api/upload/UploadFile", true);
                        $scope.progressVisible = true;
                        xhr.send(fd);
                    }
                }
            }

            function RTUploadComplete(evt) {
                $scope.progressVisible = false;
                if (evt.target.status == 201) {
                    $scope.FilePath = evt.target.responseText;

                    $scope.$apply(function (scpe) {
                        $scope.RTUploadStatus = "Upload Complete";
                        for (var i in $scope.files) {
                            var jsonObj = $scope.files[i];
                            if (jsonObj.ftype == 'RT' & jsonObj.seen == "false") {
                                FileManager.getValidFile('RT', jsonObj.file).then(function (v) {
                                    jsonObj.seen = "true";
                                    $scope.RTUploadedFile = jsonObj.file.name;  // this includes the extension

                                    // Don't know why have to set variable valid, but does not work otherwise.
                                    var valid = (v === "true" ? true : false);

                                    if (!valid) {
                                        $scope.RTUploadedFile = "Bad format. " + jsonObj.file.name + " does not look like a Research Table File.";
                                        FileManager.setRTFileName($scope.RTUploadedFile);
                                    }
                                    else {
                                        FileManager.setRTUploadFile(jsonObj.file);
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

            // Same as setFIles on mergeController.js
            $scope.setFiles = function (type, element) {
                // alert("setFiles: type = " + type);
                $scope.$apply(function (scpe) {
                    for (var i = 0; i < element.files.length; i++) {
                        $scope.files.push({ ftype: type, file: element.files[i], seen: "false" });
                    };
                    $scope.progressVisible = false
                });
            }

            $scope.RestoreFromBackup = function () {
                var rtFileName = FileManager.getRTFileName();
                var rtFileType;

                if (rtFileName == 'unknown') {
                    rtFileType = "xslx";
                }
                else {
                    rtFileType = FileManager.getRTFileType();
                }

                ResearchManager.researchTableEmpty().then(function(result)
                {
                    if (result == '"empty"') { // Note that result is a string within a string!
                        $scope.restorationStatus = "Restoring...";

                        ResearchManager.restore(rtFileName, rtFileType).then(function (rs) {
                            $scope.restorationStatus = "Restoration complete";
                        });
                    } else {
                        alert("Research Table Must Be Empty Before Restoring From Backup!");
                    }
                }) 
            }
        }]);
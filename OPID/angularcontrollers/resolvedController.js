

MSMApp.controller('resolvedController', ['$rootScope', '$scope', '$http', '$window', 'FileManager', 'DTOptionsBuilder', 'DTColumnBuilder',
        function ($rootScope, $scope, $http, $window, FileManager, DTOptionsBuilder, DTColumnBuilder) {
           
            $scope.tab = 'resolved';

            var timestampPromise = FileManager.getDownloadTimestamp();

            timestampPromise.then(function (d) {
                // Example: d = ""22-Nov-2016-0941""
                // Yes, really - d is a string inside a string!
                // Use the substr operator to extract the inside string.
                // This is safe since by construction the string will always have the same length.
                $scope.timestamp = d.substr(1, 16);

                $rootScope.pageTitle = "Resolved " + $scope.timestamp;
            })
       
            $scope.InterviewDownload = function () {

                var filePromise = FileManager.getDownloadFile("interview", "csv");

                filePromise.then(function (result) {

                    var textToWrite = result;
                    // alert("download = " + textToWrite);
                    // $window.open(textToWrite);

                    // From: http://stackoverflow.com/questions/34870711/download-a-file-at-different-location-using-html5
                    var textFileAsBlob = new Blob([textToWrite], { type: 'text/plain' });

                    var downloadLink = document.createElement("a");
                    downloadLink.download = "interview-importme-" + $scope.timestamp + ".csv";

                    downloadLink.innerHtml = "Download Interview IMPORTME File";

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

            $scope.ModificationsDownload = function () {

                var filePromise = FileManager.getDownloadFile("modifications", "csv");

                filePromise.then(function (result) {

                    var textToWrite = result;
                    // alert("download = " + textToWrite);
                    // $window.open(textToWrite);

                    // From: http://stackoverflow.com/questions/34870711/download-a-file-at-different-location-using-html5
                    var textFileAsBlob = new Blob([textToWrite], { type: 'text/plain' });

                    var downloadLink = document.createElement("a");
                    downloadLink.download = "modifications-importme-" + $scope.timestamp + ".csv";

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
        }]);
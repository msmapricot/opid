

MSMApp.controller('typosController', ['$rootScope', '$scope', '$http', '$window', 'FileManager', 'DTOptionsBuilder', 'DTColumnBuilder',
        function ($rootScope, $scope, $http, $window, FileManager, DTOptionsBuilder, DTColumnBuilder) {
           
            $scope.tab = 'typos';

            var timestampPromise = FileManager.getTyposTimestamp();

            timestampPromise.then(function (d) {
                // Example: d = ""22-Nov-2016-0941""
                // Yes, really - d is a string inside a string!
                // Use the substr operator to extract the inside string.
                // This is safe since by construction the string will always have the same length.
                $scope.timestamp = d.substr(1, 16);

                $rootScope.pageTitle = "Typos " + $scope.timestamp;
            })
        }]);
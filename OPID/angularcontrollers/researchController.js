
MSMApp.controller('researchController', ['$rootScope', '$scope', '$http', '$route', 'FileManager', 'ResearchManager', 'DTOptionsBuilder', 'DTColumnBuilder',
        function ($rootScope, $scope, $http, $route, FileManager, ResearchManager, DTOptionsBuilder, DTColumnBuilder) {
            $scope.tab = 'research';
            var timestampPromise = FileManager.getResearchTimestamp();

            timestampPromise.then(function (d) {
                // Example: d = ""22-Nov-2016-0941""
                // Yes, really - d is a string inside a string!
                // Use the substr operator to extract the inside string.
                // This is safe since by construction the string will always have the same length.
                $scope.timestamp = d.substr(1, 16);

                $rootScope.pageTitle = "Research " + $scope.timestamp;
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
        }]);
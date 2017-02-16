﻿
ResearchServices.factory('ResearchManager', ['$http', function ($http) {
    var resolve = function (checkNum) {
        return $http.get(server + "api/resolvecheck/" + checkNum).then(function (result) {
            return result.data;
        });
    };

    var markStaleChecks = function (type) {
        return $http.get(server + "api/markstalechecks/" + type);
    }

    var resolvedStatus = "";

    var getResolvedStatus = function()
    {
        return resolvedStatus;
    }

    var setResolvedStatus = function(r)
    {
        resolvedStatus = r;
    }

    var restoreResearchTable = function (rtFileName, rtFileType) {
        return $http.get(server + "api/restore",
            {
                params:
                    {
                        "rtFileName": rtFileName,
                        "rtFileType": rtFileType
                    }
            }).then(function (result) {
                return result.data;
            });
    };

    return {
        resolve: resolve,
        getResolvedStatus: getResolvedStatus,
        setResolvedStatus: setResolvedStatus,
        markStaleChecks: markStaleChecks,
        restore: restoreResearchTable
    };
}]);
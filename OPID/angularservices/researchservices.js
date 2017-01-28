
ResearchServices.factory('ResearchManager', ['$http', function ($http) {
    var resolve = function (checkNum) {
        return $http.get(server + "api/resolvecheck/" + checkNum).then(function (result) {
            return result.data;
        });
    };

    var resolvedStatus = "";

    var getResolvedStatus = function()
    {
        return resolvedStatus;
    }

    var setResolvedStatus = function(r)
    {
        resolvedStatus = r;
    }

    return {
        resolve: resolve,
        getResolvedStatus: getResolvedStatus,
        setResolvedStatus: setResolvedStatus,    
    };
}]);
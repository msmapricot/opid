
MergeServices.factory('MergeManager', ['$http', function ($http) {

    var merge = function (vcFileName, vcFileType, apFileName, apFileType, mdFileName, mdFileType, qbFileName, qbFileType, mrFileName, mrFileType, rrcFileName, rrcFileType, rrvFileName, rrvFileType) {
        return $http.get(server + "api/merge",
            {
                params:
                   {
                       "vcFileName": vcFileName,
                       "vcFileType": vcFileType,
                       "apFileName": apFileName,
                       "apFileType": apFileType,
                       "mdFileName": mdFileName,
                       "mdFileType": mdFileType,
                       "qbFileName": qbFileName,
                       "qbFileType": qbFileType,
                       "mrFileName": mrFileName,
                       "mrFileType": mrFileType,
                       "rrcFileName": rrcFileName,
                       "rrcFileType": rrcFileType,
                       "rrvFileName": rrvFileName,
                       "rrvFileType": rrvFileType
                   }
            }).then(function (result) {
                return result.data;
            });   
    };
     
    return {
        merge: merge
    };
}]);
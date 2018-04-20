
FileServices.factory('FileManager', ['$http', function ($http) {
    var getDownloadFile = function (fileName, fileType) {
       // return $http.get(server + "api/downloadimportme").then(function (result) {
        return $http.get(server + "api/downloadimportme", { params: { "fileName": fileName, "fileType": fileType } }).then(function (result) {
            return result.data;
        })
    };

    var getDownloadStaleChecks = function (fileName, fileType) {
        return $http.get(server + "api/downloadstalechecks", { params: { "fileName": fileName, "fileType": fileType } }).then(function (result) {
            return result.data;
        })
    };

    var uploadedFiles = {};
    var menuFiles = [];

    var addToMenuFiles = function (ftype, fname, extension)
    {
        fileName = fname + "." + extension;
        menuFiles.push({type: ftype, file: fileName});
    }

    var setQBUploadFile = function (fileObj) {
        var fparts = fileObj.name.split(".");
        var fname = fparts[0];
        var ftype = fparts[1];
        var qbUploaded = uploadedFiles.qbFileName;

        if (qbUploaded == undefined || qbUploaded != fname) {
            uploadedFiles.qbFileName = fname;
            uploadedFiles.qbFileType = ftype;
            addToMenuFiles("Quickbooks", fname, ftype);
        }
    }

    var getQBFileName = function () {
        if (uploadedFiles.qbFileName == undefined)
        {
            return "unknown";
        }

        return uploadedFiles.qbFileName;
    }

    var setQBFileName = function (name) {
        uploadedFiles.qbFileName = name;
    }

    var getQBFileType = function () {
        if (uploadedFiles.qbFileType == undefined)
        {
            return "";
        }

        return uploadedFiles.qbFileType;
    }

    var setQBUploadFile = function (fileObj) {
        var fparts = fileObj.name.split(".");
        var fname = fparts[0];
        var ftype = fparts[1];
        var qbUploaded = uploadedFiles.qbFileName;

        if (qbUploaded == undefined || qbUploaded != fname) {
            uploadedFiles.qbFileName = fname;
            uploadedFiles.qbFileType = ftype;
            addToMenuFiles("Quickbooks", fname, ftype);
        }
    }

    // Mistakenly resolved 
    var getMRFileName = function () {
        if (uploadedFiles.mrFileName == undefined) {
            return "unknown";
        }

        return uploadedFiles.mrFileName;
    }

    var setMRFileName = function (name) {
        uploadedFiles.mrFileName = name;
    }

    var getMRFileType = function () {
        if (uploadedFiles.mrFileType == undefined) {
            return "";
        }

        return uploadedFiles.mrFileType;
    }

    var setMRUploadFile = function (fileObj) {
        var fparts = fileObj.name.split(".");
        var fname = fparts[0];
        var ftype = fparts[1];
        var mrUploaded = uploadedFiles.mrFileName;

        if (mrUploaded == undefined || mrUploaded != fname) {
            uploadedFiles.mrFileName = fname;
            uploadedFiles.mrFileType = ftype;
            addToMenuFiles("MistakenlyResolved", fname, ftype);
        }
    }
    // End Mistakenly resolved

    // Re-resolve cleared
    var getRRCFileName = function () {
        if (uploadedFiles.rrcFileName == undefined) {
            return "unknown";
        }

        return uploadedFiles.rrcFileName;
    }

    var setRRCFileName = function (name) {
        uploadedFiles.rrcFileName = name;
    }

    var getRRCFileType = function () {
        if (uploadedFiles.rrcFileType == undefined) {
            return "";
        }

        return uploadedFiles.rrcFileType;
    }

    var setRRCUploadFile = function (fileObj) {
        var fparts = fileObj.name.split(".");
        var fname = fparts[0];
        var ftype = fparts[1];
        var rrcUploaded = uploadedFiles.rrcFileName;

        if (rrcUploaded == undefined || rrcUploaded != fname) {
            uploadedFiles.rrcFileName = fname;
            uploadedFiles.rrcFileType = ftype;
            addToMenuFiles("ReResolveCleared", fname, ftype);
        }
    }
    // End Re-resolve cleared

    // Re-resolve voided
    var getRRVFileName = function () {
        if (uploadedFiles.rrvFileName == undefined) {
            return "unknown";
        }

        return uploadedFiles.rrvFileName;
    }

    var setRRVFileName = function (name) {
        uploadedFiles.rrvFileName = name;
    }

    var getRRVFileType = function () {
        if (uploadedFiles.rrvFileType == undefined) {
            return "";
        }

        return uploadedFiles.rrvFileType;
    }

    var setRRVUploadFile = function (fileObj) {
        var fparts = fileObj.name.split(".");
        var fname = fparts[0];
        var ftype = fparts[1];
        var rrvUploaded = uploadedFiles.rrvFileName;

        if (rrvUploaded == undefined || rrvUploaded != fname) {
            uploadedFiles.rrvFileName = fname;
            uploadedFiles.rrvFileType = ftype;
            addToMenuFiles("ReResolveVoided", fname, ftype);
        }
    }
    // End Re-resolve voided

    var setAPUploadFile = function (fileObj) {
        var fparts = fileObj.name.split(".");
        var fname = fparts[0];
        var ftype = fparts[1];
        var apUploaded = uploadedFiles.apFileName;

        if (apUploaded == undefined || apUploaded != fname) {
            uploadedFiles.apFileName = fname;
            uploadedFiles.apFileType = ftype;
            addToMenuFiles("Research", fname, ftype);
        }
    }

    var setRTUploadFile = function (fileObj) {
        var fparts = fileObj.name.split(".");
        var fname = fparts[0];
        var ftype = fparts[1];
        var rtUploaded = uploadedFiles.rtFileName;

        if (rtUploaded == undefined || rtUploaded != fname) {
            uploadedFiles.rtFileName = fname;
            uploadedFiles.rtFileType = ftype;
            addToMenuFiles("Restore", fname, ftype);
        }
    }

    var setMDUploadFile = function (fileObj) {
        var fparts = fileObj.name.split(".");
        var fname = fparts[0];
        var ftype = fparts[1];
        var mdUploaded = uploadedFiles.mdFileName;

        if (mdUploaded == undefined || mdUploaded != fname) {
            uploadedFiles.mdFileName = fname;
            uploadedFiles.mdFileType = ftype;
            addToMenuFiles("Modifications", fname, ftype);
        }
    }

    var setVCUploadFile = function (fileObj) {
        var fparts = fileObj.name.split(".");
        var fname = fparts[0];
        var ftype = fparts[1];
        var vcUploaded = uploadedFiles.vcFileName;

        if (vcUploaded == undefined || vcUploaded != fname) {
            uploadedFiles.vcFileName = fname;
            uploadedFiles.vcFileType = ftype;
            addToMenuFiles("Voidedchecks", fname, ftype);
        }
    }

    var setSelectedFile = function(fname)
    {
        uploadedFiles.selectedFile = fname;
    }

    var getAPFileName = function () {
        if (uploadedFiles.apFileName == undefined) {
            return "unknown";
        };

        return uploadedFiles.apFileName;
    }

    var setAPFileName = function (name) {
        uploadedFiles.apFileName = name;
    }
   
    var getAPFileType = function () {
        if (uploadedFiles.apFileType == undefined)
        {
            return "";
        }
        return uploadedFiles.apFileType;
    }

    var getRTFileName = function () {
        if (uploadedFiles.rtFileName == undefined) {
            return "unknown";
        };

        return uploadedFiles.rtFileName;
    }

    var setRTFileName = function (name) {
        uploadedFiles.rtFileName = name;
    }

    var getRTFileType = function () {
        if (uploadedFiles.rtFileType == undefined) {
            return "";
        }
        return uploadedFiles.rtFileType;
    }

    var getMDFileName = function () {
        if (uploadedFiles.mdFileName == undefined) {
            return "unknown";
        };

        return uploadedFiles.mdFileName;
    }

    var setMDFileName = function (name) {
        uploadedFiles.mdFileName = name;
    }

    var getMDFileType = function () {
        if (uploadedFiles.mdFileType == undefined) {
            return "";
        }
        return uploadedFiles.mdFileType;
    }

    var getVCFileName = function () {
        if (uploadedFiles.vcFileName == undefined)
        {
            return "unknown";
        }

        return uploadedFiles.vcFileName;
    }

    var setVCFileName = function(name) {
        uploadedFiles.vcFileName = name;
    }

    var getVCFileType = function () {
        if (uploadedFiles.vcFileType == undefined)
        {
            return "";
        }
        return uploadedFiles.vcFileType;
    }

    var getMenuFiles = function () {
        return menuFiles;
    }

    var getSelectedFile = function() {
        return uploadedFiles.selectedFile;
    }

    var getValidFile = function(ftype, fileObj)
    {
        var fparts = fileObj.name.split(".");
        var fname = fparts[0];
        var fext = fparts[1];
        return $http.get(server + "api/checkvalidity",
            { params: { "ftype": ftype, "fname": fname, "fext": fext } }).then(function (result) {
                return result.data;
            })
    }

    var getTyposTimestamp = function () {
        return $http.get(server + "api/typostimestamp").then(function (result) {
            return result.data;
        })
    }

    var getDownloadTimestamp = function()
    {
       return $http.get(server + "api/downloadtimestamp").then(function (result) {
            return result.data;
        })
    }

    var getResearchTimestamp = function () {
        return $http.get(server + "api/researchtimestamp").then(function (result) {
            return result.data;
        })
    }
    
    return {
        getDownloadFile: getDownloadFile,
        getDownloadStaleChecks : getDownloadStaleChecks,

        getQBFileName: getQBFileName,
        setQBFileName: setQBFileName,
        getQBFileType: getQBFileType,
        setQBUploadFile: setQBUploadFile,

        getMRFileName: getMRFileName,
        setMRFileName: setMRFileName,
        getMRFileType: getMRFileType,
        setMRUploadFile: setMRUploadFile,

        getRRCFileName: getRRCFileName,
        setRRCFileName: setRRCFileName,
        getRRCFileType: getRRCFileType,
        setRRCUploadFile: setRRCUploadFile,

        getRRVFileName: getRRVFileName,
        setRRVFileName: setRRVFileName,
        getRRVFileType: getRRVFileType,
        setRRVUploadFile: setRRVUploadFile,

        getAPFileName: getAPFileName,
        setAPFileName: setAPFileName,
        getAPFileType: getAPFileType,
        setAPUploadFile: setAPUploadFile,

        getMDFileName: getMDFileName,
        setMDFileName: setMDFileName,
        getMDFileType: getMDFileType,
        setMDUploadFile: setMDUploadFile,
      
        getVCFileName: getVCFileName,
        setVCFileName : setVCFileName,
        getVCFileType: getVCFileType,
        setVCUploadFile: setVCUploadFile,

        getMenuFiles: getMenuFiles,
        setSelectedFile: setSelectedFile,
        getSelectedFile: getSelectedFile,
        getValidFile: getValidFile,

        getTyposTimestamp: getTyposTimestamp,
        getDownloadTimestamp: getDownloadTimestamp,
        getResearchTimestamp: getResearchTimestamp,

        getRTFileName: getRTFileName,
        getRTFileType: getRTFileType,
        setRTFileName: setRTFileName,
        setRTUploadFile: setRTUploadFile
    };
}]);


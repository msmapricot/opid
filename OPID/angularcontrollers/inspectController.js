
MSMApp.controller('inspectController', inspectController);

function inspectController($rootScope, $scope, $http, $q, $route, FileManager) {

    var vm = this;

    $rootScope.pageTitle = "Main Street Ministries - Inspect";
    vm.menuFiles = FileManager.getMenuFiles();
    $scope.tab = 'inspect';


    vm.changedValue = function () {

        if (vm.selectedFile == "Quickbooks") {
            FileManager.setSelectedFile("Quickbooks");
            $route.reload();
        }
        else if (vm.selectedFile == "ImportMe") {
            FileManager.setSelectedFile("ImportMe");
            $route.reload();
        }
        else if (vm.selectedFile == "MistakenlyResolved") {
            FileManager.setSelectedFile("MistakenlyResolved");
            $route.reload();
        }
        else if (vm.selectedFile == "Research") {
            FileManager.setSelectedFile("Research");
            $route.reload();
        }
        else if (vm.selectedFile == "Modifications") {
            FileManager.setSelectedFile("Modifications");
            $route.reload();
        }
        else if (vm.selectedFile == "Voidedchecks") {
            FileManager.setSelectedFile("Voidedchecks");
            $route.reload();
        }
        else {
            alert("Could not inspect selected file");
        }
    }
}
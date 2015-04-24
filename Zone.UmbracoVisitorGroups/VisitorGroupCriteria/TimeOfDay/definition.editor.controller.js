angular.module("umbraco")
    .controller("UmbracoVisitorGroups.TimeOfDayVisitorGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = {};
            $scope.renderModel.periods = [];

            if ($scope.dialogOptions.definition) {
                $scope.renderModel.periods = JSON.parse($scope.dialogOptions.definition);
            }

            $scope.newFrom = "";
            $scope.newTo = "";
            $scope.currentEditPeriod = null;
            $scope.hasError = false;

            function isNumber(n) {
                return !isNaN(parseFloat(n)) && isFinite(n);
            };
            
            function isValidPeriod(from, to) {
                return isNumber(from) && isNumber(to) &&
                    parseInt(from) >= 0 && parseInt(from) < 2359 &&
                    parseInt(to) >= 0 && parseInt(to) < 2359 &&
                    parseInt(to) > parseInt(from);
            };

            $scope.edit = function (index) {
                for (var i = 0; i < $scope.renderModel.periods.length; i++) {
                    $scope.renderModel.periods[i].edit = false;
                }

                $scope.renderModel.periods[index].edit = true;
            };

            $scope.saveEdit = function (index) {
                $scope.renderModel.periods[index].edit = false;
            };

            $scope.delete = function (index) {
                $scope.renderModel.periods.splice(index, 1);
            };

            $scope.add = function () {
                if (isValidPeriod($scope.newFrom, $scope.newTo)) {
                    var period = { from: $scope.newFrom, to: $scope.newTo };
                    $scope.renderModel.periods.push(period);

                    $scope.newFrom = "";
                    $scope.newTo = "";
                } else {
                    $scope.hasError = true;
                }
            };

            $scope.saveAndClose = function () {
                var serializedResult = "[";

                for (var i = 0; i < $scope.renderModel.periods.length; i++) {
                    if (serializedResult.length > 1) {
                        serializedResult += ", ";
                    }

                    serializedResult += "{ \"from\": " + $scope.renderModel.periods[i].from + ", " + 
                        "\"to\": " + $scope.renderModel.periods[i].to + " }";
                }

                serializedResult += "]";
                $scope.submit(serializedResult);
            };
        });
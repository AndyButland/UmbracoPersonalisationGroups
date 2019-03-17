angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.TimeOfDayPersonalisationGroupCriteriaController",
        function ($scope) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            function isNumber(n) {
                return !isNaN(parseFloat(n)) && isFinite(n);
            };
            
            function isValidPeriod(from, to) {
                return isNumber(from) && isNumber(to) &&
                    parseInt(from) >= 0 && parseInt(from) < 2359 &&
                    parseInt(to) >= 0 && parseInt(to) < 2359 &&
                    parseInt(to) > parseInt(from);
            };

            $scope.renderModel = {};
            $scope.renderModel.periods = [];

            if (definition) {
                $scope.renderModel.periods = JSON.parse(definition);
            }

            $scope.newFrom = "";
            $scope.newTo = "";
            $scope.currentEditPeriod = null;
            $scope.hasError = false;

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

                    serializedResult += "{ \"from\": \"" + $scope.renderModel.periods[i].from + "\", " + 
                        "\"to\": \"" + $scope.renderModel.periods[i].to + "\" }";
                }

                serializedResult += "]";

                // For V7 we use $scope.submit(), for V8 $scope.model.submit()
                if ($scope.submit) {
                    $scope.submit(serializedResult);
                } else {
                    $scope.model.submit(serializedResult);
                }
            };

            // For V8 we need to make a call to fire any handler on the close of the dialog
            if ($scope.model && $scope.model.close) {
                $scope.close = function () {
                    $scope.model.close();
                }
            }
        });
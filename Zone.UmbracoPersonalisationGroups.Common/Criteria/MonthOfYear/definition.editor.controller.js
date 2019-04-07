angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.MonthOfYearPersonalisationGroupCriteriaController",
        function ($scope) {

            var monthsInYear = 12;

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            $scope.renderModel = {};
            $scope.renderModel.months = [];
            for (var i = 0; i < monthsInYear; i++) {
                $scope.renderModel.months.push(false);
            }

            if (definition) {
                var selectedMonths = JSON.parse(definition);
                for (var i = 0; i < selectedMonths.length; i++) {
                    $scope.renderModel.months[selectedMonths[i] - 1] = true;
                }
            }

            $scope.saveAndClose = function () {
                var serializedResult = "[";
                for (var i = 0; i < monthsInYear; i++) {
                    if ($scope.renderModel.months[i]) {
                        if (serializedResult.length > 1) {
                            serializedResult += ",";
                        }

                        serializedResult += (i + 1);
                    }
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
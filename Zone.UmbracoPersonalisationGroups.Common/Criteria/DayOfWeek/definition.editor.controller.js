angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.DayOfWeekPersonalisationGroupCriteriaController",
        function ($scope) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            $scope.renderModel = {};
            $scope.renderModel.days = [];
            for (var i = 0; i < 7; i++) {
                $scope.renderModel.days.push(false);
            }

            if (definition) {
                var selectedDays = JSON.parse(definition);
                for (var i = 0; i < selectedDays.length; i++) {
                    $scope.renderModel.days[selectedDays[i] - 1] = true;
                }
            }

            $scope.saveAndClose = function () {
                var serializedResult = "[";
                for (var i = 0; i < 7; i++) {
                    if ($scope.renderModel.days[i]) {
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
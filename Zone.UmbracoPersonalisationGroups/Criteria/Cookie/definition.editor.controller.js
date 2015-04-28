angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.DayOfWeekPersonalisationGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = {};
            $scope.renderModel.days = [];
            for (var i = 0; i < 7; i++) {
                $scope.renderModel.days.push(false);
            }

            if ($scope.dialogOptions.definition) {
                var selectedDays = JSON.parse($scope.dialogOptions.definition);
                for (var i = 0; i < selectedDays.length; i++) {
                    $scope.renderModel.days[i + 1] = true;
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
                $scope.submit(serializedResult);
            };

        });
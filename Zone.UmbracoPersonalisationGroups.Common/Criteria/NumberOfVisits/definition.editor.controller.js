angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.NumberOfVisitsPersonalisationGroupCriteriaController",
        function ($scope) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            $scope.renderModel = { match: "Exists" };

            if (definition) {
                var numberOfVisitsSettings = JSON.parse(definition);
                $scope.renderModel = numberOfVisitsSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"match\": \"" + $scope.renderModel.match + "\", " +
                    "\"number\": " + $scope.renderModel.number + "}";

                // For V7 we use $scope.submit(), for V8 $scope.model.submit()
                if ($scope.submit) {
                    $scope.submit(serializedResult);
                } else {
                    $scope.model.submit(serializedResult);
                }
            };

            $scope.close = function () {
                if ($scope.model.close) {
                    $scope.model.close();
                }
            };

        });
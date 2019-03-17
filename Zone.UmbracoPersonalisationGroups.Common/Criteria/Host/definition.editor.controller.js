angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.HostPersonalisationGroupCriteriaController",
        function ($scope) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            $scope.renderModel = { match: "MatchesValue" };

            if (definition) {
                var hostSettings = JSON.parse(definition);
                $scope.renderModel = hostSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"value\": \"" + $scope.renderModel.value + "\", " +
                    "\"match\": \"" + $scope.renderModel.match + "\" }";

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
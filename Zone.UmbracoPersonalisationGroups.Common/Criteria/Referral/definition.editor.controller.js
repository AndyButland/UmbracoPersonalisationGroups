angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.ReferralPersonalisationGroupCriteriaController",
        function ($scope) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            $scope.renderModel = { match: "MatchesValue" };

            if (definition) {
                var referrerSettings = JSON.parse(definition);
                $scope.renderModel = referrerSettings;
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

            $scope.close = function () {
                if ($scope.model.close) {
                    $scope.model.close();
                }
            };

        });
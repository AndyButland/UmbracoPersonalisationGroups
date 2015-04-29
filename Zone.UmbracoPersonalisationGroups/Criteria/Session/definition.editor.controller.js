angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.SessionPersonalisationGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = { match: "Exists" };

            if ($scope.dialogOptions.definition) {
                var sessionSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = sessionSettings;
            }

            $scope.valueRequired = function() {
                return $scope.renderModel.match === "MatchesValue" || $scope.renderModel.match === "ContainsValue";
            };

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"key\": \"" + $scope.renderModel.key + "\", " +
                    "\"match\": \"" + $scope.renderModel.match + "\", " + 
                    "\"value\": \"" + ($scope.valueRequired() ? $scope.renderModel.value : "") + "\" }";

                $scope.submit(serializedResult);
            };

        });
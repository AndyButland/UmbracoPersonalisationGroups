angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.CookiePersonalisationGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = {};

            if ($scope.dialogOptions.definition) {
                var cookieSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = cookieSettings;
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
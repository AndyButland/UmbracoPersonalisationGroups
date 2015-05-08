angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.CookiePersonalisationGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = { match: "Exists" };

            if ($scope.dialogOptions.definition) {
                var cookieSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = cookieSettings;
            }

            $scope.valueRequired = function() {
                return !($scope.renderModel.match === "Exists" || $scope.renderModel.match === "DoesNotExist");
            };

            $scope.saveAndClose = function () {
                if ($scope.renderModel.key) {
                    var serializedResult = "{ \"key\": \"" + $scope.renderModel.key + "\", " +
                        "\"match\": \"" + $scope.renderModel.match + "\", " +
                        "\"value\": \"" + ($scope.valueRequired() ? $scope.renderModel.value : "") + "\" }";
                    $scope.submit(serializedResult);
                }
            };

        });
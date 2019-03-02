angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.HostPersonalisationGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = { match: "MatchesValue" };

            if ($scope.dialogOptions.definition) {
                var hostSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = hostSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"value\": \"" + $scope.renderModel.value + "\", " +
                    "\"match\": \"" + $scope.renderModel.match + "\" }";
                $scope.submit(serializedResult);
            };

        });
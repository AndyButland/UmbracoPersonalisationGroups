angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.NumberOfVisitsPersonalisationGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = { match: "Exists" };

            if ($scope.dialogOptions.definition) {
                var numberOfVisitsSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = numberOfVisitsSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"match\": \"" + $scope.renderModel.match + "\", " +
                    "\"number\": " + $scope.renderModel.number + "}";
                $scope.submit(serializedResult);
            };

        });
angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.ReferralPersonalisationGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = { match: "MatchesValue" };

            if ($scope.dialogOptions.definition) {
                var referrerSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = referrerSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"value\": \"" + $scope.renderModel.value + "\", " +
                    "\"match\": \"" + $scope.renderModel.match + "\" }";
                $scope.submit(serializedResult);
            };

        });
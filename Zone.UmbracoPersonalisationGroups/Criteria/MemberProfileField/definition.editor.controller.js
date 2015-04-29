angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.MemberProfileFieldPersonalisationGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = { match: "MatchesValue" };

            if ($scope.dialogOptions.definition) {
                var profileFieldSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = profileFieldSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"alias\": \"" + $scope.renderModel.alias + "\", " +
                    "\"match\": \"" + $scope.renderModel.match + "\", " +
                    "\"value\": \"" + $scope.renderModel.value + "\" }";
                $scope.submit(serializedResult);
            };

        });
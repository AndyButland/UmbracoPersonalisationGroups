angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.MemberGroupPersonalisationGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = { match: "IsInGroup" };

            if ($scope.dialogOptions.definition) {
                var memberGroupSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = memberGroupSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"groupName\": \"" + $scope.renderModel.groupName + "\", " +
                    "\"match\": \"" + $scope.renderModel.match + "\" }";
                $scope.submit(serializedResult);
            };

        });
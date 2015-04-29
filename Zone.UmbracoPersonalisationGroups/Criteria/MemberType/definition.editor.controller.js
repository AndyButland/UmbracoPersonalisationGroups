angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.MemberTypePersonalisationGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = {};

            if ($scope.dialogOptions.definition) {
                var memberTypeSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = memberTypeSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"typeName\": \"" + $scope.renderModel.typeName + "\", " +
                    "\"match\": \"" + $scope.renderModel.match + "\" }";
                $scope.submit(serializedResult);
            };

        });
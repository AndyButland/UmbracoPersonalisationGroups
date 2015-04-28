angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.AuthenticationStatusPersonalisationGroupCriteriaController",
        function ($scope) {

            $scope.renderModel = {};

            if ($scope.dialogOptions.definition) {
                var authenticationStatus = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = authenticationStatus;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"isAuthenticated\": " + $scope.renderModel.isAuthenticated + " }";
                $scope.submit(serializedResult);
            };

        });
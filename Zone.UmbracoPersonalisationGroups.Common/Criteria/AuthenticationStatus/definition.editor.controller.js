angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.AuthenticationStatusPersonalisationGroupCriteriaController",
        function ($scope) {

            console.log($scope);
            console.log($scope.dialogOptions);
            $scope.renderModel = {};

            if ($scope.dialogOptions.definition) {
                var authenticationStatus = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = authenticationStatus;
            }

            $scope.saveAndClose = function () {
                var value = $scope.renderModel.isAuthenticated ? true : false;
                var serializedResult = "{ \"isAuthenticated\": " + value + " }";
                $scope.submit(serializedResult);
            };

        });
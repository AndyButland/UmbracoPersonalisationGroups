angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.AuthenticationStatusPersonalisationGroupCriteriaController",
        function ($scope) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            $scope.renderModel = {};

            if (definition) {
                var authenticationStatus = JSON.parse(definition);
                $scope.renderModel = authenticationStatus;
            }

            $scope.saveAndClose = function () {
                var value = $scope.renderModel.isAuthenticated ? true : false;
                var serializedResult = "{ \"isAuthenticated\": " + value + " }";

                // For V7 we use $scope.submit(), for V8 $scope.model.submit()
                if ($scope.submit) {
                    $scope.submit(serializedResult);
                } else {
                    $scope.model.submit(serializedResult);
                }
            };

            // For V8 we need to make a call to fire any handler on the close of the dialog
            if ($scope.model && $scope.model.close) {
                $scope.close = function () {
                    $scope.model.close();
                }
            }

        });
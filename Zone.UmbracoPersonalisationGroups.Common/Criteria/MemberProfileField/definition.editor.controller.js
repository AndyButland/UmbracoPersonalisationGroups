angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.MemberProfileFieldPersonalisationGroupCriteriaController",
        function ($scope, $http) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            function initGroupList() {
                $scope.availableFields = [];
                $http.get("/App_Plugins/UmbracoPersonalisationGroups/Member/GetMemberProfileFields")
                    .then(function (result) {
                        $scope.availableFields = result.data;
                        if (result.data.length > 0 && !$scope.renderModel.alias) {
                            $scope.renderModel.alias = result.data[0];
                        }
                    });
            };

            $scope.renderModel = { match: "MatchesValue" };

            initGroupList();

            if (definition) {
                var profileFieldSettings = JSON.parse(definition);
                $scope.renderModel = profileFieldSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"alias\": \"" + $scope.renderModel.alias + "\", " +
                    "\"match\": \"" + $scope.renderModel.match + "\", " +
                    "\"value\": \"" + $scope.renderModel.value + "\" }";

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
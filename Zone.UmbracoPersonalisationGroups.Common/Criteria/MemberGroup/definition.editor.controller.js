angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.MemberGroupPersonalisationGroupCriteriaController",
        function ($scope, $http) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            function initGroupList() {
                $scope.availableGroups = [];
                $http.get("/App_Plugins/UmbracoPersonalisationGroups/Member/GetMemberGroups")
                    .then(function (result) {
                        $scope.availableGroups = result.data;
                        if (result.data.length > 0 && !$scope.renderModel.groupName) {
                            $scope.renderModel.groupName = result.data[0];
                        }
                    });
            };

            $scope.renderModel = { match: "IsInGroup" };

            initGroupList();

            if (definition) {
                var memberGroupSettings = JSON.parse(definition);
                $scope.renderModel = memberGroupSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"groupName\": \"" + $scope.renderModel.groupName + "\", " +
                    "\"match\": \"" + $scope.renderModel.match + "\" }";

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
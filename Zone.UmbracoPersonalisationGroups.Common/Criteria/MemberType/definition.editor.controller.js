angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.MemberTypePersonalisationGroupCriteriaController",
        function ($scope, $http) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            function initGroupList() {
                $scope.availableTypes = [];
                $http.get("/App_Plugins/UmbracoPersonalisationGroups/Member/GetMemberTypes")
                    .then(function (result) {
                        $scope.availableTypes = result.data;
                        if (result.data.length > 0 && !$scope.renderModel.typeName) {
                            $scope.renderModel.typeName = result.data[0];
                        }
                    });
            };

            $scope.renderModel = { match: "IsOfType" };

            initGroupList();

            if (definition) {
                var memberTypeSettings = JSON.parse(definition);
                $scope.renderModel = memberTypeSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"typeName\": \"" + $scope.renderModel.typeName + "\", " +
                    "\"match\": \"" + $scope.renderModel.match + "\" }";

                // For V7 we use $scope.submit(), for V8 $scope.model.submit()
                if ($scope.submit) {
                    $scope.submit(serializedResult);
                } else {
                    $scope.model.submit(serializedResult);
                }
            };

            $scope.close = function () {
                if ($scope.model.close) {
                    $scope.model.close();
                }
            };

        });
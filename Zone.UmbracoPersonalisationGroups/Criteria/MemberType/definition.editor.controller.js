angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.MemberTypePersonalisationGroupCriteriaController",
        function ($scope, $http) {

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
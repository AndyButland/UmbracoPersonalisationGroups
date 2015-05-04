angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.MemberGroupPersonalisationGroupCriteriaController",
        function ($scope, $http) {

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
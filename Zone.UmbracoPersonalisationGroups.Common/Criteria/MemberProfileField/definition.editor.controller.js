angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.MemberProfileFieldPersonalisationGroupCriteriaController",
        function ($scope, $http) {

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

            if ($scope.dialogOptions.definition) {
                var profileFieldSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = profileFieldSettings;
            }

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"alias\": \"" + $scope.renderModel.alias + "\", " +
                    "\"match\": \"" + $scope.renderModel.match + "\", " +
                    "\"value\": \"" + $scope.renderModel.value + "\" }";
                $scope.submit(serializedResult);
            };

        });
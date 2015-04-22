angular.module("umbraco")
    .controller("UmbracoVisitorGroups.DayOfWeekVisitorGroupCriteriaController",
    function ($scope) {

        $scope.renderModel = {};
        $scope.renderModel.definition = $scope.dialogOptions.definition;

        $scope.saveAndClose = function (isValid) {
            $scope.submit($scope.renderModel.definition);
        };

    });

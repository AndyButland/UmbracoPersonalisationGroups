angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.CountryPersonalisationGroupCriteriaController",
        function ($scope) {

            function isValidCountryCode(code) {
                return code.length === 2;
            };

            $scope.renderModel = {};
            $scope.renderModel.countries = [];

            if ($scope.dialogOptions.definition) {
                $scope.renderModel.countries = JSON.parse($scope.dialogOptions.definition);
            }

            $scope.newCode = "";
            $scope.currentEditCountry = null;
            $scope.hasError = false;

            $scope.edit = function (index) {
                for (var i = 0; i < $scope.renderModel.countries.length; i++) {
                    $scope.renderModel.countries[i].edit = false;
                }

                $scope.renderModel.countries[index].edit = true;
            };

            $scope.saveEdit = function (index) {
                $scope.renderModel.countries[index].edit = false;
            };

            $scope.delete = function (index) {
                $scope.renderModel.countries.splice(index, 1);
            };

            $scope.add = function () {
                if (isValidCountryCode($scope.newCode)) {
                    var country = { code: $scope.newCode };
                    $scope.renderModel.countries.push(country);

                    $scope.newCode = "";
                } else {
                    $scope.hasError = true;
                }
            };

            $scope.saveAndClose = function () {
                var serializedResult = "[";

                for (var i = 0; i < $scope.renderModel.countries.length; i++) {
                    if (serializedResult.length > 1) {
                        serializedResult += ", ";
                    }

                    serializedResult += "{ \"code\": \"" + $scope.renderModel.countries[i].code + "\" }";
                }

                serializedResult += "]";
                $scope.submit(serializedResult);
            };
        });
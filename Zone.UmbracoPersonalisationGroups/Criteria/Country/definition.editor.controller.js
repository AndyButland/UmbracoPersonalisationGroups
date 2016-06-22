angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.CountryPersonalisationGroupCriteriaController",
        function ($scope) {

            function isValidCountryCode(code) {
                return code.length === 2;
            };

            function resetNewCode() {
                $scope.newCode = { value: "", hasError: false };
            }

            $scope.renderModel = { match: "IsLocatedIn" };
            $scope.renderModel.countries = [];

            if ($scope.dialogOptions.definition) {
                var countrySettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel.match = countrySettings.match;
                if (countrySettings.codes) {
                    for (var i = 0; i < countrySettings.codes.length; i++) {
                        $scope.renderModel.countries.push({ code: countrySettings.codes[i], edit: false });
                    }
                }
            }

            resetNewCode();

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
                if (isValidCountryCode($scope.newCode.value)) {
                    var country = { code: $scope.newCode.value, edit: false };
                    $scope.renderModel.countries.push(country);

                    resetNewCode();
                } else {
                    $scope.newCode.hasError = true;
                }
            };

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"match\": \"" + $scope.renderModel.match + "\", " + "\"codes\": [";

                for (var i = 0; i < $scope.renderModel.countries.length; i++) {
                    if (i > 0) {
                        serializedResult += ", ";
                    }

                    serializedResult += "\"" + $scope.renderModel.countries[i].code + "\"";
                }

                serializedResult += "] }";
                $scope.submit(serializedResult);
            };
        });
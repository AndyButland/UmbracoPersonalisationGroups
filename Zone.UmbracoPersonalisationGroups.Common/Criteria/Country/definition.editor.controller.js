angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.CountryPersonalisationGroupCriteriaController",
        function ($scope, geoLocationService) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            function initAvailableCountriesList() {
                geoLocationService.getCountryList()
                    .then(function (result) {
                        $scope.availableCountries = result.data;
                    });
            };

            initAvailableCountriesList();

            function resetNewCountry() {
                $scope.newCountry = { code: "", hasError: false };
            }

            $scope.renderModel = { match: "IsLocatedIn" };
            $scope.renderModel.countries = [];

            if (definition) {
                var countrySettings = JSON.parse(definition);
                $scope.renderModel.match = countrySettings.match;
                if (countrySettings.codes) {
                    for (var i = 0; i < countrySettings.codes.length; i++) {
                        $scope.renderModel.countries.push({ code: countrySettings.codes[i], edit: false });
                    }
                }
            }

            resetNewCountry();

            $scope.geoDetailsRequired = function () {
                return $scope.renderModel.match !== "CouldNotBeLocated";
            };

            $scope.getCountryName = function (code) {
                return geoLocationService.getCountryName(code, $scope.availableCountries);
            }

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

            function isValidCountryCode(code) {
                return code.length === 2;
            };

            $scope.add = function () {
                if (isValidCountryCode($scope.newCountry.code)) {
                    var country = { code: $scope.newCountry.code, edit: false };
                    $scope.renderModel.countries.push(country);
                    resetNewCountry();
                } else {
                    $scope.newCountry.hasError = true;
                }
            };

            $scope.saveAndClose = function () {

                var serializedResult = "{ \"match\": \"" + $scope.renderModel.match + "\" ";

                if ($scope.renderModel.match !== "CouldNotBeLocated") {

                    serializedResult += ", \"codes\": [";
                    for (var i = 0; i < $scope.renderModel.countries.length; i++) {
                        if (i > 0) {
                            serializedResult += ", ";
                        }

                        serializedResult += "\"" + $scope.renderModel.countries[i].code + "\"";
                    }
                    serializedResult += "], ";

                    serializedResult += "\"names\": [";
                    for (var i = 0; i < $scope.renderModel.countries.length; i++) {
                        if (i > 0) {
                            serializedResult += ", ";
                        }

                        serializedResult += "\"" + geoLocationService.getCountryName($scope.renderModel.countries[i].code, $scope.availableCountries) + "\"";
                    }

                    serializedResult += "]";
                }
                
                serializedResult += " }";

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
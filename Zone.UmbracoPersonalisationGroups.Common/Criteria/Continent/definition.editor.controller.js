angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.ContinentPersonalisationGroupCriteriaController",
        function ($scope, geoLocationService) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            function initAvailableContinentsList() {
                geoLocationService.getContinentList()
                    .then(function (result) {
                        $scope.availableCountries = result.data;
                    });
            };

            initAvailableContinentsList();

            function resetNewContinent() {
                $scope.newContinent = { code: "", hasError: false };
            }

            $scope.renderModel = { match: "IsLocatedIn" };
            $scope.renderModel.continents = [];

            if (definition) {
                var continentSettings = JSON.parse(definition);
                $scope.renderModel.match = continentSettings.match;
                if (continentSettings.codes) {
                    for (var i = 0; i < continentSettings.codes.length; i++) {
                        $scope.renderModel.continents.push({ code: continentSettings.codes[i], edit: false });
                    }
                }
            }

            resetNewContinent();

            $scope.geoDetailsRequired = function () {
                return $scope.renderModel.match !== "CouldNotBeLocated";
            };

            $scope.getContinentName = function (code) {
                return geoLocationService.getContinentName(code, $scope.availableCountries);
            }

            $scope.edit = function (index) {
                for (var i = 0; i < $scope.renderModel.continents.length; i++) {
                    $scope.renderModel.continents[i].edit = false;
                }

                $scope.renderModel.continents[index].edit = true;
            };

            $scope.saveEdit = function (index) {
                $scope.renderModel.continents[index].edit = false;
            };

            $scope.delete = function (index) {
                $scope.renderModel.continents.splice(index, 1);
            };

            function isValidContinentCode(code) {
                return code.length === 2;
            };

            $scope.add = function () {
                if (isValidContinentCode($scope.newContinent.code)) {
                    var country = { code: $scope.newContinent.code, edit: false };
                    $scope.renderModel.continents.push(country);
                    resetNewContinent();
                } else {
                    $scope.newContinent.hasError = true;
                }
            };

            $scope.saveAndClose = function () {

                var serializedResult = "{ \"match\": \"" + $scope.renderModel.match + "\" ";

                if ($scope.renderModel.match !== "CouldNotBeLocated") {

                    serializedResult += ", \"codes\": [";
                    for (var i = 0; i < $scope.renderModel.continents.length; i++) {
                        if (i > 0) {
                            serializedResult += ", ";
                        }

                        serializedResult += "\"" + $scope.renderModel.continents[i].code + "\"";
                    }
                    serializedResult += "], ";

                    serializedResult += "\"names\": [";
                    for (var i = 0; i < $scope.renderModel.continents.length; i++) {
                        if (i > 0) {
                            serializedResult += ", ";
                        }

                        serializedResult += "\"" + geoLocationService.getContinentName($scope.renderModel.continents[i].code, $scope.availableCountries) + "\"";
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

            // For V8 we need to make a call to fire any handler on the close of the dialog
            if ($scope.model && $scope.model.close) {
                $scope.close = function () {
                    $scope.model.close();
                }
            }
        });
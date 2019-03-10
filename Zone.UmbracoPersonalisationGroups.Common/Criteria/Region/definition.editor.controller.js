angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.RegionPersonalisationGroupCriteriaController",
        function ($scope, geoLocationService) {

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

            var defaultCountryCode = "GB";

            function initAvailableRegionsList(countryCode) {
                geoLocationService.getRegionList(countryCode)
                    .then(function (result) {
                        $scope.availableRegions = result.data;
                    });
            }

            function initCountryList() {
                geoLocationService.getCountryList(true)
                    .then(function (result) {
                        $scope.availableCountries = result.data;
                    });
            };

            initCountryList();

            function resetNewRegion() {
                $scope.newRegion = { name: "", hasError: false };
            }

            function clearRegions() {
                $scope.renderModel.regions = [];
            }

            $scope.renderModel = { match: "IsLocatedIn", countryCode: defaultCountryCode };
            $scope.renderModel.regions = [];

            if (definition) {
                var regionSettings = JSON.parse(definition);
                $scope.renderModel.match = regionSettings.match;
                $scope.renderModel.countryCode = regionSettings.countryCode;
                if (regionSettings.names) {
                    for (var i = 0; i < regionSettings.names.length; i++) {
                        $scope.renderModel.regions.push({ name: regionSettings.names[i], edit: false });
                    }
                }

                initAvailableRegionsList(regionSettings.countryCode);
            } else {
                initAvailableRegionsList(defaultCountryCode);
            }

            resetNewRegion();

            $scope.geoDetailsRequired = function () {
                return $scope.renderModel.match !== "CouldNotBeLocated";
            };

            $scope.changedCountryCode = function (countryCode) {
                clearRegions();
                initAvailableRegionsList(countryCode);
            }

            $scope.edit = function (index) {
                for (var i = 0; i < $scope.renderModel.regions.length; i++) {
                    $scope.renderModel.regions[i].edit = false;
                }

                $scope.renderModel.regions[index].edit = true;
            };

            $scope.saveEdit = function (index) {
                $scope.renderModel.regions[index].edit = false;
            };

            $scope.delete = function (index) {
                $scope.renderModel.regions.splice(index, 1);
            };

            $scope.add = function () {
                var country = { name: $scope.newRegion.name, edit: false };
                $scope.renderModel.regions.push(country);
                resetNewRegion();
            };

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"match\": \"" + $scope.renderModel.match + "\"";

                if ($scope.renderModel.match !== "CouldNotBeLocated") {
                    serializedResult += ", ";
                    serializedResult += "\"countryCode\": \"" + $scope.renderModel.countryCode + "\", ";
                    serializedResult += "\"countryName\": \"" + geoLocationService.getCountryName($scope.renderModel.countryCode, $scope.availableCountries) + "\", ";

                    serializedResult += "\"names\": [";
                    for (var i = 0; i < $scope.renderModel.regions.length; i++) {
                        if (i > 0) {
                            serializedResult += ", ";
                        }

                        serializedResult += "\"" + $scope.renderModel.regions[i].name + "\"";
                    }

                    serializedResult += "] ";
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
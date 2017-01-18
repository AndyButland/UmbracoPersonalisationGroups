angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.RegionPersonalisationGroupCriteriaController",
        function ($scope, geoLocationService) {

            var defaultCountryCode = "GB";

            function initAvailableRegionsList(countryCode) {
                geoLocationService.getRegionList(countryCode)
                    .success(function (data) {
                        $scope.availableRegions = data;
                    });
            }

            function initCountryList() {
                geoLocationService.getCountryList(true)
                    .success(function (data) {
                        $scope.availableCountries = data;
                    });
            };

            initCountryList();

            function resetNewRegion() {
                $scope.newRegion = { code: "", hasError: false };
            }

            function clearRegions() {
                $scope.renderModel.regions = [];
            }

            $scope.renderModel = { match: "IsLocatedIn", countryCode: defaultCountryCode };
            $scope.renderModel.regions = [];

            if ($scope.dialogOptions.definition) {
                var regionSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel.match = regionSettings.match;
                $scope.renderModel.countryCode = regionSettings.countryCode;
                if (regionSettings.codes) {
                    for (var i = 0; i < regionSettings.codes.length; i++) {
                        $scope.renderModel.regions.push({ code: regionSettings.codes[i], edit: false });
                    }
                }

                initAvailableRegionsList(regionSettings.countryCode);
            } else {
                initAvailableRegionsList(defaultCountryCode);
            }

            resetNewRegion();

            $scope.changedCountryCode = function (countryCode) {
                clearRegions();
                initAvailableRegionsList(countryCode);
            }

            $scope.getRegionName = function (code) {
                return geoLocationService.getRegionName(code, $scope.availableRegions);
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

            function isValidRegionCode(code) {
                return code.length === 2;
            };

            $scope.add = function () {
                if (isValidRegionCode($scope.newRegion.code)) {
                    var country = { code: $scope.newRegion.code, edit: false };
                    $scope.renderModel.regions.push(country);
                    resetNewRegion();
                } else {
                    $scope.newRegion.hasError = true;
                }
            };

            $scope.saveAndClose = function () {
                var serializedResult = "{ \"match\": \"" + $scope.renderModel.match + "\", ";
                serializedResult += "\"countryCode\": \"" + $scope.renderModel.countryCode + "\", ";
                serializedResult += "\"countryName\": \"" + geoLocationService.getCountryName($scope.renderModel.countryCode, $scope.availableCountries) + "\", ";

                serializedResult += "\"codes\": [";
                for (var i = 0; i < $scope.renderModel.regions.length; i++) {
                    if (i > 0) {
                        serializedResult += ", ";
                    }

                    serializedResult += "\"" + $scope.renderModel.regions[i].code + "\"";
                }
                serializedResult += "], ";

                serializedResult += "\"names\": [";
                for (var i = 0; i < $scope.renderModel.regions.length; i++) {
                    if (i > 0) {
                        serializedResult += ", ";
                    }

                    serializedResult += "\"" + geoLocationService.getRegionName($scope.renderModel.regions[i].code, $scope.availableRegions) + "\"";
                }
                serializedResult += "]";

                serializedResult += " }";

                $scope.submit(serializedResult);
            };
        });
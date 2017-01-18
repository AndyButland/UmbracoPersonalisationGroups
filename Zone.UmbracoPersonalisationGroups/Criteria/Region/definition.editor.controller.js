angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.RegionPersonalisationGroupCriteriaController",
        function ($scope, geoLocationService) {

            function initCountryList() {
                geoLocationService.getCountryList(true)
                    .then(function (result) {
                        $scope.availableCountries = result;
                    });
            };

            initCountryList();

            function resetNewRegion() {
                $scope.newRegion = { code: "", hasError: false };
            }

            $scope.renderModel = { match: "IsLocatedIn", countryCode: "GB" };
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
            }

            resetNewRegion();

            $scope.changedCountryCode = function (countryCode) {
                geoLocationService.getRegionList(countryCode)
                    .then(function (result) {
                        $scope.availableRegions = result;
                    });
            }

            $scope.getRegionName = function (code) {
                return geoLocationService.getRegionName(code);
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
                var serializedResult = "{ \"match\": \"" + $scope.renderModel.match + "\", " + "\"countryCode\": \"" + $scope.renderModel.countryCode + "\", " + "\"codes\": [";

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
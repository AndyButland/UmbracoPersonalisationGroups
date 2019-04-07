angular.module("umbraco.services")
    .factory("geoLocationService", function ($http) {

        var service = {
            getContinentList: function () {
                var url = "/App_Plugins/UmbracoPersonalisationGroups/GeoLocation/GetContinents";
                return $http.get(url, { cache: true });
            },

            getCountryList: function (withRegionsOnly) {
                var url = "/App_Plugins/UmbracoPersonalisationGroups/GeoLocation/GetCountries?withRegionsOnly=" + withRegionsOnly;
                return $http.get(url, { cache: true });
            },

            getRegionList: function (countryCode) {
                var url = "/App_Plugins/UmbracoPersonalisationGroups/GeoLocation/GetRegions?countryCode=" + countryCode;
                return $http.get(url, { cache: true });
            },

            getContinentName: function (code, continents) {
                if (continents) {
                    for (var j = 0; j < continents.length; j++) {
                        if (continents[j].code === code) {
                            return continents[j].name;
                        }
                    }
                }

                return "";
            },

            getCountryName: function (code, countries) {
                if (countries) {
                    for (var j = 0; j < countries.length; j++) {
                        if (countries[j].code === code) {
                            return countries[j].name;
                        }
                    }
                }

                return "";
            },

            getRegionName: function (code, regions) {
                if (regions) {
                    for (var j = 0; j < regions.length; j++) {
                        if (regions[j].code === code) {
                            return regions[j].name;
                        }
                    }
                }

                return "";
            }
        };

        return service;
    });
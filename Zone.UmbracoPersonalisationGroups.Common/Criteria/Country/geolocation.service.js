angular.module("umbraco.services")
	.factory("geoLocationService", function ($http) {

	    var service = {
	        getCountryList: function (withRegionsOnly) {
	            var url = "/App_Plugins/UmbracoPersonalisationGroups/GeoLocation/GetCountries?withRegionsOnly=" + withRegionsOnly;
	            return $http.get(url, { cache: true });
	        },

            getRegionList: function (countryCode) {
                var url = "/App_Plugins/UmbracoPersonalisationGroups/GeoLocation/GetRegions?countryCode=" + countryCode;
                return $http.get(url, { cache: true });
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
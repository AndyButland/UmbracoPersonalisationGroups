angular.module("umbraco.services")
	.factory("geoLocationService", function ($http) {

	    var currentCountryCode = "GB";
        var regions = [];

        //function initCountryList(withRegionsOnly) {
        //    return $http.get("/App_Plugins/UmbracoPersonalisationGroups/GeoLocation/GetCountries?withRegionsOnly=" + withRegionsOnly)
        //        .success(function (result) {

        //            // Cache result for further requests
        //            if (withRegionsOnly) {
        //                countriesWithRegions = result.data;
        //            } else {
        //                allCountries = result.data;
        //            }
        //        });
        //};

	    //function initRegionList() {
	    //    $http.get("/App_Plugins/UmbracoPersonalisationGroups/GeoLocation/GetRegions?countryCode=" + currentCountryCode)
        //        .then(function (result) {
        //            regions = result.data;
        //            return result.data;
        //        });
	    //};

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
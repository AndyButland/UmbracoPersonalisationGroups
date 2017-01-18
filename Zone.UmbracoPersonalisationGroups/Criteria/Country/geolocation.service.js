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
	            return $http.get("/App_Plugins/UmbracoPersonalisationGroups/GeoLocation/GetCountries?withRegionsOnly=" + withRegionsOnly,
                    { cache: true });
	        },

            //getRegionList: function (countryCode) {
            //    if (countryCode !== currentCountryCode || regions.length === 0) {
            //        currentCountryCode = countryCode;
            //        return initRegionList();
            //    } else {
            //        return Promise.resolve(regions);
            //    }
            //},

            getCountryName: function (code, countries) {
                for (var j = 0; j < countries.length; j++) {
                    if (countries[j].code === code) {
                        return countries[j].name;
                    }
                }

                return "";
            }
	    };

	    return service;
    });
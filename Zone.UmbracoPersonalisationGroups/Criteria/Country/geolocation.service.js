angular.module("umbraco.services")
	.factory("geoLocationService", function ($http) {

	    var availableCountries = [];

	    function initCountryList() {
	        $http.get("/App_Plugins/UmbracoPersonalisationGroups/GeoLocation/GetCountries")
                .then(function (result) {
                    availableCountries = result.data;
                });
	    };

        initCountryList();

        var service = {
            getCountryList: function() {
                return availableCountries;
            },

            getCountryName: function (code) {
                for (var j = 0; j < availableCountries.length; j++) {
                    if (availableCountries[j].code === code) {
                        return availableCountries[j].name;
                    }
                }

                return "";
	        }
	    };

	    return service;
    });
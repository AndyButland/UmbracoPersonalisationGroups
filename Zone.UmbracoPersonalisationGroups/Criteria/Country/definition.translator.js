angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.CountryTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var selectedCountryCodes = JSON.parse(definition);

	                for (var i = 0; i < selectedCountryCodes.length; i++) {
	                    if (translation.length > 0) {
	                        translation += ", ";
	                    }

	                    translation += selectedCountryCodes[i].code.toUpperCase();
	                }
	            }

	            return translation;
	        }
	    };

	    return service;
    });
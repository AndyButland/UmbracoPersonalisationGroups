angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.CountryTranslatorService", function (geoLocationService) {

        var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var selectedCountryDetails = JSON.parse(definition);
	                translation = "Visitor is ";
	                switch (selectedCountryDetails.match) {
	                    case "IsLocatedIn":
	                        translation += "located";
	                        break;
	                    case "IsNotLocatedIn":
	                        translation += "not located";
	                        break;
	                }

	                translation += " in: ";

	                for (var i = 0; i < selectedCountryDetails.codes.length; i++) {
	                    if (i > 0 && i === selectedCountryDetails.codes.length - 1) {
	                        translation += " or ";
	                    } else if (i > 0) {
	                        translation += ", ";
	                    }

	                    translation += geoLocationService.getCountryName(selectedCountryDetails.codes[i].toUpperCase());
	                }
	            }

	            return translation;
	        }
	    };

	    return service;
    });
angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.CountryTranslatorService", function () {

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
	                    if (i > 0) {
	                        translation += ", ";
	                    }

	                    translation += selectedCountryDetails.codes[i].toUpperCase();
	                }
	            }

	            return translation;
	        }
	    };

	    return service;
    });
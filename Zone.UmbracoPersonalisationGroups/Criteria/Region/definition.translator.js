angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.RegionTranslatorService", function () {

        var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var selectedRegionDetails = JSON.parse(definition);
	                translation = "Visitor is ";
	                switch (selectedRegionDetails.match) {
	                    case "IsLocatedIn":
	                        translation += "located";
	                        break;
	                    case "IsNotLocatedIn":
	                        translation += "not located";
	                        break;
	                }

	                translation += " in: ";

	                for (var i = 0; i < selectedRegionDetails.codes.length; i++) {
	                    if (i > 0 && i === selectedRegionDetails.codes.length - 1) {
	                        translation += " or ";
	                    } else if (i > 0) {
	                        translation += ", ";
	                    }

	                    translation += selectedRegionDetails.names[i];
	                }

	                translation += ", " + selectedRegionDetails.countryName;
	            }

	            return translation;
	        }
	    };

	    return service;
    });
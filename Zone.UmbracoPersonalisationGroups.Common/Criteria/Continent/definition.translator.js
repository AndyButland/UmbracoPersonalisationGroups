angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.ContinentTranslatorService", function () {

        var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
                    var selectedContinentDetails = JSON.parse(definition);
                    if (selectedContinentDetails.match === "CouldNotBeLocated") {
	                    translation = "Visitor cannot be located";
	                } else {
	                    translation = "Visitor is ";
	                    switch (selectedContinentDetails.match) {
	                    case "IsLocatedIn":
	                        translation += "located";
	                        break;
	                    case "IsNotLocatedIn":
	                        translation += "not located";
	                        break;
	                    }

	                    translation += " in: ";

	                    for (var i = 0; i < selectedContinentDetails.codes.length; i++) {
	                        if (i > 0 && i === selectedContinentDetails.codes.length - 1) {
	                            translation += " or ";
	                        } else if (i > 0) {
	                            translation += ", ";
	                        }

	                        // Versions 0.2.5 and later store the continent name, before that we just had the code.
	                        // So display the name if we have it, otherwise just the code.
	                        translation += selectedContinentDetails.names
	                            ? selectedContinentDetails.names[i]
	                            : selectedContinentDetails.codes[i].toUpperCase();
	                    }
	                }
	            }

	            return translation;
	        }
	    };

	    return service;
    });
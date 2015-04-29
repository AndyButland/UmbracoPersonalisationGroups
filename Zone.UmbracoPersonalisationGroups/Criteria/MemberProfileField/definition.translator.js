angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.MemberProfileFieldTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var profileFieldDetails = JSON.parse(definition);
	                translation = "Field with alias '" + profileFieldDetails.alias + "' " + 
                        (profileFieldDetails.match === "MatchesValue" ? "matches " : "does not match ") +
	                    "'" + profileFieldDetails.value + "'.";

	            }

	            return translation;
	        }
	    };

	    return service;
	});
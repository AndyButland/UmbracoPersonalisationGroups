angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.MemberGroupTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var memberTypeDetails = JSON.parse(definition);
	                translation = (memberTypeDetails.match === "IsInGroup" ? "Is in group " : "Is not in group ") +
	                    "'" + memberTypeDetails.groupName + "'.";

	            }

	            return translation;
	        }
	    };

	    return service;
	});
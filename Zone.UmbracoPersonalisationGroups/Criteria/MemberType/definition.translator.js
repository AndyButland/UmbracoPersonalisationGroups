angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.MemberTypeTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var memberTypeDetails = JSON.parse(definition);
	                translation = (memberTypeDetails.match === "IsOfType" ? "Is of type " : "Is not of type ") +
	                    "'" + memberTypeDetails.typeName + "'.";

	            }

	            return translation;
	        }
	    };

	    return service;
	});
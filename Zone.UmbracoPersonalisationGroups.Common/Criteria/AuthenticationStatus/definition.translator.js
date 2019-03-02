angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.AuthenticationStatusTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var authenticationStatusDetails = JSON.parse(definition);
	                translation = authenticationStatusDetails.isAuthenticated ? "Is logged in." : "Is not logged in.";
	            }

	            return translation;
	        }
	    };

	    return service;
	});
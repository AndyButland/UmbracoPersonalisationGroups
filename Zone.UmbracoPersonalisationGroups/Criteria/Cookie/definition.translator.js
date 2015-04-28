angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.CookieTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var selectedCookieDetails = JSON.parse(definition);
	                translation = "Cookie with key '" + selectedCookieDetails.key + "' ";
	                switch (selectedCookieDetails.match) {
	                case "Exists":
	                    translation += "is present.";
	                    break;
	                case "DoesNotExist":
	                    translation += "is not present.";
	                    break;
	                case "MatchesValue":
	                    translation += "matches value '" + selectedCookieDetails.value + "'.";
	                    break;
	                case "ContainsValue":
	                    translation += "contains value '" + selectedCookieDetails.value + "'.";
	                    break;
	                }
	            }

	            return translation;
	        }
	    };

	    return service;
	});
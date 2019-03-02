angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.ReferralTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var selectedReferrerDetails = JSON.parse(definition);
	                translation = "Referrer value ";
	                switch (selectedReferrerDetails.match) {
	                    case "MatchesValue":
	                        translation += "matches '" + selectedReferrerDetails.value + "'.";
	                        break;
	                    case "DoesNotMatchValue":
	                        translation += "does not match '" + selectedReferrerDetails.value + "'.";
	                        break;
	                    case "ContainsValue":
	                        translation += "contains '" + selectedReferrerDetails.value + "'.";
	                        break;
	                    case "DoesNotContainValue":
	                        translation += "does not contain '" + selectedReferrerDetails.value + "'.";
	                        break;
	                }
	            }

	            return translation;
	        }
	    };

	    return service;
	});
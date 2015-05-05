angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.SessionTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var selectedSessionDetails = JSON.parse(definition);
	                translation = "Session key '" + selectedSessionDetails.key + "' ";
	                switch (selectedSessionDetails.match) {
	                    case "Exists":
	                        translation += "is present.";
	                        break;
	                    case "DoesNotExist":
	                        translation += "is absent.";
	                        break;
	                    case "MatchesValue":
	                        translation += "matches value '" + selectedSessionDetails.value + "'.";
	                        break;
	                    case "ContainsValue":
	                        translation += "contains value '" + selectedSessionDetails.value + "'.";
	                        break;
	                    case "GreaterThanValue":
	                        translation += "is greater than value '" + selectedSessionDetails.value + "'.";
	                        break;
	                    case "GreaterThanOrEqualToValue":
	                        translation += "is greater than or equal to value '" + selectedSessionDetails.value + "'.";
	                        break;
	                    case "LessThanValue":
	                        translation += "is less than value '" + selectedSessionDetails.value + "'.";
	                        break;
	                    case "LessThanOrEqualToValue":
	                        translation += "is less than or equal to value '" + selectedSessionDetails.value + "'.";
	                        break;
	                }
	            }

	            return translation;
	        }
	    };

	    return service;
	});
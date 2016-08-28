angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.QuerystringTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var selectedQuerystringDetails = JSON.parse(definition);
	                translation = "Key of '" + selectedQuerystringDetails.key + "' ";
	                switch (selectedQuerystringDetails.match) {
	                    case "MatchesValue":
	                        translation += "matches '" + selectedQuerystringDetails.value + "'.";
	                        break;
	                    case "DoesNotMatchValue":
	                        translation += "does not match '" + selectedQuerystringDetails.value + "'.";
	                        break;
	                    case "ContainsValue":
	                        translation += "contains '" + selectedQuerystringDetails.value + "'.";
	                        break;
	                    case "DoesNotContainValue":
	                        translation += "does not contain '" + selectedQuerystringDetails.value + "'.";
	                        break;
	                    case "MatchesRegex":
	                        translation += "matches regular expression '" + selectedQuerystringDetails.value + "'.";
	                        break;
	                    case "DoesNotMatchRegex":
	                        translation += "does not match regular expression '" + selectedQuerystringDetails.value + "'.";
	                        break;
	                }
	            }

	            return translation;
	        }
	    };

	    return service;
	});
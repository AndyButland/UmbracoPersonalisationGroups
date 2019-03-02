angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.PagesViewedTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var selectedPagesViewedDetails = JSON.parse(definition);
	                translation = "Visitor has ";
	                switch (selectedPagesViewedDetails.match) {
	                    case "ViewedAny":
	                        translation += "viewed any of";
	                        break;
	                    case "ViewedAll":
	                        translation += "viewed all";
	                        break;
	                    case "NotViewedAny":
	                        translation += "not viewed any of";
	                        break;
	                    case "NotViewedAll":
	                        translation += "not viewed all";
	                        break;
	                }

	                translation += " the " + selectedPagesViewedDetails.nodeIds.length +
                        " selected page" + (selectedPagesViewedDetails.nodeIds.length === 1 ? "" : "s");
	            }

	            return translation;
	        }
	    };

	    return service;
	});
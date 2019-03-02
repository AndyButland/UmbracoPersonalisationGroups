angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.NumberOfVisitsTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var selectedNumberOfVisitsDetails = JSON.parse(definition);
	                translation = "Visitor has visited the site ";
	                switch (selectedNumberOfVisitsDetails.match) {
	                    case "MoreThan":
	                        translation += "more than";
	                        break;
	                    case "LessThan":
	                        translation += "less than";
	                        break;
	                    case "Exactly":
	                        translation += "exactly";
	                        break;
	                }

	                translation += " " + selectedNumberOfVisitsDetails.number +
                        " time" + (selectedNumberOfVisitsDetails.number === 1 ? "" : "s");
	            }

	            return translation;
	        }
	    };

	    return service;
	});
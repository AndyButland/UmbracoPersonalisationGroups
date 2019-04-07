angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.MonthOfYearTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
	                var selectedMonths = JSON.parse(definition);

	                for (var i = 0; i < selectedMonths.length; i++) {
	                    if (translation.length > 0) {
	                        translation += ", ";
	                    }

	                    translation += months[selectedMonths[i] - 1];
	                }
	            }

	            return translation;
	        }
	    };

	    return service;
	});
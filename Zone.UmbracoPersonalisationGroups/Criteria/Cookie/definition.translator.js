angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.DayOfWeekTranslatorService", function () {

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
	            var selectedDays = JSON.parse(definition);

	            for (var i = 0; i < selectedDays.length; i++) {
	                if (translation.length > 0) {
	                    translation += ", ";
	                }

	                translation += days[selectedDays[i] - 1];
	            }

	            return translation;
	        }
	    };

	    return service;
	});
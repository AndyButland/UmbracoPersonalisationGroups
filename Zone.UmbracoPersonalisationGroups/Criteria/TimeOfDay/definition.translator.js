angular.module("umbraco.services")
	.factory("UmbracoPersonalisationGroups.TimeOfDayTranslatorService", function () {

	    function formatTime(time) {
	        var timeAsString = "" + time;
	        return timeAsString.substr(0, timeAsString.length - 2) + ":" + timeAsString.substr(timeAsString.length - 2);
	    }

	    var service = {
	        translate: function (definition) {
	            var translation = "";
	            if (definition) {
	                var selectedTimes = JSON.parse(definition);

	                for (var i = 0; i < selectedTimes.length; i++) {
	                    if (translation.length > 0) {
	                        translation += ", ";
	                    }

	                    translation += formatTime(selectedTimes[i].from) + " - " + formatTime(selectedTimes[i].to);
	                }
	            }

	            return translation;
	        }
	    };

	    return service;
    });
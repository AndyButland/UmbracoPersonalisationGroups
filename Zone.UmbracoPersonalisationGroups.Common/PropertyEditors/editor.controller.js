angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.PersonalisationGroupDefinitionController",
        function ($scope, $http, $injector, dialogService) {

            var translators = [];
            var editingNew = false;

            function convertToPascalCase(s) {
                return s.charAt(0).toUpperCase() + s.substr(1);
            }

            function loadTranslators() {
                for (var i = 0; i < $scope.availableCriteria.length; i++) {
                    translators.push($injector.get("UmbracoPersonalisationGroups." + convertToPascalCase($scope.availableCriteria[i].alias) + "TranslatorService"));
                }
            }

            function initAvailableCriteriaList() {
                $scope.availableCriteria = [];
                $http.get("/App_Plugins/UmbracoPersonalisationGroups/Criteria")
                    .then(function (result) {
                        $scope.availableCriteria = result.data;
                        if (result.data.length > 0) {
                            $scope.selectedCriteria = result.data[0];
                        }

                        loadTranslators();
                });
            };

            function getCriteriaByAlias(alias) {
                for (var i = 0; i < $scope.availableCriteria.length; i++) {
                    if ($scope.availableCriteria[i].alias === alias) {
                        return $scope.availableCriteria[i];
                    }
                }

                return null;
            };

            function getCriteriaIndexByAlias(alias) {
                var index = 0;
                for (var i = 0; i < $scope.availableCriteria.length; i++) {
                    if ($scope.availableCriteria[i].alias === alias) {
                        return index;
                    }

                    index++;
                }

                return -1;
            };

            if (!$scope.model.value) {
                $scope.model.value = { match: "All", duration: "Page", score: 50, details: [] };
            }

            if (!$scope.model.value.duration) {
                $scope.model.value.duration = "Page";
            }

            if (!$scope.model.value.score) {
                $scope.model.value.score = 50;
            }

            $scope.selectedCriteria = null;

            initAvailableCriteriaList();

            $scope.addCriteria = function () {
                var detail = { alias: $scope.selectedCriteria.alias, definition: "" };
                $scope.model.value.details.push(detail);
                $scope.editDefinitionDetail(detail);
                editingNew = true;
            };

            $scope.editDefinitionDetail = function (definitionDetail) {
                editingNew = false;
                var templateUrl = "/App_Plugins/UmbracoPersonalisationGroups/GetResourceForCriteria/" + definitionDetail.alias + "/definition.editor.html";
                dialogService.open(
                    {
                        template: templateUrl,
                        definition: definitionDetail.definition,
                        callback: function (data) {
                            definitionDetail.definition = data;
                        },
                        closeCallback: function() {
                            if (editingNew) {
                                // If we've cancelled a new one, we don't want an empty record
                                $scope.model.value.details.pop();
                            }
                        },
                    });
            };

            $scope.delete = function (index) {
                $scope.model.value.details.splice(index, 1);
            };

            $scope.getCriteriaName = function (alias) {
                var criteria = getCriteriaByAlias(alias);
                if (criteria) {
                    return criteria.name;
                }

                return "";
            };

            $scope.getDefinitionTranslation = function (definitionDetail) {
                var translator = translators[getCriteriaIndexByAlias(definitionDetail.alias)];
                if (translator) {
                    return translator.translate(definitionDetail.definition);
                }

                return "";
            };
        });

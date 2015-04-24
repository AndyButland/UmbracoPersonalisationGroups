angular.module("umbraco")
    .controller("UmbracoVisitorGroups.VisitorGroupDefinitionController",
        function ($scope, $http, dialogService, assetsService) {
            if (!$scope.model.value) {
                $scope.model.value = { match: "All", details: [] };
            }

            // Load the available criteria
            $scope.availableCriteria = [];
            $scope.selectedCriteria = null;
            $http.get("/App_Plugins/UmbracoVisitorGroups/AvailableCriteria")
                .then(function (result) {

                    // Assign to scope so can be selected for use
                    $scope.availableCriteria = result.data;
                    if (result.data.length > 0) {
                        $scope.selectedCriteria = result.data[0];
                    }

                    // Load associated controllers needed for definition builders (this is working in the sense
                    // that the controller is being loaded, but it's not found when the view loads from the dialogService)
                    /*
                    for (var i = 0; i < result.data.length; i++) {
                        if (result.data[i].hasDefinitionEditorView) {
                            var controllerPath = "/App_Plugins/UmbracoVisitorGroups/ResourceForCriteria/" + result.data[i].alias + "/definition.editor.controller.js";
                            assetsService.loadJs(controllerPath).then(function () {
                                console.log("Loaded controller.");
                            });
                        }
                    }
                    */
                });

            function getCriteriaByAlias(alias) {
                for (var i = 0; i < $scope.availableCriteria.length; i++) {
                    if ($scope.availableCriteria[i].alias === alias) {
                        return $scope.availableCriteria[i];
                    }
                }

                return null;
            };

            $scope.addCriteria = function () {
                var detail = { alias: $scope.selectedCriteria.alias, definition: "" };
                $scope.model.value.details.push(detail);
            };

            $scope.editDefinitionDetail = function (definitionDetail) {
                var templateUrl = "/App_Plugins/UmbracoVisitorGroups/ResourceForCriteria/" + definitionDetail.alias + "/definition.editor.html";
                dialogService.open(
                    {
                        template: templateUrl,
                        definition: definitionDetail.definition,
                        callback: function (data) {
                            definitionDetail.definition = data;
                        }
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

            $scope.criteriaHasDefinitionEditorView = function (alias) {
                var criteria = getCriteriaByAlias(alias);
                if (criteria) {
                    return criteria.hasDefinitionEditorView;
                }

                return false;
            };
        });

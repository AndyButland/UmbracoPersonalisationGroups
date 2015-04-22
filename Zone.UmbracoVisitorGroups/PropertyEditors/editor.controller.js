angular.module("umbraco")
    .controller("UmbracoVisitorGroups.VisitorGroupDefinitionController",
        function ($scope, $http, dialogService) {
            if (!$scope.model.value) {
                $scope.model.value = { match: "All", details: [] };
            }

            $scope.availableCriteria = [];
            $scope.selectedCriteria = null;
            $http.get("/App_Plugins/UmbracoVisitorGroups/AvailableCriteria")
                .then(function (result) {
                    $scope.availableCriteria = result.data;
                    if (result.data.length > 0) {
                        $scope.selectedCriteria = result.data[0];
                    }
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
                dialogService.open(
                    {
                        template: "/App_Plugins/UmbracoVisitorGroups/Resource/" + definitionDetail.alias.toLowerCase() + ".definition.editor.html",
                        definition: definitionDetail.definition,
                        callback: function (data) {
                            definitionDetail.definition = data.definition;
                        }
                    });
            };

            $scope.delete = function (index) {
                $scope.model.value.details.splice(index, 1);
            };

            $scope.getCriteriaName = function (alias) {
                var criteria = getCriteriaByAlias(alias);
                if (criteria) {
                    return $scope.availableCriteria[i].name;
                }
            };

            $scope.criteriaHasDefinitionEditorView = function (alias) {
                var criteria = getCriteriaByAlias(alias);
                if (criteria) {
                    return $scope.availableCriteria[i].hasDefinitionEditorView;
                }
            };
        });

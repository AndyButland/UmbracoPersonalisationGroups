angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.PagesViewedPersonalisationGroupCriteriaController",
        function ($scope, dialogService, entityResource, iconHelper) {

            function loadNodeDetails() {

                $scope.renderModel.nodes = [];
                entityResource.getByIds($scope.renderModel.nodeIds, "Document").then(function (data) {

                    // Load full node details from the ids that were stored, in the same order
                    _.each($scope.renderModel.nodeIds, function (id, i) {
                        var entity = _.find(data, function (d) {
                            return d.id == id;
                        });

                        if (entity) {
                            entity.icon = iconHelper.convertFromLegacyIcon(entity.icon);
                            $scope.renderModel.nodes.push({ name: entity.name, id: entity.id, icon: entity.icon });
                        }

                    });

                });
            }

            $scope.renderModel = { match: "ViewedAny", nodes: [] };

            if ($scope.dialogOptions.definition) {
                var pagesViewedSettings = JSON.parse($scope.dialogOptions.definition);
                $scope.renderModel = pagesViewedSettings;
                if ($scope.renderModel.nodeIds.length > 0) {
                    loadNodeDetails();
                }
            }

            $scope.openContentPicker = function () {

                var dialogOptions = {
                    multiPicker: true,
                    entityType: "Document",
                    filterCssClass: "not-allowed not-published",
                    startNodeId: null,
                    callback: function (data) {
                        if (angular.isArray(data)) {
                            _.each(data, function (item, i) {
                                $scope.add(item);
                            });
                        } else {
                            $scope.clear();
                            $scope.add(data);
                        }
                    },
                    treeAlias: "content",
                    section: "content"
                };

                var d = dialogService.treePicker(dialogOptions);
            };

            $scope.remove = function (index) {
                $scope.renderModel.nodes.splice(index, 1);
            };

            $scope.add = function (item) {
                var currIds = _.map($scope.renderModel.nodes, function (i) {
                    return i.id;
                });

                if (currIds.indexOf(item.id) < 0) {
                    item.icon = iconHelper.convertFromLegacyIcon(item.icon);
                    $scope.renderModel.nodes.push({ name: item.name, id: item.id, icon: item.icon });
                }
            };

            $scope.clear = function () {
                $scope.renderModel.nodes = [];
            };

            $scope.saveAndClose = function () {

                // Populate nodeIds property that is saved to the definition from the list of selected nodes
                $scope.renderModel.nodeIds = _.map($scope.renderModel.nodes, function (i) {
                    return i.id;
                });

                var serializedResult = "{ \"match\": \"" + $scope.renderModel.match + "\", " +
                    "\"nodeIds\": " + "[" + $scope.renderModel.nodeIds.join() + "]" + " }";
                $scope.submit(serializedResult);
            };

        });
angular.module("umbraco")
    .controller("UmbracoPersonalisationGroups.PagesViewedPersonalisationGroupCriteriaController",
        function ($scope, $injector, entityResource, iconHelper) {

            // In V7 we use dialogService, in V8 it's editorService.
            // So we can't inject them directly as one or other will fail.
            // Instead we'll pull them in manually using $injector, handling when they can't be located.
            var dialogService = null;
            var editorService = null;
            try {
                dialogService = $injector.get("dialogService");
            } catch (e) {
                editorService = $injector.get("editorService");
            }

            // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
            var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

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

            if (definition) {
                var pagesViewedSettings = JSON.parse(definition);
                $scope.renderModel = pagesViewedSettings;
                if ($scope.renderModel.nodeIds.length > 0) {
                    loadNodeDetails();
                }
            }

            function processSelections(selection) {
                if (angular.isArray(selection)) {
                    _.each(selection,
                        function(item) {
                            $scope.add(item);
                        });
                } else {
                    $scope.clear();
                    $scope.add(data);
                }
            }

            $scope.openContentPicker = function () {

                var dialogOptions;
                if (dialogService) {
                    // V7 - use dialogService
                    dialogOptions = {
                        multiPicker: true,
                        entityType: "Document",
                        filterCssClass: "not-allowed not-published",
                        startNodeId: null,
                        callback: function (data) {
                            processSelections(data);
                        },
                        treeAlias: "content",
                        section: "content"
                    };

                    dialogService.treePicker(dialogOptions);
                } else {
                    // V8 - use editorService
                    dialogOptions = {
                        view: "views/common/infiniteeditors/treepicker/treepicker.html",
                        size: "small",
                        section: "content",
                        treeAlias: "content",
                        multiPicker: true,
                        submit: function (data) {
                            processSelections(data.selection);
                            editorService.close();
                        },
                        close: function () {
                            editorService.close();
                        }
                    };
                    editorService.contentPicker(dialogOptions);

                }
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

                // For V7 we use $scope.submit(), for V8 $scope.model.submit()
                if ($scope.submit) {
                    $scope.submit(serializedResult);
                } else {
                    $scope.model.submit(serializedResult);
                }
            };

            $scope.close = function () {
                if ($scope.model.close) {
                    $scope.model.close();
                }
            };

        });
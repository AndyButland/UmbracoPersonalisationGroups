angular.module("umbraco")
.controller("UmbracoPersonalisationGroups.QuerystringPersonalisationGroupCriteriaController",
    function ($scope) {

        // Handle passed value for V7 (will have populated dialogOptions), falling back to V8 if not found.
        var definition = $scope.dialogOptions ? $scope.dialogOptions.definition : $scope.model.definition;

        var self = this;

        self.renderModel = { match: "MatchesValue" };
        self.matchOptions = [
            { key: 'MatchesValue', value: 'Matches value' },
            { key: 'DoesNotMatchValue', value: 'Does not match value' },

            { key: 'ContainsValue', value: 'Contains value' },
            { key: 'DoesNotContainValue', value: 'Does not contain value' },

            { key: 'MatchesRegex', value: 'Matches regular expression' },
            { key: 'DoesNotMatchRegex', value: 'Does not match regular expression' },
        ];

        self.currentMatchIsCaseInsensitive = function() {
            if (!self.renderModel.match) return false;
            var key = self.renderModel.match;
            return key.indexOf('Regex') === -1;
        }

        if (definition) {
            self.renderModel = JSON.parse(definition);
        }

        $scope.saveAndClose = function () {

            var serializedResult = JSON.stringify(self.renderModel);

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
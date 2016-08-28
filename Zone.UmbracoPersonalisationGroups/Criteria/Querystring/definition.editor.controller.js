angular.module("umbraco")
.controller("UmbracoPersonalisationGroups.QuerystringPersonalisationGroupCriteriaController",
    function ($scope) {
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

        if ($scope.dialogOptions.definition) {
            self.renderModel = JSON.parse($scope.dialogOptions.definition);
        }

        $scope.saveAndClose = function () {
            $scope.submit(JSON.stringify(self.renderModel));
        };
    });
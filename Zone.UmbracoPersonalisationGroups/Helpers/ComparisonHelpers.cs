namespace Zone.UmbracoPersonalisationGroups.Helpers
{
    using System;

    public static class ComparisonHelpers
    {
        public static bool CompareValues(string value, string definitionValue, Comparison comparison)
        {
            bool comparisonMade;
            var result = DateCompare(value, definitionValue, comparison, out comparisonMade);
            if (comparisonMade)
            {
                return result;
            }
            result = NumericCompare(value, definitionValue, comparison, out comparisonMade);

            if (comparisonMade)
            {
                return result;
            }

            return StringCompare(value, definitionValue, comparison);
        }

        private static bool DateCompare(string value, string definitionValue, Comparison comparison, out bool comparisonMade)
        {
            DateTime dateValue, dateDefinitionValue;
            if (DateTime.TryParse(value, out dateValue) && DateTime.TryParse(definitionValue, out dateDefinitionValue))
            {
                comparisonMade = true;
                switch (comparison)
                {
                    case Comparison.GreaterThan:
                        return dateValue > dateDefinitionValue;
                    case Comparison.GreaterThanOrEqual:
                        return dateValue >= dateDefinitionValue;
                    case Comparison.LessThan:
                        return dateValue < dateDefinitionValue;
                    case Comparison.LessThanOrEqual:
                        return dateValue <= dateDefinitionValue;
                }
            }

            comparisonMade = false;
            return false;
        }

        private static bool NumericCompare(string value, string definitionValue, Comparison comparison, out bool comparisonMade)
        {
            decimal decimalValue, decimalDefinitionValue;
            if (decimal.TryParse(value, out decimalValue) && decimal.TryParse(definitionValue, out decimalDefinitionValue))
            {
                comparisonMade = true;
                switch (comparison)
                {
                    case Comparison.GreaterThan:
                        return decimalValue > decimalDefinitionValue;
                    case Comparison.GreaterThanOrEqual:
                        return decimalValue >= decimalDefinitionValue;
                    case Comparison.LessThan:
                        return decimalValue < decimalDefinitionValue;
                    case Comparison.LessThanOrEqual:
                        return decimalValue <= decimalDefinitionValue;
                }
            }

            comparisonMade = false;
            return false;
        }

        private static bool StringCompare(string value, string definitionValue, Comparison comparison)
        {
            var comparisonValue = string.Compare(value, definitionValue, StringComparison.InvariantCultureIgnoreCase);
            switch (comparison)
            {
                case Comparison.GreaterThan:
                    return comparisonValue > 0;
                case Comparison.GreaterThanOrEqual:
                    return comparisonValue >= 0;
                case Comparison.LessThan:
                    return comparisonValue < 0;
                case Comparison.LessThanOrEqual:
                    return comparisonValue <= 0;
            }

            return false;
        }
    }
}

namespace Zone.UmbracoPersonalisationGroups.Criteria
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides common base functionality for personalisation criteria
    /// </summary>
    public abstract class PersonalisationGroupCriteriaBase
    {
        protected bool MatchesValue(string valueFromContext, string valueFromDefinition)
        {
            if (valueFromContext == null)
            {
                return false;
            }

            return string.Equals(valueFromContext, valueFromDefinition, 
                StringComparison.InvariantCultureIgnoreCase);
        }

        protected bool ContainsValue(string valueFromContext, string valueFromDefinition)
        {
            if (valueFromContext == null)
            {
                return false;
            }

            return CultureInfo.InvariantCulture.CompareInfo
                .IndexOf(valueFromContext, valueFromDefinition, CompareOptions.IgnoreCase) >= 0;
        }

        protected bool MatchesRegex(string valueFromContext, string valueFromDefinition)
        {
            if (valueFromContext == null)
            {
                return false;
            }

            return Regex.IsMatch(valueFromContext, valueFromDefinition);
        }

        protected bool CompareValues(string value, string definitionValue, Comparison comparison)
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

        private bool DateCompare(string value, string definitionValue, Comparison comparison, out bool comparisonMade)
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

        private bool NumericCompare(string value, string definitionValue, Comparison comparison, out bool comparisonMade)
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

        private bool StringCompare(string value, string definitionValue, Comparison comparison)
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

using FluentValidation;
using TokenApi.Common;

namespace TokenApi.Validation
{
    public static class ValidationExtentions
    {
        public static IRuleBuilderOptions<T, TProperty> WithMessageFormat<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule,
            ApiError apiError)
        {
            return rule.WithMessage($"[{apiError.Code}] {apiError.Message}");
        }
    }
}
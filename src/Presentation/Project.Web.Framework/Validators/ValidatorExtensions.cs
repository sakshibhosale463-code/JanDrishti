using FluentValidation;

namespace Project.Web.Framework.Validators;

/// <summary>
/// Validator extensions
/// </summary>
public static class ValidatorExtensions
{
    /// <summary>
    /// Set decimal validator
    /// </summary>
    /// <typeparam name="TModel">Type of model being validated</typeparam>
    /// <param name="ruleBuilder">Rule builder</param>
    /// <param name="maxValue">Maximum value</param>
    /// <returns>Result</returns>
    public static IRuleBuilderOptions<TModel, decimal> IsDecimal<TModel>(this IRuleBuilder<TModel, decimal> ruleBuilder, decimal maxValue)
    {
        return ruleBuilder.SetValidator(new DecimalPropertyValidator<TModel, decimal>(maxValue));
    }

    /// <summary>
    /// Set email address validator
    /// </summary>
    /// <typeparam name="TModel">Type of model being validated</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
    /// <returns></returns>
    public static IRuleBuilderOptions<TModel, string> IsEmailAddress<TModel>(this IRuleBuilder<TModel, string> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new EmailPropertyValidator<TModel>());
    }
}
using FluentValidation;
using Krypton.Budgets.Blazor.APIClient._Common;
using Krypton.Budgets.Blazor.UI._Main;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Severity = MudBlazor.Severity;

namespace Krypton.Budgets.Blazor.UI._Shared;

public abstract class ErrorHandlingForm<TLoc, TModel> : LocalizedComponent<TLoc>, IErrorHandler
{
    private readonly Dictionary<string, PropertyInfo> fields;
    private Dictionary<PropertyInfo, string?> fieldErrors = new();

    protected MudForm? Form { get; set; } = default!;
    protected FormValidator Validator { get; private init; }

    [Inject]
    protected IStringLocalizer<MainRouter> ErrorL { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    public ErrorHandlingForm()
    {
        fields = typeof(TModel).GetProperties(BindingFlags.Instance | BindingFlags.Public).
            ToDictionary(p => p.Name, p => p);

        Validator = new FormValidator(this);
    }

    async Task IErrorHandler.CheckErrorsAsync<T>(SafeResult<T> result, Action<T> handleResult)
    {
        if (result.IsErrors)
        {
            await MarkErrors(result.Errors);
        }
        else
        {
            handleResult(result.Result);
        }
    }

    async Task IErrorHandler.CheckErrorsAsync<T>(SafeResult<T> result, Func<T, Task> handleResult)
    {
        if (result.IsErrors)
        {
            await MarkErrors(result.Errors);
        }
        else
        {
            await handleResult(result.Result);
        }
    }

    public virtual async Task<bool> PreValidate()
    {
        var valid = true;

        if (Form is MudForm form)
        {
            fieldErrors.Clear();
            await form.Validate();
            valid = form.IsValid;
        }

        return valid && await InnerValidate();
    }

    protected virtual Task<bool> InnerValidate() => Task.FromResult(true);

    protected virtual void ConfigureValidator(AbstractValidator<TModel> validator) { }

    private async Task MarkErrors(ErrorResults errors)
    {
        if (errors.InvalidParams is IEnumerable<ErrorResultsItem> items)
        {
            foreach (var item in items.
                Where(i => i is not null && (i?.Name is not string name || !fields.ContainsKey(name))))
            {
                Snackbar.Add(ErrorL["ERRORS:SERVICE:" + item!.Reason], Severity.Error);
            }

            fieldErrors = items.
                Where(i => i?.Name is string name && fields.ContainsKey(name)).
                ToDictionary(i => fields[i!.Name!], i => i!.Reason);
        }

        if (Form is MudForm form)
        {
            await form.Validate();
        }
    }

    protected class FormValidator : AbstractValidator<TModel>
    {
        public FormValidator(ErrorHandlingForm<TLoc, TModel> owner)
        {
            if (!typeof(IEnumerable).IsAssignableFrom(typeof(TModel)))
            {
                foreach (var field in owner.fields.Values)
                {
                    var parameter = Expression.Parameter(typeof(TModel));
                    var memberExpression = Expression.Property(parameter, field);
                    var conversionExpression = Expression.Convert(memberExpression, typeof(object));
                    var lambdaExpression = Expression.Lambda<Func<TModel, object>>(conversionExpression, parameter);

                    RuleFor(lambdaExpression).
                        Must(_ => !owner.fieldErrors.ContainsKey(field)).
                        WithMessage(_ => owner.ErrorL["ERRORS:FORM:" + owner.fieldErrors[field]]);
                }
            }

            owner.ConfigureValidator(this);
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<TModel>.
                CreateWithOptions((TModel)model, x => x.IncludeProperties(propertyName)));

            if (result.IsValid)
                return Array.Empty<string>();

            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}

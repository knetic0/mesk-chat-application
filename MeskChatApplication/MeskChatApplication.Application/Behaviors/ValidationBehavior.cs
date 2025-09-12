using FluentValidation;
using FluentValidation.Results;
using MESK.MediatR;

namespace MeskChatApplication.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(!_validators.Any()) return await next();
        
        var context = new ValidationContext<TRequest>(request);
        var errors = _validators
            .Select(v => v.Validate(context))
            .SelectMany(v => v.Errors)
            .Where(v => v != null)
            .GroupBy(v => v.PropertyName, v => v.ErrorMessage,
                (propertyName, errorMessage) => new
                {
                    Key = propertyName,
                    Values = errorMessage.Distinct().ToArray()
                })
            .ToDictionary(k => k.Key, v => v.Values[0]);

        if (errors.Any())
        {
            var errs = errors.Select(s => new ValidationFailure
            {
                PropertyName = s.Key,
                ErrorMessage = s.Value
            });
            throw new ValidationException(errs);
        }
        
        return await next();
    }
}
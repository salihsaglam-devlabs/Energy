using System.Reflection;
using Energy.Shared.Models.V1.Common.Responses;
using FluentValidation;
using MediatR;

namespace Energy.Application.Common.Messaging.Behaviors;

/// <summary>
/// MediatR validation pipeline: ilgili request (Command/Query) için DI'da kayıtlı tüm
/// <c>IValidator&lt;TRequest&gt;</c>'leri çalıştırır. Doğrulama başarısızsa ve yanıt türü
/// <see cref="BaseResponse{T}"/> ise, exception fırlatmak yerine standart hata zarfı döndürür
/// (mevcut API davranışıyla tutarlı). Aksi halde <see cref="ValidationException"/> fırlatılır.
/// Request-model doğrulaması ayrıca API ActionFilter'da da çalışmaya devam eder.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToArray();

        if (failures.Length == 0)
        {
            return await next();
        }

        var errors = failures.Select(f => f.ErrorMessage).ToArray();

        if (TryCreateFailureResponse(errors, out var failureResponse))
        {
            return failureResponse;
        }

        throw new ValidationException(failures);
    }

    /// <summary>
    /// <typeparamref name="TResponse"/> bir <see cref="BaseResponse{T}"/> ise, statik
    /// <c>Failure</c> fabrikasını yansıma ile çağırıp hata zarfı üretir.
    /// </summary>
    private static bool TryCreateFailureResponse(string[] errors, out TResponse response)
    {
        response = default!;
        var responseType = typeof(TResponse);
        if (!responseType.IsGenericType ||
            responseType.GetGenericTypeDefinition() != typeof(BaseResponse<>))
        {
            return false;
        }

        var failureMethod = responseType.GetMethod(
            nameof(BaseResponse<object>.Failure),
            BindingFlags.Public | BindingFlags.Static);
        if (failureMethod is null)
        {
            return false;
        }

        response = (TResponse)failureMethod.Invoke(
            null,
            new object?[] { "Validation failed.", errors })!;
        return true;
    }
}


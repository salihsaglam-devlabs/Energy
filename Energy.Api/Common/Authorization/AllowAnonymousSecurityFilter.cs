using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Energy.Api.Common.Authorization;

/// <summary>
/// Suppresses the global Bearer security requirement on operations that are
/// explicitly anonymous (or that have no <see cref="AuthorizeAttribute"/> in
/// their metadata chain). The OpenAPI spec interprets an empty
/// <c>security</c> array as "no authentication required", which hides the
/// lock icon in Swagger UI for those operations.
/// </summary>
public sealed class AllowAnonymousSecurityFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;

        var hasAuthorize = metadata.OfType<AuthorizeAttribute>().Any();
        var hasAllowAnonymous = metadata.OfType<AllowAnonymousAttribute>().Any();

        if (hasAllowAnonymous || !hasAuthorize)
        {
            operation.Security = new List<OpenApiSecurityRequirement>();
        }
    }
}


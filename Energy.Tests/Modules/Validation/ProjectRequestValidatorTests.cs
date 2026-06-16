using Energy.Application.Modules.Projects.Project.Validators;
using Energy.Shared.Models.V1.Projects.Project.Requests;
using Xunit;

namespace Energy.Tests.Modules.Validation;

/// <summary>
/// Üretilen FluentValidation validator'larının zorunlu alan kurallarını uyguladığını
/// doğrular (örnek: CreateProjectRequest — Company/ProjectType/Status/Code/Name).
/// </summary>
public sealed class ProjectRequestValidatorTests
{
    [Fact]
    public void Create_Empty_Fails_With_Required_Errors()
    {
        var validator = new CreateProjectRequestValidator();
        var result = validator.Validate(new CreateProjectRequest());

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateProjectRequest.Code));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateProjectRequest.Name));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateProjectRequest.CompanyId));
    }

    [Fact]
    public void Create_Populated_Passes()
    {
        var validator = new CreateProjectRequestValidator();
        var result = validator.Validate(new CreateProjectRequest
        {
            CompanyId = Guid.NewGuid(),
            ProjectTypeId = Guid.NewGuid(),
            StatusId = Guid.NewGuid(),
            Code = "PRJ-001",
            Name = "Demo Project"
        });

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Update_RequiresId()
    {
        var validator = new UpdateProjectRequestValidator();
        var result = validator.Validate(new UpdateProjectRequest());
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateProjectRequest.Id));
    }
}


using Energy.Domain.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class AccessRuleConfiguration : IEntityTypeConfiguration<AccessRule>
{
    public void Configure(EntityTypeBuilder<AccessRule> builder)
    {
        builder.ToTable("AccessRules");
        builder.HasQueryFilter(rule => !rule.IsDeleted);
        builder.Property(rule => rule.Name).HasMaxLength(150).IsRequired();
        builder.Property(rule => rule.Scope).HasMaxLength(30).IsRequired();
        builder.Property(rule => rule.Path).HasMaxLength(300).IsRequired();
        builder.Property(rule => rule.HttpMethod).HasMaxLength(16).IsRequired();
        builder.Property(rule => rule.Description).HasMaxLength(500).IsRequired();
        builder.HasIndex(rule => new { rule.Scope, rule.Path, rule.HttpMethod }).IsUnique();
    }
}


using Energy.Domain.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.ToTable("Logs");

        builder.HasKey(log => log.Id)
            .HasName("PK_ApiRequestLogs");

        builder.Property(log => log.Id)
            .ValueGeneratedNever();

        builder.Property(log => log.TraceId)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(log => log.CorrelationId)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(log => log.HttpMethod)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(log => log.Path)
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(log => log.QueryString)
            .HasColumnType("text");

        builder.Property(log => log.RequestHeaders)
            .HasColumnType("text");

        builder.Property(log => log.RequestPayload)
            .HasColumnType("text");

        builder.Property(log => log.ContentType)
            .HasMaxLength(256);

        builder.Property(log => log.ResponseHeaders)
            .HasColumnType("text");

        builder.Property(log => log.ResponsePayload)
            .HasColumnType("text");

        builder.Property(log => log.UserId)
            .HasMaxLength(128);

        builder.Property(log => log.UserName)
            .HasMaxLength(256);

        builder.Property(log => log.UserEmail)
            .HasMaxLength(256);

        builder.Property(log => log.ClientId)
            .HasMaxLength(128);

        builder.HasIndex(log => log.ClientId)
            .HasDatabaseName("IX_Logs_ClientId");
        
        builder.Property(log => log.ClientIpAddress)
            .HasMaxLength(128);

        builder.Property(log => log.ClientMachineName)
            .HasMaxLength(256);

        builder.Property(log => log.UserAgent)
            .HasColumnType("text");

        builder.Property(log => log.ServerMachineName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(log => log.ApplicationName)
            .HasMaxLength(256);

        builder.Property(log => log.EnvironmentName)
            .HasMaxLength(128);

        builder.Property(log => log.ExceptionType)
            .HasMaxLength(1024);

        builder.Property(log => log.ExceptionMessage)
            .HasColumnType("text");

        builder.Property(log => log.ExceptionStackTrace)
            .HasColumnType("text");

        builder.Property(log => log.InnerExceptionMessage)
            .HasColumnType("text");

        builder.Property(log => log.RequestStartedAtUtc)
            .HasColumnType("timestamp with time zone");

        builder.Property(log => log.RequestCompletedAtUtc)
            .HasColumnType("timestamp with time zone");

        builder.Property(log => log.CreatedAtUtc)
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(log => log.CreatedAtUtc)
            .HasDatabaseName("IX_ApiRequestLogs_CreatedAtUtc");

        builder.HasIndex(log => log.UserId)
            .HasDatabaseName("IX_ApiRequestLogs_UserId");

        builder.HasIndex(log => log.UserName)
            .HasDatabaseName("IX_ApiRequestLogs_UserName");

        builder.HasIndex(log => log.ClientIpAddress)
            .HasDatabaseName("IX_ApiRequestLogs_ClientIpAddress");

        builder.HasIndex(log => log.IsSuccess)
            .HasDatabaseName("IX_ApiRequestLogs_IsSuccess");

        builder.HasIndex(log => log.HasException)
            .HasDatabaseName("IX_ApiRequestLogs_HasException");

        builder.HasIndex(log => log.CorrelationId)
            .HasDatabaseName("IX_ApiRequestLogs_CorrelationId");

        builder.HasIndex(log => new
            {
                log.HttpMethod,
                log.Path,
                log.CreatedAtUtc
            })
            .HasDatabaseName("IX_ApiRequestLogs_HttpMethod_Path_CreatedAtUtc");
    }
}
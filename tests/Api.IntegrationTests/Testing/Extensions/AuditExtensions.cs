namespace Argo.CA.Api.IntegrationTests.Testing.Extensions;

using Domain.Common;
using AwesomeAssertions;

public static class AuditExtensions
{
    public static void ValidateCreatedBy(this IAuditCreated entity, string userName)
    {
        entity.CreatedBy.Should().Be(userName);
    }

    public static void ValidateModifiedBy(this IAuditModified entity, string userName)
    {
        entity.ModifiedBy.Should().Be(userName);
    }
}

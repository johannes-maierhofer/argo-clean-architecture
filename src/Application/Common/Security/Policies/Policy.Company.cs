namespace Argo.CA.Application.Common.Security.Policies;

public static partial class Policy
{
    public abstract class Company
    {
        public const string Get = "get:company";
        public const string Create = "create:company";
        public const string Update = "update:company";
        public const string Delete = "delete:company";
    }
}
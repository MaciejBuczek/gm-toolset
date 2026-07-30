namespace Identity.API
{
    public static class Constants
    {
        public const string DefaultSchema = "Identity";
        public const string DefaultMigrationsHistoryTable = "__EFMigrationsHistory";
        public const string ConnectionStringName = "Database";
        public const string ExceptionSeparator = "|";

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string AdminNormalized = "ADMIN";
            public static readonly Guid AdminId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            public const string User = "User";
            public const string UserNormalized = "USER";
            public static readonly Guid UserId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        }
    }
}

namespace LogiFlowAPI.Services.Common
{
    public static class ClientMessages
    {
        public static class Identity
        {
            public const string InvalidCredentials = "Invalid credentials.";
        }

        public static string NotFoundOrInaccessible { get; set; } = "Unable to access the requested resource. Please check if the resource exists and ensure you have the necessary permissions.";
    }
}

namespace LogiFlowAPI.Web.Models.Auth
{
    public class TokenPairModel
    {
        public string RefreshToken { get; set; }

        public string AccessToken { get; set; }
    }
}

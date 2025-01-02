namespace LogiFlowAPI.Web.Models
{
    public class TokenSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Secret { get; set; }

        public int RefreshTokenExpiryInDays { get; set; }

        public int AuthTokenExpiryInMinutes { get; set; }
    }
}

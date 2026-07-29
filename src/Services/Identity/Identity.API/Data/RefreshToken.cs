namespace Identity.API.Data
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public AppUser? User { get; set; }

        public required string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsExpired { get; set; }
    }
}
namespace Identity.API.Data
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required AppUser User { get; set; }

        public required string Token { get; set; }
        public required DateTime ExpirationDate { get; set; }
    }
}
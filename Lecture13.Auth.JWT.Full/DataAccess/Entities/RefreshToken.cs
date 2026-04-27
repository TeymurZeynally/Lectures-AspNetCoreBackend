namespace Lecture13.Auth.JWT.Full.DataAccess.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public required Guid Uid { get; set; }

        public required byte[] TokenHash { get; set; }

        public required DateTimeOffset CreationTimestamp { get; set; }

        public required DateTimeOffset ExpirationTimestamp { get; set; }

        public DateTimeOffset? RevokedTimestamp { get; set; }

        public required User User { get; set; }
    }
}

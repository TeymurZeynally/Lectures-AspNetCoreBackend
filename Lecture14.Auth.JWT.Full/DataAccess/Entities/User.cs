namespace Lecture14.Auth.JWT.Full.DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }

        public required Guid Uid { get; set; }

        public required string Login { get; set; }

        public required byte[] PasswordHash { get; set; }

        public required byte[] PasswordSalt { get; set; }

        public required string Role { get; set; }

        public required DateTimeOffset CreationTimestamp { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; } = [];
    }
}

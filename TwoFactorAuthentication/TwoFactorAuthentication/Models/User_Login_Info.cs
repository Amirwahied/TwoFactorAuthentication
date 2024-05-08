namespace TwoFactorAuthentication.Models
{
    public sealed record User_Login_Info : BaseEntity
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required byte[] Password { get; set; }
        public required byte[] Salt { get; set; }
        public bool Is_Active { get; set; } = false;
        public bool Is_TFA_Enabled { get; set; } = false;
        public DateTime? TFA_LastUse { get; set; }
        public required byte[] Token { get; set; }
        public string? Authenticator_Key { get; set; }
        public int? Created_By { get; set; }
    }
}

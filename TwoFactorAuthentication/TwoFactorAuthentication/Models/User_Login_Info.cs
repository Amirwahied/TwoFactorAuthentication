namespace TwoFactorAuthentication.Models
{
    public sealed record User_Login_Info : BaseEntity
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required byte[] Password { get; set; }
        public required byte[] Salt { get; set; }
        public required bool Is_Active { get; set; }
        public required bool Is_TFA_Enabled { get; set; }
        public required DateTime TFA_LastUse { get; set; }
        public required string Token { get; set; }
        public string? Authenticator_Key { get; set; }
        public int? CreatedBy { get; set; }
    }
}

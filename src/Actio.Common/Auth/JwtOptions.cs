namespace Actio.Common.Auth;

public class JwtOptions
{
    public JwtOptions()
    {
        
    }
    public JwtOptions(string secretKey,int expiryMinutes, string issuer)
    {
        SecretKey = secretKey;
        ExpiryMinutes = expiryMinutes;
        Issuer = issuer;
    }

    public string SecretKey { get; set; } = "";
    public int ExpiryMinutes { get; set; } = 0;
    public string Issuer { get; set; } = "";

    public void Deconstruct(out string SecretKey, out int ExpiryMinutes, out string Issuer)
    {
        SecretKey = this.SecretKey;
        ExpiryMinutes = this.ExpiryMinutes;
        Issuer = this.Issuer;
    }
}
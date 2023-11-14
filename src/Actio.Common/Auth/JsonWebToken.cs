namespace Actio.Common.Auth;

public class JsonWebToken()
{
    public long Expires { get; set; }
    public string Token { get; set; }

    public void Deconstruct(out string Token)
    {
        Token = this.Token;
    }
}
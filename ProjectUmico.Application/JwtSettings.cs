namespace ProjectUmico.Api.Common;

public class JwtSettings
{
    public string Secret { get; set; }
    public DateTime Expires { get; set; }
}
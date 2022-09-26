namespace ProjectUmico.Application.Contracts.Authentication.v1;

public class UserAuthenticationRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
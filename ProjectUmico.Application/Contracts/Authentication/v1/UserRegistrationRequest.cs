namespace ProjectUmico.Application.Contracts.Authentication.v1;

public class UserRegistrationRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    
}
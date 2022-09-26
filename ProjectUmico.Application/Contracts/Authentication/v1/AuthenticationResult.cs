namespace ProjectUmico.Application.Contracts.Authentication.v1;

public class AuthenticationResult
{
    public string? Token { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    public bool Success { get; set; }
    
    
}

public static class AuthenticationResultExtensions
{
    public static AuthenticationResult SuccessResponseFromToken(string token)
    {
        return new AuthenticationResult()
        {
            Success = true,
            Token = token,
            Errors = Array.Empty<string>()
        };
    }

    public static AuthenticationResult FailResponseFromErrors(IEnumerable<string> errors)
    {
        return new AuthenticationResult()
        {
            Success = false,
            Errors = errors
        };
    }
    
    
    public static AuthenticationResult FailResponseForInvalidUserCredentials()
    {
        return new AuthenticationResult()
        {
            Success = false,
            Errors = new []
            {
                "User password/login combination is invalid."
            }
        };
    }
}


namespace ProjectUmico.Application.Common.Models;

public class Result
{
    public Result(bool succeded,IEnumerable<string> errors)
    {
        Succeded = succeded;
        Errors = errors.ToArray();
    }
    public string[] Errors { get; set; }

    public bool Succeded { get; set; }

    public static Result Success()
    {
        return new Result(true,Array.Empty<string>());
    }
    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false,errors);
    }
    
    public static Result Failure()
    {
        return new Result(false,Array.Empty<string>());
    }
}
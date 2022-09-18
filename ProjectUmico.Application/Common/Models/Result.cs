using AutoMapper;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Application.Common.Models;

public class Result<T> where T:class
{
    public Result(bool succeded,IEnumerable<string> errors)
    {
        Succeded = succeded;
        Errors = errors.ToArray();
    }
    public Result(bool succeded,IEnumerable<string> errors,T value)
    {
        Succeded = succeded;
        Errors = errors.ToArray();
        Value = value;
    }
    
    
    public string[] Errors { get; set; }

    public bool Succeded { get; set; }

    public static Result<T> Success(T value = null)
    {
        return new Result<T>(true,Array.Empty<string>(),value);
    }
    
    public static Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>(false,errors);
    }
    
    public static Result<T> Failure()
    {
        return new Result<T>(false,Array.Empty<string>());
    }

    public T? Value { get;}
   
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Application.Common.Models;

public class Result<T> where T:class
{
    public Result(bool succeded,ICollection<string> errors)
    {
        Succeded = succeded;
        Errors = errors.ToArray();
    }
    public Result(bool succeded,ICollection<string> errors,T value)
    {
        Succeded = succeded;
        Errors = errors.ToArray();
        Value = value;
    }
    
    public string[] Errors { get; set; }
    public bool Succeded { get; set; }
    public Exception? exception { get; set; }
    

    public static Result<T> Success(T value = null)
    {
        return new Result<T>(true,Array.Empty<string>(),value);
    }
    
    public static Result<T> Failure(ICollection<string> errors)
    {
        return new Result<T>(false,errors);
    }
    
    public static Result<T> Failure()
    {
        return new Result<T>(false,Array.Empty<string>());
    }    
    public static Result<T> Failure(Exception exception)
    {
        return new Result<T>(false,new[]{exception.Message});
    }
    public T? Value { get;}

    public IActionResult Match(Func<IActionResult> successHandler
        ,Func<Exception,IActionResult> exceptionHandler)
    {
        if (exception is not null)
        {
            return exceptionHandler.Invoke(exception);
        }

        if (Succeded is not true)
        {
            return new ObjectResult(500);
        }
        
        return successHandler.Invoke();
    }
    
    public IActionResult Match(Func<IActionResult> successHandler
        ,Func<Exception,IActionResult> exceptionHandler,
        Func<IActionResult> failHandler)
    {
        if (exception is not null)
        {
            return exceptionHandler.Invoke(exception);
        }

        if (Succeded is not true)
        {
            return failHandler.Invoke();
        }
        
        return successHandler.Invoke();
    }
    
}


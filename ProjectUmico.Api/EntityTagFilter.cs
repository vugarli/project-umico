using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Api;


[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method,AllowMultiple = false)]
public class EntityTagFilter : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;
        var executedContext = await next();
        var response = context.HttpContext.Response;
        
        // Computing ETags for Response Caching on GET requests
        if (request.Method is "GET" && response.HttpContext.Response.StatusCode is StatusCodes.Status200OK)
        {
            HandleEntityTagForResponseCaching(executedContext);
        }
    }

    private void HandleEntityTagForResponseCaching(ActionExecutedContext executedContext)
    {
        if (executedContext.Result == null)
        {
            return; // no need to etag if no result 
        }
        
        var request = executedContext.HttpContext.Request;
        var response = executedContext.HttpContext.Response;

        // TODO handle null
        var result = (ICachable)(executedContext.Result as ObjectResult).Value;

        if (result?.LastModified is null)
        {
            return;
        }
        
        var hashCode = result.LastModified.GetHashCode(); // TODO implement proper hashing
        
        var entityTag = hashCode.ToString();

        if (request.Headers.ContainsKey(HeaderNames.IfNoneMatch))
        {
            if (request.Headers[HeaderNames.IfNoneMatch].ToString() == entityTag)
            {
                executedContext.Result = new StatusCodeResult((int) HttpStatusCode.NotModified);
            }
        }
        response.Headers.Add(HeaderNames.ETag,new[]{entityTag});
    }
}


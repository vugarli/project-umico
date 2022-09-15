using Microsoft.AspNetCore.Mvc;

namespace ProjectUmico.Api.Controllers.v1;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ApiControllerBasev1 : ControllerBase
{
    
}
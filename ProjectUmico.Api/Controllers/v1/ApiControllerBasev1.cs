using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectUmico.Api.Controllers.v1;


[ApiController]
[ApiVersion("1.0")]
[Authorize("RequiresAuthentication")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ApiControllerBasev1 : ControllerBase
{
    
}
using System;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]  // this means that the cotroller will be replaced with the first part of class name
public class BaseApiController : ControllerBase // localhost/api/users
{

}

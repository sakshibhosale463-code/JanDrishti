using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Services.Batch;

namespace Project.Web.Framework;

[Authorize]
public class BaseProtectedController : BaseController
{
    
}

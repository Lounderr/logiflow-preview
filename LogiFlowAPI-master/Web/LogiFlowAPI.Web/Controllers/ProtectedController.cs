﻿namespace LogiFlowAPI.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    public abstract class ProtectedController : ControllerBase
    {
    }
}

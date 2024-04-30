// Copyright (C) TBC Bank. All Rights Reserved.

using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers;

[Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class CustomBaseController : ControllerBase
{
}

using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Core;

/// <summary>
/// ����������
/// </summary>
[Route("api/[area]/[controller]/[action]")]
[ApiController]
[ValidatePermission]
[ValidateInput]
public abstract class BaseController : ControllerBase
{
}

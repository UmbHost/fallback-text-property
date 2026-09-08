using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Authorization;   // AuthorizationPolicies.BackOfficeAccess
using Wholething.FallbackTextProperty.Services;

namespace Wholething.FallbackTextProperty.Api;

// Plain ApiController guarded by the back-office access policy, replacing the
// removed UmbracoAuthorizedApiController. Serves the property dictionary the
// Lit editor renders its Mustache preview from.
[ApiController]
[Route("umbraco/fallbacktext")]
[Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]
public class FallbackTextController : ControllerBase
{
    private readonly IFallbackTextService _fallbackTextService;

    public FallbackTextController(IFallbackTextService fallbackTextService)
        => _fallbackTextService = fallbackTextService;

    [HttpGet("dictionary")]
    public IActionResult Dictionary(
        [FromQuery] Guid nodeId,
        [FromQuery] string? template,
        [FromQuery] string? culture,
        [FromQuery] Guid? blockId)
    {
        var dict = _fallbackTextService.BuildDictionary(nodeId, blockId, template, culture);
        return Ok(dict);
    }
}

using System.Collections.Generic;
using UmbHost.FallbackTextProperty.Services.Models;

namespace UmbHost.FallbackTextProperty.Services
{
    public interface IFallbackTextReferenceParser
    {
        List<FallbackTextFunctionReference> Parse(string template);
    }
}

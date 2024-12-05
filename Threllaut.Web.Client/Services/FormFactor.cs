using Threllaut.Shared.Services;

namespace Threllaut.Web.Client.Services;

public class FormFactor : IFormFactor
{
    public string GetFormFactor() => "WebAssembly";

    public string GetPlatform() => Environment.OSVersion.ToString();
}

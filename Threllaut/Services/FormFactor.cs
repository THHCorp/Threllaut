using Threllaut.Shared.Services;

namespace Threllaut.Services;

public class FormFactor : IFormFactor
{
    public string GetFormFactor() => DeviceInfo.Idiom.ToString();

    public string GetPlatform() => DeviceInfo.Platform.ToString() + " - " + DeviceInfo.VersionString;
}

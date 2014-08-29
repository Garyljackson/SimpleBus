using System.Collections.Generic;

namespace SimpleBus.Configuration
{
    public interface IValidatableConfigurationSetting
    {
        IEnumerable<string> Validate();
    }
}
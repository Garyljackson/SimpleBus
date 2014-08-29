using System.Collections.Generic;

namespace SimpleBus.Configuration.Settings
{
    public abstract class Setting<T> : IValidatableConfigurationSetting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected Setting()
        {
            Value = Default;
        }

        public T Value { get; set; }

        public virtual T Default
        {
            get { return default(T); }
        }

        public virtual IEnumerable<string> Validate()
        {
            yield break;
        }

        public static implicit operator T(Setting<T> setting)
        {
            return setting.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
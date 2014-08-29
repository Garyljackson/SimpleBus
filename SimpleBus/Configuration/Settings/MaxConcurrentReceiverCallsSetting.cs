namespace SimpleBus.Configuration.Settings
{
    public class MaxConcurrentReceiverCallsSetting : Setting<int>
    {
        public override int Default
        {
            get { return 1; }
        }
    }
}
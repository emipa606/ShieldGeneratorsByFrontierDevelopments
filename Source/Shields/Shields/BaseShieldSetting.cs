namespace FrontierDevelopments.Shields
{
    public abstract class BaseShieldSetting<T> : ShieldSetting
    {
        private readonly T _value;

        protected BaseShieldSetting(T value)
        {
            _value = value;
        }

        public T Get()
        {
            return _value;
        }
    }
}
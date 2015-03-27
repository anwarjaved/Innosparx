namespace Framework.Ioc
{
    /// <summary>
    /// This lifetime manager’s behavior is to always return a new instance when the Resolve method is called by executing the factory method.  This is the default behavior.
    /// </summary>
    internal class TransientLifetime : ILifetime
    {
        public object GetInstance(IBindingInfo dependencyInfo)
        {
            object instance = dependencyInfo.Instance();

            return instance;
        }

        public void ReleaseInstance(IBindingInfo dependencyInfo)
        {
        }
    }
}

namespace Framework.Wpf.MVVM
{
    using Framework.Ioc;

    /// <summary>
    /// This class contains references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private readonly ViewModelIndexer indexer;

        public ViewModelLocator()
        {
            this.indexer = new ViewModelIndexer();
        }

        public ViewModelIndexer Get
        {
            get
            {
                return this.indexer;
            }
        }


        public static T GetView<T>() where T : ViewModelBase
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                return null;
            }

            return Container.TryGet<ViewModelBase>(typeof(T).Name) as T;
        }
    }
}

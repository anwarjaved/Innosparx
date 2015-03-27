using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Wpf.MVVM
{
    using Framework.Ioc;

    public class ViewModelIndexer
    {
        public object this[string viewModel]
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                {
                    return null;
                }

                return Container.TryGet<ViewModelBase>(viewModel);
            }
        }

    }
}

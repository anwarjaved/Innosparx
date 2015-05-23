using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure
{
    using System.ComponentModel;
    using System.Security;

    using Framework.Localization;

    using Container = Framework.Ioc.Container;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SqlLocalizationInitializer
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        
        public static void Init()
        {
            Container.OverrideDefaultService<ILanguageProvider>("SqlLocalization");
        }
    }
}

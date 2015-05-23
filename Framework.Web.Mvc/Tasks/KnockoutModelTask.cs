namespace Framework.Tasks
{
    using System;
    using System.IO;
    using System.Security;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Framework.Configuration;
    using Framework.Ioc;
    using Framework.Knockout;

    [InjectBind(typeof(IBootstrapTask), "KnockoutModelTask")]
    [Order(6)]
    public class KnockoutModelTask : IBootstrapTask
    {
        private static volatile bool done;

        private static readonly object SyncLock = new object();

        
        public void Execute()
        {
            if (HostingEnvironment.IsHosted)
            {
                if (!done)
                {
                    lock (SyncLock)
                    {
                        if (!done)
                        {
                            done = true;
                            var path = HostingEnvironment.MapPath(Path.Combine(WebConstants.AssetsFolderPath, "js"));
                            KnockoutModelBuilder.Save(path);
                        }
                    }
                }
            }
        }
    }
}

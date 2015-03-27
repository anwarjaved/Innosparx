using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

using Framework;
using Framework.Activator;
using Framework.Infrastructure;
using Framework.Ioc;

[assembly: AssemblyTitle("Inno Sparx Db Logging Adapter")]
[assembly: AssemblyDescription("Db Logging Adapter")]
[assembly: AssemblyProduct("Inno Sparx Framework")]
[assembly: AssemblyCopyright("Copyright © 2008-2014")]
[assembly: AssemblyTrademark("Copyright © 2008-2014, All right reserved.")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: Guid("dbd2e12b-51d7-43ca-9bfd-877ecdca7236")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityRules(SecurityRuleSet.Level2)]
[assembly: ContextName(DbLogConstants.SystemLogsContext)]
[assembly: PreApplicationStartMethod(typeof(SystemLogInitializer), "Init")]
[assembly: ContainerAssembly]
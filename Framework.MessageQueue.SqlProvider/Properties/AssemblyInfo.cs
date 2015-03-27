using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

using Framework;
using Framework.Activator;
using Framework.Infrastructure;
using Framework.Ioc;

[assembly: AssemblyTitle("Inno Sparx Sql MessageQueue Provider")]
[assembly: AssemblyDescription("Sql MessageQueue Provider")]
[assembly: AssemblyProduct("Inno Sparx Framework")]
[assembly: AssemblyCopyright("Copyright ©  2008-2014")]
[assembly: AssemblyTrademark("Copyright ©  2008-2014, All right reserved.")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: Guid("a49039a9-29a0-4644-a53a-500f0049e099")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityRules(SecurityRuleSet.Level2)]
[assembly: ContainerAssembly]
[assembly: PreApplicationStartMethod(typeof(SqlMessageQueueInitializer), "Init")]
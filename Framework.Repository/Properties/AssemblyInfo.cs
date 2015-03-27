using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

using Framework.Activator;
using Framework.Ioc;

[assembly: AssemblyTitle("Inno Sparx Repository Framework")]
[assembly: AssemblyDescription("Repository Module")]
[assembly: AssemblyProduct("Inno Sparx Framework")]
[assembly: AssemblyCopyright("Copyright © 2008-2014")]
[assembly: AssemblyTrademark("Copyright © 2008-2014, All right reserved.")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: Guid("628b0f83-bd49-4e6c-a817-919bb30e8df7")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityRules(SecurityRuleSet.Level2)]
[assembly: ContainerAssembly]
[assembly: PreApplicationStartMethod(typeof(Framework.Infrastructure.RepositoryInitializer), "Init")]
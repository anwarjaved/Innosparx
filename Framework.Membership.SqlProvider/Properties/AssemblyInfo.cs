using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

using Framework;
using Framework.Activator;
using Framework.Infrastructure;
using Framework.Ioc;

[assembly: AssemblyTitle("Inno Sparx Sql Membership Provider")]
[assembly: AssemblyDescription("Sql Membership Provider")]
[assembly: AssemblyProduct("Inno Sparx Framework")]
[assembly: AssemblyCopyright("Copyright © 2008-2014")]
[assembly: AssemblyTrademark("Copyright © 2008-2014, All right reserved.")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: Guid("ddd7a3e7-ba7e-45d5-832e-cf8b95feb6c8")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: ContainerAssembly]
[assembly: PreApplicationStartMethod(typeof(SqlMembershipInitializer), "Init")]


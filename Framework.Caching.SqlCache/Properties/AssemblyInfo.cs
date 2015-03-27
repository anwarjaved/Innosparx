using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

using Framework;
using Framework.Activator;
using Framework.Caching.SqlCache.Infrastructure;
using Framework.Ioc;

[assembly: AssemblyTitle("Inno Sparx Sql Cache")]
[assembly: AssemblyDescription("Sql Cache")]
[assembly: AssemblyCompany("www.lucemorker.com")]
[assembly: AssemblyProduct("Inno Sparx Framework")]
[assembly: AssemblyCopyright("Copyright © Luce & Morker 2008-2014")]
[assembly: AssemblyTrademark("Copyright © Luce & Morker 2008-2014, All right reserved.")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: Guid("b474c2e9-a29c-4c36-8361-b8f0027d8fbf")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityRules(SecurityRuleSet.Level2)]
[assembly: ContainerAssembly]
[assembly: PreApplicationStartMethod(typeof(SqlCacheInitializer), "Init")]
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

using Framework;
using Framework.Activator;
using Framework.Ioc;
using Framework.Services.Infrastructure;

[assembly: AssemblyTitle("Inno Sparx Email Service SqlProvider")]
[assembly: AssemblyDescription("Email Service Sql Provider")]
[assembly: AssemblyProduct("Inno Sparx Framework")]
[assembly: AssemblyCopyright("Copyright ©  2008-2014")]
[assembly: AssemblyTrademark("Copyright ©  2008-2014, All right reserved.")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: Guid("dc531d27-39df-40dd-a37d-03d4a64d6367")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: ContainerAssembly]
[assembly: PreApplicationStartMethod(typeof(SqlEmailServiceInitializer), "Init")]

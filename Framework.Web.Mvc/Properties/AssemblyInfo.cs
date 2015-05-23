using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

using Framework.Activator;
using Framework.Infrastructure;
using Framework.Ioc;

[assembly: AssemblyTitle("Inno Sparx Web Framework")]
[assembly: AssemblyDescription("Web Module")]
[assembly: AssemblyProduct("Inno Sparx Framework")]
[assembly: AssemblyCopyright("Copyright ©  2008-2014")]
[assembly: AssemblyTrademark("Copyright ©  2008-2014, All right reserved.")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: Guid("073391d2-95ce-42b1-8030-ea0127585b25")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: ContainerAssembly]
[assembly: PreApplicationStartMethod(typeof(WebInitializer), "Init")]
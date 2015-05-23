using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

using Framework.Activator;
using Framework.Infrastructure;
using Framework.Ioc;

[assembly: AssemblyTitle("Inno Sparx Serialization Framework")]
[assembly: AssemblyDescription("Serialization Module")]
[assembly: AssemblyProduct("Inno Sparx Framework")]
[assembly: AssemblyCopyright("Copyright ©  2008-2014")]
[assembly: AssemblyTrademark("Copyright ©  2008-2014, All right reserved.")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: Guid("7dde6683-4d10-4007-89a0-a8438836fd93")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: PreApplicationStartMethod(typeof(SerializationInitializer), "PreInit")]
[assembly: ContainerAssembly]
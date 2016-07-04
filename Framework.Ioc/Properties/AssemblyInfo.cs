using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

using Framework.Activator;
using Framework.Ioc;

[assembly: AssemblyTitle("Inno Sparx Framework Ioc Container")]
[assembly: AssemblyDescription("IoC Module")]
[assembly: AssemblyProduct("Inno Sparx Framework")]
[assembly: AssemblyCopyright("Copyright © 2008-2014")]
[assembly: AssemblyTrademark("Copyright © 2008-2014, All right reserved.")]
[assembly: ComVisible(false)]
[assembly: Guid("19b649d0-2e64-4353-84b5-6bb42d909c0e")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: System.Web.PreApplicationStartMethod(typeof(ActivationManager), "Run")]
[assembly: ContainerAssembly]

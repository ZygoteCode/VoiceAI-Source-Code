using System;
using System.IO;
using System.Reflection;
using CefSharp;
using CefSharp.Internals;

// Token: 0x02000012 RID: 18
public class SelfHost
{
	// Token: 0x0600013F RID: 319 RVA: 0x0000825C File Offset: 0x0000645C
	public static int Main(string[] args)
	{
		if (string.IsNullOrEmpty(CommandLineArgsParser.GetArgumentValue(args, "--type")))
		{
			return -1;
		}
		Type type = Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(typeof(BrowserSettings).Assembly.Location), "CefSharp.BrowserSubprocess.Core.dll")).GetType("CefSharp.BrowserSubprocess.BrowserSubprocessExecutable");
		Activator.CreateInstance(type);
		MethodInfo method = type.GetMethod("MainSelfHost", BindingFlags.Static | BindingFlags.Public);
		method.GetParameters();
		object[] array = new object[] { args };
		return (int)method.Invoke(null, array);
	}
}

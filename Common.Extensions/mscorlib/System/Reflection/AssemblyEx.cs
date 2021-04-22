#pragma warning disable RCS1110
using System;
using System.Reflection;

public static class AssemblyEx
{
	public static DateTime GetBuildDate(this Assembly assembly)
	{
		return assembly.GetName().Version.ToDate();
	}
}
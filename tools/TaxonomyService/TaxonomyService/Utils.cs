using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using log4net;
using log4net.Config;

namespace TTI.TTF.Taxonomy
{
	public static class Utils
	{
		public static void InitLog()
		{
			var xmlDocument = new XmlDocument();
			try
			{
				if (Os.IsWindows())
					xmlDocument.Load(File.OpenRead(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
					                               "\\log4net.config"));
				else
					xmlDocument.Load(File.OpenRead(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
					                               "/log4net.config"));
			}
			catch (Exception)
			{
				if (Os.IsWindows())
					xmlDocument.Load(File.OpenRead(
						Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\log4net.config"));
				else
					xmlDocument.Load(File.OpenRead(
						Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/log4net.config"));
			}

			XmlConfigurator.Configure(
				LogManager.CreateRepository(Assembly.GetEntryAssembly(),
					typeof(log4net.Repository.Hierarchy.Hierarchy)), xmlDocument["log4net"]);
		}

		public static string FirstToUpper(string nameString)
		{
			var ch = nameString.ToCharArray();
			for (var i = 0; i < nameString.Length; i++)
			{

				// If first character of a word is found 
				if ((i != 0 || ch[i] == ' ') && (ch[i] == ' ' || ch[i - 1] != ' ')) continue;
				// If it is in lower-case 
				if (ch[i] >= 'a' && ch[i] <= 'z')
				{

					// Convert into Upper-case 
					ch[i] = (char) (ch[i] - 'a' + 'A');
				}
			}

			return ch.ToString();
		}

		public static (string Name, string visual, string tooling) GetRandomArtifactFromArtifact(string name,
			string visual ,string tooling)
		{
			var randStr = Randomize("");
			return (name + randStr, visual + randStr, tooling + randStr);
		}
		
		public static (string Name, string visual, string tooling) GetRandomTemplate(string name,
			string visual ,string tooling)
		{
			var randStr = Randomize("");
			return (name + randStr, visual + randStr, tooling + randStr);
		}

		public static string Randomize(string input)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
			var stringChars = new char[1];
			var random = new Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			return new string(input+stringChars);
		}
	}

	public static class Os
	{
		public static bool IsWindows()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		}

		public static bool IsMacOs()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
		}

		public static bool IsLinux()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
		}

		public static string WhatIs()
		{
			var os = (IsWindows() ? "win" : null) ??
			         (IsMacOs() ? "mac" : null) ??
			         (IsLinux() ? "gnu" : null);
			return os;
		}
	}
}
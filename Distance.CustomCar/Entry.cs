using Distance.CustomCar.Data;
using Distance.CustomCar.Data.Car;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using Reactor.API.Storage;
using System.IO;
using UnityEngine;

namespace Distance.CustomCar
{
	[ModEntryPoint("com.github.larnin/CustomCar")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public IManager Manager { get; set; }

		public Log Logger { get; set; }

		public FileSystem Files { get; set; }
		
		public ErrorList Errors { get; set; }

		public CarFactory CarFactory { get; set; }

		public void Initialize(IManager manager)
		{
			Instance = this;
			Manager = manager;

			Logger = LogManager.GetForCurrentAssembly();
			Files = new FileSystem();

			Errors = new ErrorList(Logger);

			CarFactory = new CarFactory(Errors);

			RuntimePatcher.AutoPatch();
		}

		public DirectoryInfo GetLocalFolder(string dir)
		{
			return new DirectoryInfo(Path.GetDirectoryName(Path.Combine(Files.VirtualFileSystemRoot, dir)));
		}

		public void Load()
		{
			CarFactory.Prefabs.LoadAll();
		}
	}
}
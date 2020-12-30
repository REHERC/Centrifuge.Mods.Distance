using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Distance.AdventureMaker
{
	[ModEntryPoint("com.github.reherc/Distance.AdventureMaker")]
	public class Mod : MonoBehaviour
	{
		public static Mod Instance;

		public IManager Manager { get; set; }

		public Log Logger { get; set; }

		public void Initialize(IManager manager)
		{
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;
			Logger = LogManager.GetForCurrentAssembly();
			
			RuntimePatcher.AutoPatch();
		}
	}
}
using Distance.AdventureMaker.Interfaces;
using System.IO;
using UnityEngine;

namespace Distance.AdventureMaker.Scripts.Managers.Campaigns
{
	public class CampaignManager : MonoBehaviour
	{
		public ILevelRegister Levels { get; private set; }

		public void Awake()
		{
			Levels = new LevelRegister();
		}

		
	}
}

using Distance.TrackMusic.Models;
using UnityEngine;

namespace Distance.TrackMusic
{
	public class VariablesLogic : MonoBehaviour
	{
		public string MusicTrackOptions { get; set; } = Options.CustomType.Format(CustomInspector.Type.StringWithButton);

		public string MusicTrackButtonOptions { get; set; } = Options.CustomType.Format(CustomInspector.Type.StringButton) + Options.DontUndoOption_;

		public AttachedData<MusicTrack> CachedMusicTrack { get; set; } = new AttachedData<MusicTrack>();

		public AttachedData<MusicChoice> CachedMusicChoice { get; set; } = new AttachedData<MusicChoice>();

		public AttachedData<MusicZoneData> CachedMusicZoneData { get; set; } = new AttachedData<MusicZoneData>();

		public bool PlayingMusic { get; set; } = false;

		public string CurrentTrackName { get; set; } = null;
	}
}

namespace Distance.TrackMusic.Models
{
	public class MusicChoiceEntry
	{
		public string Track { get; set; } = string.Empty;

		public MusicChoiceEntry(string track)
		{
			Track = track;
		}
	}
}

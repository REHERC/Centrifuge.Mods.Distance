using UnityEngine;

namespace Distance.TrackMusic.Editor.Tools
{
	public class AddMusicChoiceAction : AddOrRemoveMusicChoiceAction
	{
		public AddMusicChoiceAction(GameObject obj) : base(obj, null) { }

		public override string Description_ => "Added Music Choice to object";
	}
}
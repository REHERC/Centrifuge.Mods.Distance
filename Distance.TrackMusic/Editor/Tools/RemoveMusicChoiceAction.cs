using UnityEngine;

namespace Distance.TrackMusic.Editor.Tools
{
	public class RemoveMusicChoiceAction : AddOrRemoveMusicChoiceAction
	{
		public RemoveMusicChoiceAction(GameObject obj, ZEventListener c) : base(obj, c) { }

		public override string Description_ => "Removed Music Choice from object";
	}
}

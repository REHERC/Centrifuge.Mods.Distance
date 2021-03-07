using Distance.TrackMusic.Models;
using LevelEditorActions;
using UnityEngine;

namespace Distance.TrackMusic.Editor.Tools
{
	public class AddMusicTrackAction : SimplerAction
	{
		public override string Description_ => "Add Music Track";

		public ReferenceMap.Handle<GameObject> objectHandle;

		public GameObject CreateTrack()
		{
			GameObject gameObject = Resource.LoadPrefabInstance("Group", true);

			gameObject.GetComponent<CustomName>().customName_ = "Music Track";

			var component = gameObject.AddComponent<ZEventListener>();

			var track = new MusicTrack() { Name = "Unknown" };

			track.NewVersion();

			track.WriteObject(component);

			gameObject.ForEachILevelEditorListener((ILevelEditorListener listener) => listener.LevelEditorStart(true));

			foreach (MonoBehaviour monoBehaviour in gameObject.GetComponents<MonoBehaviour>())
			{
				monoBehaviour.enabled = false;
			}

			LevelEditor editor = G.Sys.LevelEditor_;

			editor.AddGameObjectSilent(ref objectHandle, gameObject, null);

			return gameObject;
		}

		public void DestroyTrack()
		{
			GameObject gameObject = objectHandle.Get();
			if (gameObject == null)
			{
				return;
			}
			G.Sys.LevelEditor_.RemoveGameObjectSilent(gameObject);
		}

		public override void Undo()
		{
			DestroyTrack();
		}

		public override void Redo()
		{
			CreateTrack();
		}
	}
}

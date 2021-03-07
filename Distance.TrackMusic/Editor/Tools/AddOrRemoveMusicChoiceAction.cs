#pragma warning disable RCS1079
using Distance.TrackMusic.Models;
using Events.LevelEditor;
using LevelEditorActions;
using System;
using UnityEngine;

namespace Distance.TrackMusic.Editor.Tools
{
	public abstract class AddOrRemoveMusicChoiceAction : SimplerAction
	{
		public override string Description_ => throw new NotImplementedException();

		private ReferenceMap.Handle<GameObject> originalHandle;
		private ReferenceMap.Handle<GameObject> newHandle;
		private ReferenceMap.Handle<ZEventListener> addedComponentHandle;

		private readonly bool isAdd;
		private readonly byte[] componentBytes;

		protected AddOrRemoveMusicChoiceAction(GameObject gameObject, ZEventListener comp)
		{
			ReferenceMap referenceMap_ = G.Sys.LevelEditor_.ReferenceMap_;
			originalHandle = referenceMap_.GetHandleOrNull(gameObject);
			newHandle = referenceMap_.GetHandleOrNull<GameObject>(null);
			addedComponentHandle = referenceMap_.GetHandleOrNull(comp);
			isAdd = (comp == null);
			if (!isAdd)
			{
				componentBytes = Serializers.BinarySerializer.SaveComponentToBytes(comp, null);
			}
		}

		private void AddOrRemove(ReferenceMap.Handle<GameObject> beforeHandle, ref ReferenceMap.Handle<GameObject> afterHandle, bool add)
		{
			LevelEditor editor = G.Sys.LevelEditor_;
			GameObject gameObject = beforeHandle.Get();

			ZEventListener comp = (!add) ? addedComponentHandle.Get() : ((ZEventListener)((object)null));

			if (!gameObject.HasComponent<LevelSettings>())
			{
				editor.RemoveGameObjectSilent(gameObject);
			}

			if (add)
			{
				comp = gameObject.AddComponent<ZEventListener>();

				var choice = new MusicChoice();

				if (gameObject.HasComponent<LevelSettings>())
				{
					choice.Choices.Add("Level", new MusicChoiceEntry(""));
				}

				if (gameObject.HasComponent<MusicTrigger>())
				{
					choice.Choices.Add("Trigger", new MusicChoiceEntry(""));
				}

				if (gameObject.HasComponent<MusicZone>())
				{
					choice.Choices.Add("Zone", new MusicChoiceEntry(""));
				}

				choice.WriteObject(comp);

				if (componentBytes != null)
				{
					Serializers.BinaryDeserializer.LoadComponentContentsFromBytes(comp, null, componentBytes);
				}

				comp.enabled = false;
			}
			else if (comp)
			{
				comp.Destroy();
			}

			if (gameObject.HasComponent<LevelSettings>())
			{
				Mod.Instance.LevelEditor.NeedsRefresh = true;
			}

			if (!gameObject.HasComponent<LevelSettings>())
			{
				editor.AddGameObjectSilent(ref afterHandle, gameObject, editor.WorkingLevel_.GetLayerOfObject(gameObject));
			}

			addedComponentHandle = (!add) ? default : editor.ReferenceMap_.GetHandleOrNull(comp);
			gameObject.ForEachILevelEditorListenerInChildren(listener => listener.OnLevelEditorToolFinish());

			ObjectHadComponentAddedOrRemoved.Broadcast(new ObjectHadComponentAddedOrRemoved.Data(gameObject));

			if (!gameObject.HasComponent<LevelSettings>())
			{
				editor.SelectObject(gameObject);
			}
		}

		public sealed override void Undo()
		{
			AddOrRemove(newHandle, ref originalHandle, !isAdd);
		}

		public sealed override void Redo()
		{
			AddOrRemove(originalHandle, ref newHandle, isAdd);
		}
	}
}

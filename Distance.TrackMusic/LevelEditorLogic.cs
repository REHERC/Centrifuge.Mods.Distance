using UnityEngine;

namespace Distance.TrackMusic
{
    public class LevelEditorLogic : MonoBehaviour
    {
        private Mod mod_;

        public bool NeedsRefresh { get; set; } = false;

        public bool IsWorkingStateLevel { get; set; } = false;

        public void Awake()
        {
            mod_ = GetComponent<Mod>();
        }


        public void AddMusicChoiceSelection()
        {
            EditorTools.AddMusicChoiceTool.Target = G.Sys.LevelEditor_.SelectedObjects_.ToArray();
            G.Sys.LevelEditor_.StartNewToolJobOfType(typeof(EditorTools.AddMusicChoiceTool), false);
        }

        public void AddMusicChoiceLevelSettings()
        {
            EditorTools.AddMusicChoiceTool.Target = new GameObject[] { G.Sys.LevelEditor_.WorkingSettings_.gameObject };
            G.Sys.LevelEditor_.StartNewToolJobOfType(typeof(EditorTools.AddMusicChoiceTool), false);
        }

        public void AddMusicTrack()
        {
            G.Sys.LevelEditor_.StartNewToolJobOfType(typeof(EditorTools.AddMusicTrackTool), false);
        }

        public void ToggleCustomMusic()
        {
            G.Sys.LevelEditor_.StartNewToolJobOfType(typeof(EditorTools.ToggleMusicTool), false);
        }

        public void ResetLevelSettings(LevelSettings __instance)
        {
            Debug.Log("Resetting music");

            foreach (var comp in __instance.gameObject.GetComponents<ZEventListener>())
            {
                DestroyImmediate(comp); // required for when level clear and load happen on the same frame (all the time)
                mod_.variables_.CachedMusicChoice.Remove(comp);
                mod_.variables_.CachedMusicTrack.Remove(comp);
                comp.Destroy();
            }

            __instance.gameObject.RemoveComponents<ZEventListener>();
            mod_.soundPlayer_.Update();
            NeedsRefresh = true;
        }
    }
}

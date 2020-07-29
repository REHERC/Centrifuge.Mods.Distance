using LevelEditorActions;

namespace Distance.TrackMusic.Editor.Tools
{
    public class ToggleMusicAction : SimplerAction
    {
        public override string Description_ => "Toggle Custom Music";

        public void Toggle()
        {
            Mod.Instance.Enabled = !Mod.Instance.Enabled;
            Mod.Instance.soundPlayer_.PlayTrack(Mod.Instance.soundPlayer_.GetMusicChoiceValue(G.Sys.LevelEditor_.WorkingSettings_.gameObject, "Level"), 0f);
        }

        public override void Undo()
        {
            Toggle();
        }

        public override void Redo()
        {
            Toggle();
        }
    }
}

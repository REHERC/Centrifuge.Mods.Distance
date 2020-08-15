using Harmony;

namespace Distance.EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(LevelEditorLevelNameSelectMenuLogic), "GenerateLevelNameList")]
    internal class LevelEditorLevelNameSelectMenuLogic__GenerateLevelNameList
    {
    }
}

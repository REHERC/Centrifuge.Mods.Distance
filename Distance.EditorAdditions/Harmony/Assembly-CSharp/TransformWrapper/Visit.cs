using Centrifuge.Distance.Data;
using Centrifuge.Distance.Game;
using HarmonyLib;
using UnityEngine;

namespace Distance.EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(TransformWrapper), "Visit")]
    internal class TransformWrapper__Visit
    {
        [HarmonyPrefix]
        internal static void Prefix(TransformWrapper __instance, IVisitor visitor)
        {
            if (!(visitor is Serializers.Serializer) && !(visitor is Serializers.Deserializer))
            {
                Transform ObjectTransform = __instance.tComponent_;

                if (!ObjectTransform.IsRoot())
                {
                    visitor.VisitAction("Inspect Parent", () =>
                    {
                        var Editor = G.Sys.LevelEditor_;
                        var Selection = Editor.SelectedObjects_;

                        if (Selection.Count == 1)
                        {
                            EditorUtil.Inspect(ObjectTransform.parent.gameObject);
                        }
                        else
                        {
                            MessageBox.Create("You must select only 1 object to ust this tool.", "ERROR")
                            .SetButtons(MessageButtons.Ok)
                            .Show();
                        }
                    }, null);
                }
            }
        }
    }
}

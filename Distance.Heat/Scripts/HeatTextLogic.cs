using Centrifuge.Distance;
using Distance.Heat.Enums;
using System.Collections;
using UnityEngine;

namespace Distance.Heat.Scripts
{
	public class HeatTextLogic : MonoBehaviour
	{
        internal const float MaximumOpacity = 0.7f;

        internal static HeatTextLogic Instance { get; set; } = null;
        internal static bool creatingInstance = false;

        internal UILabel label;
        internal UIWidget widget;
        internal UIPanel panel;

        internal static void Create(GameObject speedrunTimerLogic = null)
        {
            if (!Instance && !creatingInstance)
            {
                creatingInstance = true;
                GameObject alphaVersionAnchorBlueprint = null;

                if (speedrunTimerLogic)
                {
                    alphaVersionAnchorBlueprint = speedrunTimerLogic.Parent();
                }
                else
                {
                    alphaVersionAnchorBlueprint = GameObject.Find("Anchor : Speedrun Timer");
                }

                if (alphaVersionAnchorBlueprint)
                {
                    GameObject centrifugeInfoAnchor = Instantiate(alphaVersionAnchorBlueprint, alphaVersionAnchorBlueprint.transform.parent);

                    centrifugeInfoAnchor.SetActive(true);
                    centrifugeInfoAnchor.name = "Anchor : Heat Display";

                    centrifugeInfoAnchor.ForEachChildObjectDepthFirstRecursive((obj) =>
                    {
                        obj.SetActive(true);
                        obj.RemoveComponents<SpeedrunTimerLogic>();
                    });

                    UILabel label = centrifugeInfoAnchor.GetComponentInChildren<UILabel>();

                    Instance = label.gameObject.AddComponent<HeatTextLogic>();
                }

                creatingInstance = false;
            }
        }

        internal void Start()
        {
            GameObject anchorObject = gameObject.Parent();
            GameObject panelObject = anchorObject.Parent();

            label = GetComponent<UILabel>();
            widget = anchorObject.GetComponent<UIWidget>();
            panel = panelObject.GetComponent<UIPanel>();

            widget.alpha = 0;

            AdjustPosition();
        }

        internal void Update()
        {
            label.text = Mod.Instance.Text;

            Visible = CanDisplay;
        }

        private Coroutine _transitionCoroutine = null;

        private bool _visible = true;
        internal bool Visible
        {
            get => _visible;
            set
            {
                if (value != _visible)
                {
                    if (_transitionCoroutine != null)
                    {
                        StopCoroutine(_transitionCoroutine);
                    }

                    _transitionCoroutine = StartCoroutine(Transition(value));
                }

                _visible = value;
            }
        }

        internal bool CanDisplay => Mod.Instance.DisplayCondition && Flags.CanDisplayHudElements && Mod.Instance.Config.DisplayMode == DisplayMode.Watermark;

        internal void AdjustPosition()
        {
            label.SetAnchor(panel.gameObject, 21, 19, -19, -17);
            label.pivot = UIWidget.Pivot.TopLeft;
        }

        internal IEnumerator Transition(bool visible, float duration = 0.2f)
        {
            float target = visible ? MaximumOpacity : 0.0f;
            float current = widget.alpha;

            for (float time = 0.0f; time < duration; time += Timex.deltaTime_)
            {
                if (!Centrifuge.Distance.Game.Options.General.MenuAnimations)
                {
                    break;
                }

                float value = MathUtil.Map(time, 0, duration, current, target);
                widget.alpha = value;

                yield return null;
            }

            widget.alpha = target;

            yield break;
        }
    }
}
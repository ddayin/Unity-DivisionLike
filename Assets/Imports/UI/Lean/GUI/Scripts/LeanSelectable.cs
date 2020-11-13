using Selectable = UnityEngine.UI.Selectable;

namespace Lean.Gui
{
	/// <summary>This component provides an alternative to Unity's UI button, allowing you to easily add custom transitions, as well as add an OnDown event.</summary>
	public abstract class LeanSelectable : Selectable
	{
#if UNITY_EDITOR
		protected override void Reset()
		{
			base.Reset();

			transition = Selectable.Transition.None;
		}
#endif
	}
}

#if UNITY_EDITOR
namespace Lean.Gui.Inspector
{
	using UnityEditor;

	public class LeanSelectable_Inspector<T> : Lean.Common.LeanInspector<T>
		where T : LeanSelectable
	{
		protected override void DrawInspector()
		{
			Draw("m_Interactable");
			Draw("m_Transition");

			if (Any(t => t.transition == Selectable.Transition.ColorTint))
			{
				EditorGUI.indentLevel++;
					Draw("m_TargetGraphic");
					Draw("m_Colors");
				EditorGUI.indentLevel--;
			}

			if (Any(t => t.transition == Selectable.Transition.SpriteSwap))
			{
				EditorGUI.indentLevel++;
					Draw("m_TargetGraphic");
					Draw("m_SpriteState");
				EditorGUI.indentLevel--;
			}

			if (Any(t => t.transition == Selectable.Transition.Animation))
			{
				EditorGUI.indentLevel++;
					Draw("m_AnimationTriggers");
				EditorGUI.indentLevel--;
			}

			Draw("m_Navigation");
		}
	}
}
#endif
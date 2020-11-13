using UnityEngine;
using Lean.Common;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Gui
{
	/// <summary>This component will automatically constrain the current <b>RectTransform.anchoredPosition</b> to the specified range.</summary>
	[ExecuteInEditMode]
	[HelpURL(LeanGui.HelpUrlPrefix + "LeanConstrainAnchoredPosition")]
	[AddComponentMenu(LeanGui.ComponentMenuPrefix + "Constrain AnchoredPosition")]
	public class LeanConstrainAnchoredPosition : MonoBehaviour
	{
		/// <summary>Constrain horizontally?</summary>
		public bool Horizontal { set { horizontal = value; } get { return horizontal; } } [SerializeField] private bool horizontal;

		/// <summary>The minimum value in pixels.</summary>
		public float HorizontalPixelMin { set { horizontalPixelMin = value; } get { return horizontalPixelMin; } } [FSA("horizontalMin")] [SerializeField] private float horizontalPixelMin = -100.0f;

		/// <summary>The maximum value in pixels.</summary>
		public float HorizontalPixelMax { set { horizontalPixelMax = value; } get { return horizontalPixelMax; } } [FSA("horizontalMax")] [SerializeField] private float horizontalPixelMax = 100.0f;

		/// <summary>The minimum value in 0..1 percent of the current RectTransform size.</summary>
		public float HorizontalRectMin { set { horizontalRectMin = value; } get { return horizontalRectMin; } } [SerializeField] private float horizontalRectMin;

		/// <summary>The maximum value in 0..1 percent of the current RectTransform size.</summary>
		public float HorizontalRectMax { set { horizontalRectMax = value; } get { return horizontalRectMax; } } [SerializeField] private float horizontalRectMax;

		/// <summary>The minimum value in 0..1 percent of the parent RectTransform size.</summary>
		public float HorizontalParentMin { set { horizontalParentMin = value; } get { return horizontalParentMin; } } [SerializeField] private float horizontalParentMin;

		/// <summary>The maximum value in 0..1 percent of the parent RectTransform size.</summary>
		public float HorizontalParentMax { set { horizontalParentMax = value; } get { return horizontalParentMax; } } [SerializeField] private float horizontalParentMax;

		/// <summary>Constrain vertically?</summary>
		public bool Vertical { set { vertical = value; } get { return vertical; } } [SerializeField] private bool vertical;

		/// <summary>The minimum value in pixels.</summary>
		public float VerticalPixelMin { set { verticalPixelMin = value; } get { return verticalPixelMin; } } [FSA("verticalMin")] [SerializeField] private float verticalPixelMin = -100.0f;

		/// <summary>The maximum value in pixels.</summary>
		public float VerticalPixelMax { set { verticalPixelMax = value; } get { return verticalPixelMax; } } [FSA("verticalMax")] [SerializeField] private float verticalPixelMax = 100.0f;

		/// <summary>The minimum value in 0..1 percent of the current RectTransform size.</summary>
		public float VerticalRectMin { set { verticalRectMin = value; } get { return verticalRectMin; } } [SerializeField] private float verticalRectMin;

		/// <summary>The maximum value in 0..1 percent of the current RectTransform size.</summary>
		public float VerticalRectMax { set { verticalRectMax = value; } get { return verticalRectMax; } } [SerializeField] private float verticalRectMax;

		/// <summary>The minimum value in 0..1 percent of the parent RectTransform size.</summary>
		public float VerticalParentMin { set { verticalParentMin = value; } get { return verticalParentMin; } } [SerializeField] private float verticalParentMin;

		/// <summary>The maximum value in 0..1 percent of the parent RectTransform size.</summary>
		public float VerticalParentMax { set { verticalParentMax = value; } get { return verticalParentMax; } } [SerializeField] private float verticalParentMax;

		[System.NonSerialized]
		private RectTransform cachedRectTransform;

		protected virtual void OnEnable()
		{
			cachedRectTransform = GetComponent<RectTransform>();
		}

		protected virtual void LateUpdate()
		{
			var anchoredPosition = cachedRectTransform.anchoredPosition;
			var rect             = cachedRectTransform.rect;
			var parentSize       = ParentSize;

			if (horizontal == true)
			{
				var min = horizontalPixelMin + horizontalRectMin * rect.width + horizontalParentMin * parentSize.x;
				var max = horizontalPixelMax + horizontalRectMax * rect.width + horizontalParentMax * parentSize.x;

				anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, min, max);
			}

			if (vertical == true)
			{
				var min = verticalPixelMin + verticalRectMin * rect.height + verticalParentMin * parentSize.x;
				var max = verticalPixelMax + verticalRectMax * rect.height + verticalParentMax * parentSize.x;

				anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, min, max);
			}

			cachedRectTransform.anchoredPosition = anchoredPosition;
		}

		private Vector2 ParentSize
		{
			get
			{
				var parent = cachedRectTransform.parent as RectTransform;

				return parent != null ? parent.rect.size : Vector2.zero;
			}
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Gui.Inspector
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanConstrainAnchoredPosition))]
	public class LeanConstrainAnchoredPosition_Inspector : LeanInspector<LeanConstrainAnchoredPosition>
	{
		protected override void DrawInspector()
		{
			Draw("horizontal", "Constrain horizontally?");

			if (Any(t => t.Horizontal == true))
			{
				EditorGUI.indentLevel++;
					Draw("horizontalPixelMin", "The minimum value in pixels.", "Pixel Min");
					Draw("horizontalPixelMax", "The maximum value in pixels.", "Pixel Max");
					Draw("horizontalRectMin", "The minimum value in 0..1 percent of the current RectTransform size.", "Rect Min");
					Draw("horizontalRectMax", "The maximum value in 0..1 percent of the current RectTransform size.", "Rect Max");
					Draw("horizontalParentMin", "The maximum value in 0..1 percent of the parent RectTransform size.", "Parent Min");
					Draw("horizontalParentMax", "The maximum value in 0..1 percent of the parent RectTransform size.", "Parent Min");
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Separator();

			Draw("vertical", "Constrain vertically?");

			if (Any(t => t.Vertical == true))
			{
				EditorGUI.indentLevel++;
					Draw("verticalPixelMin", "The minimum value in pixels.", "Pixel Min");
					Draw("verticalPixelMax", "The maximum value in pixels.", "Pixel Max");
					Draw("verticalRectMin", "The maximum value in 0..1 percent of the current RectTransform size.", "Rect Min");
					Draw("verticalRectMax", "The maximum value in 0..1 percent of the current RectTransform size.", "Rect Max");
					Draw("verticalParentMin", "The maximum value in 0..1 percent of the parent RectTransform size.", "Parent Min");
					Draw("verticalParentMax", "The maximum value in 0..1 percent of the parent RectTransform size.", "Parent Min");
				EditorGUI.indentLevel--;
			}
		}
	}
}
#endif
using UnityEngine;
using Lean.Common;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Gui
{
	/// <summary>This component will automatically snap <b>RectTransform.anchoredPosition</b> to the specified interval.</summary>
	[ExecuteInEditMode]
	[HelpURL(LeanGui.HelpUrlPrefix + "LeanSnap")]
	[AddComponentMenu(LeanGui.ComponentMenuPrefix + "Snap")]
	public class LeanSnap : MonoBehaviour
	{
		/// <summary>To prevent UI element dragging from conflicting with snapping, you can specify the drag component here.</summary>
		public LeanDrag DisableWith { set { disableWith = value; } get { return disableWith; } } [SerializeField] private LeanDrag disableWith;

		/// <summary>Snap horizontally?</summary>
		public bool Horizontal { set { horizontal = value; } get { return horizontal; } } [SerializeField] private bool horizontal;

		/// <summary>The snap points will be offset by this many pixels.</summary>
		public float HorizontalOffset { set { horizontalOffset = value; } get { return horizontalOffset; } } [SerializeField] private float horizontalOffset;

		/// <summary>The spacing between each snap point in pixels.</summary>
		public float HorizontalIntervalPixel { set { horizontalIntervalPixel = value; } get { return horizontalIntervalPixel; } } [FSA("horizontalInterval")] [SerializeField] private float horizontalIntervalPixel = 10.0f;

		/// <summary>The spacing between each snap point in 0..1 percent of the current RectTransform size.</summary>
		public float HorizontalIntervalRect { set { horizontalIntervalRect = value; } get { return horizontalIntervalRect; } } [SerializeField] private float horizontalIntervalRect;

		/// <summary>The spacing between each snap point in 0..1 percent of the parent.</summary>
		public float HorizontalIntervalParent { set { horizontalIntervalParent = value; } get { return horizontalIntervalParent; } } [SerializeField] private float horizontalIntervalParent;

		/// <summary>The snap speed.
		/// -1 = Instant.
		/// 1 = Slow.
		/// 10 = Fast.</summary>
		public float HorizontalSpeed { set { horizontalSpeed = value; } get { return horizontalSpeed; } } [SerializeField] private float horizontalSpeed = -1.0f;

		/// <summary>Snap vertically?</summary>
		public bool Vertical { set { vertical = value; } get { return vertical; } } [SerializeField] private bool vertical;

		/// <summary>The snap points will be offset by this many pixels.</summary>
		public float VerticalOffset { set { verticalOffset = value; } get { return verticalOffset; } } [SerializeField] private float verticalOffset;

		/// <summary>The spacing between each snap point in pixels.</summary>
		public float VerticalIntervalPixel { set { verticalIntervalPixel = value; } get { return verticalIntervalPixel; } } [FSA("verticalInterval")] [SerializeField] private float verticalIntervalPixel = 10.0f;

		/// <summary>The spacing between each snap point in 0..1 percent of the current RectTransform size.</summary>
		public float VerticalIntervalRect { set { verticalIntervalRect = value; } get { return verticalIntervalRect; } } [SerializeField] private float verticalIntervalRect;

		/// <summary>The spacing between each snap point in 0..1 percent of the parent.</summary>
		public float VerticalIntervalParent { set { verticalIntervalParent = value; } get { return verticalIntervalParent; } } [SerializeField] private float verticalIntervalParent;

		/// <summary>The snap speed.
		/// -1 = Instant.
		/// 1 = Slow.
		/// 10 = Fast.</summary>
		public float VerticalSpeed { set { verticalSpeed = value; } get { return verticalSpeed; } } [SerializeField] private float verticalSpeed = -1.0f;

		[System.NonSerialized]
		private RectTransform cachedRectTransform;

		protected virtual void OnEnable()
		{
			cachedRectTransform = GetComponent<RectTransform>();
		}

		protected virtual void LateUpdate()
		{
			if (disableWith != null && disableWith.Dragging == true)
			{
				return;
			}

			var anchoredPosition = cachedRectTransform.anchoredPosition;
			var rect             = cachedRectTransform.rect;
			var parentSize       = ParentSize;
			var intervalX        = horizontalIntervalPixel + horizontalIntervalRect * rect.width + horizontalIntervalParent * parentSize.x;
			var intervalY        =   verticalIntervalPixel +   verticalIntervalRect * rect.width +   verticalIntervalParent * parentSize.y;

			if (horizontal == true && intervalX != 0.0f)
			{
				var target = Mathf.Round((anchoredPosition.x - horizontalOffset) / intervalX) * intervalX + horizontalOffset;
				var factor = LeanHelper.DampenFactor(horizontalSpeed, Time.deltaTime);

				anchoredPosition.x = Mathf.Lerp(anchoredPosition.x, target, factor);
			}

			if (vertical == true && intervalY != 0.0f)
			{
				var target = Mathf.Round((anchoredPosition.y - verticalOffset) / intervalY) * intervalY + verticalOffset;
				var factor = LeanHelper.DampenFactor(verticalSpeed, Time.deltaTime);

				anchoredPosition.y = Mathf.Lerp(anchoredPosition.y, target, factor);
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
	[CustomEditor(typeof(LeanSnap))]
	public class LeanSnap_Inspector : LeanInspector<LeanSnap>
	{
		protected override void DrawInspector()
		{
			Draw("horizontal", "Snap horizontally?");

			if (Any(t => t.Horizontal == true))
			{
				EditorGUI.indentLevel++;
					Draw("horizontalOffset", "The snap points will be offset by this many pixels.", "Offset");
					BeginError(Any(t => t.HorizontalIntervalPixel == 0.0f && t.HorizontalIntervalRect == 0.0f && t.HorizontalIntervalParent == 0.0f));
						Draw("horizontalIntervalPixel", "The spacing between each snap point in pixels.", "Interval Pixel");
						Draw("horizontalIntervalRect", "The spacing between each snap point in 0..1 percent of the current RectTransform size.", "Interval Rect");
						Draw("horizontalIntervalParent", "The spacing between each snap point in 0..1 percent of the parent.", "Interval Parent");
					EndError();
					Draw("horizontalSpeed", "The snap speed.\n\n-1 = Instant.\n\n1 = Slow.\n\n10 = Fast.", "Speed");
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Separator();

			Draw("vertical", "Snap vertically?");

			if (Any(t => t.Vertical == true))
			{
				EditorGUI.indentLevel++;
					Draw("verticalOffset", "The snap points will be offset by this many pixels.", "Offset");
					BeginError(Any(t => t.VerticalIntervalPixel == 0.0f && t.VerticalIntervalRect == 0.0f && t.VerticalIntervalParent == 0.0f));
						Draw("verticalIntervalPixel", "The spacing between each snap point in pixels.", "Interval Pixel");
						Draw("verticalIntervalRect", "The spacing between each snap point in 0..1 percent of the current RectTransform size.", "Interval Rect");
						Draw("verticalIntervalParent", "The spacing between each snap point in 0..1 percent of the parent.", "Interval Parent");
					EndError();
					Draw("verticalSpeed", "The snap speed.\n\n-1 = Instant.\n\n1 = Slow.\n\n10 = Fast.", "Speed");
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Separator();

			Draw("disableWith", "To prevent UI element dragging from conflicting with snapping, you can specify the drag component here.");
		}
	}
}
#endif
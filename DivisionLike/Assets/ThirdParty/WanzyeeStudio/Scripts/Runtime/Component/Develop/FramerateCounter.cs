
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEngine;
using System;

namespace WanzyeeStudio{
	
	/// <summary>
	/// Calculate FPS at runtime, optional to show on GUI with color warning.
	/// </summary>
	/*
	 * Multiple framerate counter is fine actually, but no need.
	 * Singleton is just for accessing instance easily.
	 */
	[HelpURL("https://git.io/viqRq")]
	[DisallowMultipleComponent]
	public class FramerateCounter : BaseSingleton<FramerateCounter>{

		#region Static

		/// <summary>
		/// Current frame rate per second, updated every time interval set on instance.
		/// </summary>
		/// <value>The FPS.</value>
		public static float fps{
			get{ return CheckValid(true) ? instance._framerate : 0f; }
		}

		/// <summary>
		/// Flag to show FPS on screen GUI or not.
		/// </summary>
		/// <value><c>true</c> if show; otherwise, <c>false</c>.</value>
		public static bool show{
			get{ return CheckValid(false) && instance.showHud; }
			set{ if(CheckValid(value)) instance.showHud = value; }
		}

		/// <summary>
		/// Check if the instance is active and enabled to work.
		/// Set it enabled silently if disabled, or log error if gameObject inactived.
		/// </summary>
		/// 
		/// <remarks>
		/// We could re-enable safely, but not re-active the gameObject 'coz:
		/// 	1. Active current gameObject might be useless if hierarchy inactive.
		/// 	2. Active hierarchy arbitrarily might damage user's complex scene setting.
		/// </remarks>
		/// 
		/// <returns><c>true</c>, if valid, <c>false</c> otherwise.</returns>
		/// <param name="must">If set to <c>true</c> must.</param>
		/// 
		private static bool CheckValid(bool must){

			if(!instance.enabled) instance.enabled = true;
			if(instance.gameObject.activeInHierarchy) return true;

			if(must) Debug.LogError("FramerateCounter can't work with GameObject inactive in hierarchy.", instance);
			return false;

		}

		#endregion


		#region Public Fields

		/// <summary>
		/// The interval to calculate FPS.
		/// </summary>
		[Tooltip("Interval to calculate.")]
		public float interval = 1f;

		/// <summary>
		/// Flag if show FPS on GUI.
		/// </summary>
		[Tooltip("Show FPS on GUI or not.")]
		public bool showHud;

		/// <summary>
		/// The alignment of GUI on screen.
		/// </summary>
		[Tooltip("Align GUI on screen.")]
		public TextAnchor alignment = TextAnchor.UpperRight;

		/// <summary>
		/// The size of the font.
		/// </summary>
		[Tooltip("Font size of GUI text.")]
		public int fontSize = 20;

		/// <summary>
		/// The gradient of the font color, map with frame range for warning.
		/// </summary>
		[Tooltip("Color of GUI text, gradient maps FPS to warn.")]
		public Gradient fontColor = new Gradient(){
			colorKeys = new []{
				new GradientColorKey(Color.red, 0f),
				new GradientColorKey(Color.yellow, 0.5f),
				new GradientColorKey(Color.green, 1f)
			}
		};
		
		/// <summary>
		/// The frame range, used to calculate the font color of current FPS.
		/// </summary>
		[Tooltip("Map current FPS to font color gradient.")]
		public float frameRange = 80f;

		/// <summary>
		/// The color of the GUI label background.
		/// </summary>
		[Tooltip("Background color of GUI label.")]
		public Color backgroundColor = new Color(0f, 0f, 0f, 0.5f);

		#endregion


		#region Private Fields

		/// <summary>
		/// The real time of counting begining.
		/// </summary>
		private float _time;

		/// <summary>
		/// The frame count of counting begining.
		/// </summary>
		private float _count;

		/// <summary>
		/// Current frame rate per second.
		/// Calculated every time interval.
		/// </summary>
		private float _framerate;

		/// <summary>
		/// The GUI content with FPS text to show.
		/// </summary>
		private GUIContent _content;

		/// <summary>
		/// The GUI style for showing content text.
		/// </summary>
		private GUIStyle _style;

		#endregion


		#region Methods

		/// <summary>
		/// Awake, initialize GUI content and style.
		/// </summary>
		protected override void Awake(){

			base.Awake();

			_content = new GUIContent();

			_style = new GUIStyle(GUIStyle.none);
			_style.padding = new RectOffset(7, 7, 2, 2);
			_style.normal.background = Texture2D.whiteTexture;

		}

		/// <summary>
		/// OnEnable, reset counting time and frame.
		/// Also set FPS initial value, since not count yet.
		/// </summary>
		private void OnEnable(){

			_time = Time.realtimeSinceStartup;
			_count = Time.frameCount;

			_framerate = _count / _time;
			_content.text = string.Format("FPS: {0:F1}", _framerate);

		}

		/// <summary>
		/// Update, check time interval to calculate FPS and set GUI content.
		/// Reset counting time and frame for next after current done.
		/// </summary>
		private void Update(){

			var _now = Time.realtimeSinceStartup;

			if(_now - _time >= interval){

				var _frame = Time.frameCount;

				_framerate = (_frame - _count) / (_now - _time);
				_content.text = string.Format("FPS: {0:F1}", _framerate);

				_time = _now;
				_count = _frame;

			}

		}

		/// <summary>
		/// OnGUI, update layout and draw FPS text in need.
		/// </summary>
		private void OnGUI(){

			if(!showHud) return;
			if(EventType.Repaint != Event.current.type) return;

			_style.alignment = alignment;
			_style.fontSize = fontSize;

			if(null != fontColor) _style.normal.textColor = fontColor.Evaluate(_framerate / frameRange);

			var _color = GUI.backgroundColor;
			GUI.backgroundColor = backgroundColor;

			GUI.Label(GetRect(), _content, _style);
			GUI.backgroundColor = _color;

		}

		/// <summary>
		/// Get the GUI rect on screen.
		/// Calculate position by specified alignment and size by GUI content.
		/// </summary>
		/// <returns>The GUI rect.</returns>
		private Rect GetRect(){

			var _size = _style.CalcSize(_content);
			var _result = new Rect(Vector2.zero, _size);

			var _horizontal = (int)alignment % 3;
			var _vertical = (int)((float)alignment / 3f);

			if(1 == _horizontal) _result.x = (Screen.width - _size.x) * 0.5f;
			else if(2 == _horizontal) _result.x = Screen.width - _size.x;

			if(1 == _vertical) _result.y = (Screen.height - _size.y) * 0.5f;
			else if(2 == _vertical) _result.y = Screen.height - _size.y;

			return _result;

		}

		#endregion

	}

}

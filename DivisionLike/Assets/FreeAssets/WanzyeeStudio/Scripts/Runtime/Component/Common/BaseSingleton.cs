
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEngine;
using System;

namespace WanzyeeStudio{
	
	/// <summary>
	/// Base class to implement the singleton pattern for <c>UnityEngine.MonoBehaviour</c>.
	/// </summary>
	/// 
	/// <remarks>
	/// This works with <c>Singleton</c> to minimize coding, if you don't need custom inheritance.
	/// Derive from this to make a <c>UnityEngine.MonoBehaviour</c> singleton.
	/// Only allow the class of current singleton to derive, otherwise throw an exception if in the editor.
	/// This applies the singleton's root <c>Object.DontDestroyOnLoad()</c> and handle duplicates when <c>Awake()</c>.
	/// </remarks>
	/// 
	/// <example>
	/// Example to implement singleton, just derive from this:
	/// </example>
	/// 
	/// <code>
	/// using WanzyeeStudio;
	/// public class SomeComp : BaseSingleton<SomeComp>{}
	/// </code>
	/// 
	/// <example>
	/// Example to access from another class:
	/// </example>
	/// 
	/// <code>
	/// public class AnotherClass{
	/// 	private void DoSomething(){
	/// 		Debug.Log(SomeComp.instance);
	/// 	}
	/// }
	/// </code>
	/// 
	public abstract class BaseSingleton<T> : MonoBehaviour where T : BaseSingleton<T>, new(){

		#region Runtime
		
		/// <summary>
		/// Flag if to destroy excess instance automatically when it <c>Awake()</c>.
		/// </summary>
		protected static bool autoDestroy;

		/// <summary>
		/// Get the singleton instance.
		/// </summary>
		/// <value>The instance.</value>
		public static T instance{
			get{ return Singleton<T>.instance; }
		}

		/// <summary>
		/// Awake, check and handle duplicated instances.
		/// </summary>
		/// 
		/// <remarks>
		/// Apply <c>Object.DontDestroyOnLoad()</c> to the root if this's the singleton.
		/// Otherwise check if <c>autoDestroy</c> to suicide or log error.
		/// Call <c>base.Awake()</c> if you implement <c>Awake()</c> in the subclass.
		/// </remarks>
		/*
		 * Can't place in constructor, to ensure only work in play mode main thread.
		 */
		protected virtual void Awake(){

			if(this == instance) DontDestroyOnLoad(transform.root.gameObject);

			else if(autoDestroy) Destroy(this);

			else Debug.LogErrorFormat(this, "Singleton<{0}> has excess instance {1}.", typeof(T), name);

		}

		#endregion


		#if UNITY_EDITOR

		/// <summary>
		/// Constructor to check inheritance, since we can't constraint subclass.
		/// </summary>
		protected BaseSingleton(){

			if(GetType() == typeof(T)) return;

			var _format = "BaseSingleton<{0}> can be derived by {0} only, {1} is not allowed.";

			throw new InvalidProgramException(string.Format(_format, typeof(T), GetType()));

		}

		#endif

	}

}

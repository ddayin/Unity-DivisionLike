
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEngine;
using System;
using System.Linq;

using Object = UnityEngine.Object;

namespace WanzyeeStudio{

	/// <summary>
	/// Agent to implement the singleton pattern for <c>UnityEngine.MonoBehaviour</c>.
	/// </summary>
	/// 
	/// <remarks>
	/// Not required to derive from any specific class, since C# inheritance is so precious.
	/// This finds the existed instance and check if multiple, creates one if not found.
	/// Set <c>Object.DontDestroyOnLoad()</c> if the instance is created by this.
	/// The others found, created manually or by scripts, should be maintained by the creator.
	/// It might need to check and destroy duplicated when <c>Awake()</c>.
	/// </remarks>
	/// 
	/// <remarks>
	/// In the general case, singleton should be implemented in the certain class for better enclosing.
	/// And sometimes, we do need to manually create one to edit in the Inspector.
	/// Make an agent for <c>UnityEngine.MonoBehaviour</c> to make it easier in the common case.
	/// This'll remind the user to fix with exceptions when the code or scene setup incorrectly in edit mode only.
	/// And pop up a dialog if tries to create in edit mode, to avoid making scene dirty silently.
	/// </remarks>
	/// 
	/// <remarks>
	/// Since we can't protect the constructor without inheritance.
	/// For the purpose of better enclosed programming, here's usage limitations:
	/// 	1. Only allow the class of current singleton to access.
	/// 	2. Only allow one method to access to keep the code clean.
	/// 	3. Don't assign to a delegate, otherwise we can't keep the same accessor.
	/// 	4. Don't access from any constructor or assign to a field, that makes out-of-date.
	/// </remarks>
	/// 
	/// <example>
	/// Example to wrap to access, call this in a method or property:
	/// </example>
	/// 
	/// <code>
	/// using WanzyeeStudio;
	/// public class SomeComp : MonoBehaviour{
	/// 	public static SomeComp instance{
	/// 		get{ return Singleton<SomeComp>.instance; }
	/// 	}
	/// }
	/// </code>
	/// 
	/// <example>
	/// Example to maintain in <c>Awake()</c> to avoid duplicated, check to keep or destroy:
	/// </example>
	/// 
	/// <code>
	/// private void Awake(){
	/// 	if(this != instance) Destroy(this);
	/// 	else DontDestroyOnLoad(gameObject);
	/// }
	/// </code>
	/// 
	public static class Singleton<T> where T : MonoBehaviour, new(){

		#region Fields and Properties

		/// <summary>
		/// The instance stored.
		/// </summary>
		private static T _instance;
		
		/// <summary>
		/// Get the singleton instance.
		/// </summary>
		/// <value>The instance.</value>
		/*
		 * Make this entry as property to avoid assigning to delegate.
		 */
		public static T instance{
			
			get{
				
				#if UNITY_EDITOR
				CheckAccessor(new System.Diagnostics.StackFrame(1).GetMethod());
				#endif

				if(null == _instance) FindInstance();
				return _instance;

			}

		}

		#endregion
		
		
		#region Methods

		/// <summary>
		/// Find the instance in current scene, create one if none.
		/// </summary>
		/*
		 * A prefab component is HideFlags.HideInHierarchy.
		 * An editor hidden scene component is HideFlags.HideAndDontSave, which includes HideFlags.DontSaveInBuild
		 */
		private static void FindInstance(){

			var _flag = HideFlags.HideInHierarchy | HideFlags.DontSaveInBuild;
			var _all = Resources.FindObjectsOfTypeAll<T>().Where((obj) => 0 == (obj.hideFlags & _flag)).ToArray();

			var _format = "Singleton<{0}> found: {1}.";
			foreach(var _found in _all) Debug.LogFormat(_found.gameObject, _format, typeof(T).Name, _found);

			if(0 == _all.Length) CreateInstance();
			else if(1 == _all.Length) _instance = _all[0];
			else throw new OperationCanceledException("Ambiguous between multiple instances.");

		}

		/// <summary>
		/// Create the new instance with <c>Object.DontDestroyOnLoad()</c>.
		/// Show dialog to ask user to create only in edit mode.
		/// </summary>
		/*
		 * Assign instance in methods instead of returning, to wrap creating between switching active.
		 * To avoid the instance property being called when Awake() before assigning.
		 */
		private static void CreateInstance(){

			var _name = string.Format("[Singleton {0}]", typeof(T).Name);

			#if UNITY_EDITOR
			var _object = Application.isPlaying ? CreatePlayObject(_name) : CreateEditObject(_name);
			#else
			var _object = CreatePlayObject(_name);
			#endif

			if(null == _object) return;
			Debug.LogFormat(_object, "Singleton<{0}> created.", typeof(T).Name);

			_object.SetActive(false);
			_instance = _object.AddComponent<T>();
			_object.SetActive(true);
			
		}

		/// <summary>
		/// Create an <c>UnityEngine.GameObject</c> for play mode and apply <c>Object.DontDestroyOnLoad()</c>.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="name">Name.</param>
		private static GameObject CreatePlayObject(string name){
			
			var _result = new GameObject(name);
			Object.DontDestroyOnLoad(_result);

			return _result;

		}
		
		#endregion


		#if UNITY_EDITOR

		/// <summary>
		/// The method accesses and owns this.
		/// Only allow one method to access one singleton of type.
		/// </summary>
		private static System.Reflection.MethodBase _accessor;

		/// <summary>
		/// Check if the type and the accessor valid.
		/// The component type T can't be generic, it's not creatable.
		/// And only allow "one member method" of the "class of current singleton" to access.
		/// </summary>
		/// <param name="accessor">Accessor.</param>
		private static void CheckAccessor(System.Reflection.MethodBase accessor){

			if(accessor == _accessor) return;

			if(typeof(T).IsGenericType){
				var _format = "Singleton doesn't support generic type {0}.";
				throw new NotSupportedException(string.Format(_format, typeof(T).Name));
			}

			if(!accessor.DeclaringType.IsAssignableFrom(typeof(T))){
				var _format = "Singleton<{0}> can be called by {0} only, {1} isn't allowed.";
				throw new MethodAccessException(string.Format(_format, typeof(T).Name, accessor.DeclaringType));
			}

			if(accessor.IsConstructor){
				var _format = "Singleton<{0}> can't be called by constructor nor assigned to a field.";
				throw new MethodAccessException(string.Format(_format, typeof(T).Name));
			}

			if(null != _accessor){
				var _format = "Singleton<{0}> has been called by [{1}], [{2}] isn't allowed.";
				throw new MethodAccessException(string.Format(_format, typeof(T).Name, _accessor, accessor));
			}

			_accessor = accessor;

		}

		/// <summary>
		/// Create an <c>UnityEngine.GameObject</c> for edit mode with a dialog and register undo.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="name">Name.</param>
		private static GameObject CreateEditObject(string name){

			var _title = "Create Singleton";
			var _message = string.Format("Instance of {0} is not found! Create now?", typeof(T).Name);

			if(!UnityEditor.EditorUtility.DisplayDialog(_title, _message, "Create", "Ignore")) return null;

			var _result = new GameObject(name);
			UnityEditor.Undo.RegisterCreatedObjectUndo(_result, _title);

			return _result;

		}

		#endif

	}

}

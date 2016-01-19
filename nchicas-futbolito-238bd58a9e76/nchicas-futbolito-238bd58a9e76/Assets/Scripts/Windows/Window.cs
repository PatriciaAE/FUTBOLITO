using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// Window template format.
/// </summary>
public abstract class Window<T> : MonoBehaviour where T : MonoBehaviour {
	/// <summary>
	/// The instance of the Window<T>.
	/// </summary>
	private static Window<T> _instance;
	/// <summary>
	/// The instance of the class.
	/// </summary>
	public static T Instance;
	/// <summary>
	/// Check if the window is open.
	/// </summary>
	protected static bool isRunning = false;
	/// <summary>
	/// Gets a value indicating whether this <see cref="Window`1"/> is open.
	/// </summary>
	/// <value>
	/// <c>true</c> if is open; otherwise, <c>false</c>.
	/// </value>
	public static bool isOpen{
		get{
			return isRunning;
		}
	}
	[SerializeField]
	private string windowLayer ;
	[SerializeField]
	private string invisibleLayer = "Invisible";
	/// <summary>
	/// Occurs when the windows close.
	/// </summary>
	public Action CallBack;	
	/// <summary>
	/// Function for instanciate the window
	/// </summary>
	/// <returns>
	/// The create class reference for the window.
	/// </returns>
	/// <param name='WindowName'>
	/// Window name for load from prefabs "Windows" folder.
	/// </param>
	protected  static T _Create (string WindowName){
		GameObject go = Instantiate(Resources.Load("Windows/"+WindowName)) as GameObject;
		go.transform.localPosition = Vector3.zero;
		_instance = (Window<T>)go.GetComponent(typeof(T).ToString());
		go.SetLayerRecursively( LayerMask.NameToLayer( _instance.invisibleLayer));
		T classTarget = (T)go.GetComponent(typeof(T).ToString());
		isRunning = true;
		return classTarget;
	}
	/// <summary>
	/// Show the specified window.
	/// </summary>
	/// <param name='WindowName'>
	/// Window name.
	/// </param>
	protected static void _Show( string WindowName){
		if(!isRunning){
			Instance = _Create(WindowName);
		}
	}
	/// <summary>
	/// Start this instance.
	/// </summary>
	public virtual void WindowPlay(){}
	/// <summary>
	/// Pause this instance.
	/// </summary>
	public virtual void WindowPause(){}
	/// <summary>
	/// Initialize this instance.
	/// </summary>
	protected virtual void Initialize(){}
	/// <summary>
	/// Before Close this instance.
	/// </summary>
	protected virtual void BeforeClose(){}
	/// <summary>
	/// Close this instance.
	/// </summary>
	public static void Close(){
		if(isRunning){
			//Call the BeforeClose
			_instance.BeforeClose();
			if(_instance.CallBack != null) {
				_instance.CallBack();
				Debug.LogError("xD");
			}
			isRunning = false;
			_instance.CallBack = null;
			Destroy(_instance.gameObject);
			_instance = null;
			Instance = null;
		}
	}
	
	/// <summary>
	/// Used for the trasition of the animation
	/// </summary>
	IEnumerator Start () {
		yield return new WaitForSeconds( 0.05f );
		gameObject.SetLayerRecursively(LayerMask.NameToLayer( this.windowLayer));
	}

	void OnDestroy(){
		StopAllCoroutines();
		isRunning =false;
		Instance = null;
		_instance = null;
	}
}

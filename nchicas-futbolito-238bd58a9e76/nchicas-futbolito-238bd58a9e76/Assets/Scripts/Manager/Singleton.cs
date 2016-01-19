using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component {

	private static T instance;
	
	public static T Instance {
		get {
			if( instance == null ) {
				GameObject go = Instantiate(  
					Resources.Load( typeof(T).ToString() )
				) as GameObject;
				instance = go.GetComponent<T>();
			}
			
			return instance;
		}
	}
	
	protected virtual void Awake () {
		DontDestroyOnLoad( gameObject );
	}

	protected virtual void Start () {
		if( FindObjectsOfType( typeof(T) ).Length > 1 ) {
			Debug.LogError( "There is more than 1 instance of " + typeof(T).ToString() );
		}
	}

}

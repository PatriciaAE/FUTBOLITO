using UnityEngine;
using System.Collections;

public class TestTimer : MonoBehaviour {
	[SerializeField]
	private Timer timer;
	// Use this for initialization
	void Start () {

		timer.Setup(300,()=>{
			Debug.Log("Final");
		});
		timer.Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


using UnityEngine;
using System.Collections;
//using UnityEditor;

public class FieldToken : MonoBehaviour {
	[SerializeField]
	private float radius = 0;

	private bool tokenCreated = false;
	void  OnDrawGizmos (){
		
		
		float rad = radius*1.28f/512f;
		
		Vector3 topleft = new Vector3(transform.position.x - rad/2,transform.position.y + rad/2,transform.position.z);
		Vector3 bottomrigth = new Vector3(transform.position.x + rad/2,transform.position.y - rad/2,transform.position.z);
		Vector3 topRigth = new Vector3( bottomrigth.x,topleft.y,topleft.z);
		Vector3 bottomleft = new Vector3(topleft.x,bottomrigth.y,topleft.z);
		
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(topleft,topRigth);
		Gizmos.DrawLine(topRigth,bottomrigth);
		Gizmos.DrawLine(bottomrigth,bottomleft);
		Gizmos.DrawLine(bottomleft,topleft);

		//Handles.Label(this.transform.position,this.name,EditorStyles.miniButton);

	}

	public void CreatePlayerToken ( GameObject gameObject, float sizeMultiplier){
		if( !this.tokenCreated){
			GameObject go = Instantiate(gameObject) as GameObject;
			Transform t = go.transform;
			t.parent = transform;
			t.localScale = sizeMultiplier*Vector3.one;
			t.localPosition = Vector3.one;
			this.tokenCreated = true;
		} 
	}
}

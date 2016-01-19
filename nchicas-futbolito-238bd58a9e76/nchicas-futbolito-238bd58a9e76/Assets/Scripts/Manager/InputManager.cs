using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	[SerializeField]
	private LayerMask ballMask;
	[SerializeField]
	private Ball ball;
	[SerializeField]
	private Camera worldCamera;
	
	private Vector3 final;
	[SerializeField]
	private float forceMultiplayer = 10;
	private bool selected = false;
	[SerializeField]
	private bool isLockTouch = false;
	// Update is called once per frame
	void Update () {
		if(Game.Instance.ActualPlayer is LocalPlayer){
			if(!isLockTouch) CheckMouseInput();
		}else if(this.selected){
			this.selected = false;
			ball.HideArrow();
		}
	}
	private static InputManager instance;
	public static InputManager Instance{
		get{
			return instance;
		}
	}
	void Awake(){
		if(instance == null) instance = this;
	}
	private void CheckMouseInput(){
		if(Input.GetMouseButtonDown(0)){

			this.selected = CheckBall( Input.mousePosition);

			if(selected) ball.ShowArrow();
		
		}else if (this.selected && Input.GetMouseButton(0)){

			this.final = worldCamera.ScreenToWorldPoint(Input.mousePosition); 
			ball.UpdateArrow(final);
		
		}else if (this.selected && Input.GetMouseButtonUp(0)){
		
			this.selected = false;
			this.Lock();
			ball.Shoot();
			ball.HideArrow();
		
		}
	}

	public void Lock(){
		this.isLockTouch = true;
	}

	public void Unlock(){
		this.isLockTouch = false;
	}

	private bool CheckBall(Vector3 inputPosition){
	//	Vector3 worldPosition = worldCamera.ScreenToWorldPoint( inputPosition);
		Vector2 world2DPosition = worldCamera.ScreenToWorldPoint( inputPosition);
		Collider2D[] colliders = Physics2D.OverlapPointAll(world2DPosition,ballMask.value);
		bool finded = false;
		if(colliders.Length > 0){
			foreach(var collider in colliders){
				if(collider.tag == "Ball"){
					finded = true;
				}
			}
		}
		return finded;
	}
}

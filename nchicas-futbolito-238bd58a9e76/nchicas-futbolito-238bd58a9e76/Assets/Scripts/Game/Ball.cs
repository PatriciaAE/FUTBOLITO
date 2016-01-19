using UnityEngine;
using System.Collections;
using System;

 public class Ball : MonoBehaviour {
	[SerializeField]
	private GameObject arrow;

	public Vector3 arrowPosition;

	[SerializeField]
	private GameObject arrowContainer;
	[SerializeField]
	private UISprite arrowIndicator;
	[SerializeField]
	private float maxMagnitud;
	[SerializeField]
	private AudioClip shootAudio;

	public Action onBallStop;

	private Vector3 force;
	[SerializeField]
	private float forceMultiplier;

	[SerializeField]
	private AudioClip playerTouchAudio;

	public float ForceMultiplayer{
		get{
			return forceMultiplier;
		}
	}

	public float MaxMagnitud{
		get{
			return maxMagnitud;
		}
	}

	public void Shoot(){

		this.force = new Vector3( (float)Math.Round( this.force.x,4) ,(float) Math.Round( this.force.y,4) , (float)Math.Round( this.force.z,4));

		float magnitud  = this.force.magnitude;
		if(magnitud > this.maxMagnitud) magnitud = maxMagnitud;
		this.GetComponent<Rigidbody2D>().AddForce(this.forceMultiplier*magnitud*this.force.normalized);
		Game.Instance.ActualPlayer.Shooted();
		if(Game.Instance.isMultiplayer){
			//MultiplayerManager.Instance.SendShoot(this.force,this.transform.localPosition);
		}
		Debug.Log(this.force);
		AudioManager.Play(this.shootAudio,AudioType.SFX);
		StartCoroutine("WaitToStop");
	}

	public void Shoot(Vector3 force){
		this.force = force;
		Shoot();
	}

	public void RemoteShoot(Vector3 force , Vector3 position){
		this.transform.localPosition = position;
		float magnitud  = force.magnitude;
		if(magnitud > this.maxMagnitud) magnitud = maxMagnitud;
		this.GetComponent<Rigidbody2D>().AddForce(this.forceMultiplier*magnitud*force.normalized);
		AudioManager.Play(this.shootAudio,AudioType.SFX);
		StartCoroutine("WaitToStop");
	}

	public void SetPosition(Vector2 position){
		this.transform.localPosition = position;
	}

	public void ShowArrow(){
		arrowContainer.SetActive(true);
	}

	public void HideArrow(){
		arrowContainer.SetActive(false);
	}

	public void UpdateArrow(Vector3 final){
		Vector3 ballPosition = transform.position;
		this.force = ballPosition-  final;

		float angle = Vector2.Angle(Vector2.up, this.force);
		if(this.force.x > 0) angle *= -1;
		arrowContainer.transform.rotation = Quaternion.AngleAxis( angle ,Vector3.forward);

		float magnitud = this.force.magnitude;
		if(magnitud > this.maxMagnitud) magnitud = maxMagnitud;
		//arrowIndicator.fillAmount = magnitud/ maxMagnitud;
		arrowIndicator.fillAmount = 0.4f + (magnitud/ maxMagnitud) * 0.5f;

		arrowPosition = arrow.transform.localPosition;
		arrowPosition.y = 40.0f + (magnitud/ maxMagnitud) * 27.0f;
		arrow.transform.localPosition = arrowPosition;
	}

	public void OnCollisionEnter2D(Collision2D collision){
		if(collision.gameObject.tag == "Player"){
			AudioManager.Play(this.playerTouchAudio,AudioType.SFX);
		}
	}
	IEnumerator WaitToStop(){
		yield return new WaitForSeconds(1f);
		while(this.GetComponent<Rigidbody2D>().velocity.magnitude > 0.001f){
			yield return null;
		}
		Debug.Log("Finish");
		if(this.onBallStop != null) {
			this.onBallStop();
			Debug.Log("OnBallStop");
		}
	}
}

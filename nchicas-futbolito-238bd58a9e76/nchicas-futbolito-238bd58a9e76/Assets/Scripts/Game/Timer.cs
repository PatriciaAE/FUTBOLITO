using UnityEngine;
using System.Collections;
using System;

public class Timer : MonoBehaviour {
	
		public enum State{
			none,
			paused,
			playing,
			ended,
		}
		
		#region protected_variables
		protected State timerState;
		protected float time;
		protected float actualTime;
		protected Action onGameFinish;
		
		[SerializeField]
		private bool reverse = false;
		#endregion
		
		#region properties
		public float ActualTime{
			get{
				return actualTime;
			}
		}
		public State TimerState{
			get{
				return timerState;
			}
		}
		#endregion
		
		public Timer (){}
		/// <summary>
		/// Setups the initial time.
		/// </summary>
		/// <param name="time">Time.</param>
		/// <param name="onGameFinish">On game finish.</param>
		public void Setup( float time , Action onGameFinish){
			Clear();
			
			//Set up new Values
			this.time = time;
			this.onGameFinish+= onGameFinish;
		}
		
		public void Clear(){
			this.onGameFinish = null;
			actualTime = 0;
		}
		public void Hide(){
			gameObject.SetActive(false);
		}
		public void Show(){
			gameObject.SetActive(true);
		}
		/// <summary>
		/// Init the timer for first time.
		/// </summary>
		public void Init(){
			actualTime = (this.reverse) ? this.time : 0f; 
			this.Play();
		}
		/// <summary>
		/// Play this instance.
		/// </summary>
		public void Play(){
			if(!gameObject.activeSelf) return; 
			if(timerState != State.playing) {
				this.timerState = State.playing;
				StartCoroutine("TimeTick");
			}
		}
		
		public void Pause(){
			if(this.timerState == State.playing){
				this.timerState = State.paused;
			}
		}
		
		private IEnumerator TimeTick(){
			while( timerState == State.playing){
				if(reverse){
					TimeReverse(Time.deltaTime);
				}else{
					TimeCountUp(Time.deltaTime);
				}
				this.UpdateUI();
				yield return null;
			}
		}
		
		private void TimeReverse(float delta){
			actualTime -= delta;
			if(actualTime <= 0){
				actualTime = 0;
				timerState = State.ended;
				this.onGameFinish();
			}
		}
		
		private void TimeCountUp(float delta){
			actualTime += delta;
			if(actualTime >= time ){
				actualTime = time;
				timerState = State.ended;
				this.onGameFinish();
			}
		}
		
		protected virtual void UpdateUI(){}
		
}


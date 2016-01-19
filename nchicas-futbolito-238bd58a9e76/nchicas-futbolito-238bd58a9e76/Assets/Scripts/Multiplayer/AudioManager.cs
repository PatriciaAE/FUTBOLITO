using UnityEngine;

public enum AudioType {
	Music,
	SFX,
}

public class AudioManager : MonoBehaviour {
	
	private static AudioManager instance;
	[SerializeField]
	private AudioSource audioMusic;
	[SerializeField]
	private AudioSource audioSFX;
	
	void Awake () {
		if( instance == null ) {
			instance = this;
		}
	}

	public static void Play ( AudioClip clip, AudioType type ) {
		if( type == AudioType.SFX ) {
			instance.audioSFX.PlayOneShot( clip, 1f);
		}
		else if( type == AudioType.Music && clip != instance.audioMusic.clip ) {
			instance.audioMusic.loop = true;
			instance.audioMusic.volume = 0.5f;
			instance.audioMusic.clip = clip;
			instance.audioMusic.Play();
		}	
	}
	
	public static bool isMusicPlaying{
		get{
			return instance.audioMusic.isPlaying;
		}
	}
	
	public static void UpdateVolume( float volume ) {
		instance.audioMusic.volume = volume;
	}
}

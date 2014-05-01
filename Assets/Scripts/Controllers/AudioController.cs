using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour {
	public AudioClip interfaceSound;
	public AudioClip pickupSound;

	private AudioSource audioSource;

	private static AudioController instance = null;

	void Awake(){
		if (AudioController.instance == null) {
			AudioController.instance = this;
			this.audioSource = this.GetComponent<AudioSource>();

			GameObject.DontDestroyOnLoad(this.gameObject);
		} else {
			GameObject.Destroy(this.gameObject);
		}
	}

	public static void PlayAudioLoop( AudioClip clip ){
		instance.audioSource.Stop ();
		instance.audioSource.loop = true;
		instance.audioSource.clip = clip;
		instance.audioSource.Play ();
	}

	public static void PlayInterfaceSound(){
		instance.audioSource.PlayOneShot (instance.interfaceSound);
	}

	public static void PlayPickupSound(){
		instance.audioSource.PlayOneShot (instance.pickupSound);
	}
}

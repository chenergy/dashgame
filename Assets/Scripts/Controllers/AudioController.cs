using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour {
	[Range(0.0f, 1.0f)]
	public float sfxVolumeScale = 1.0f;
	[Range(0.0f, 1.0f)]
	public float bgVolumeScale = 1.0f;

	//public UISlider		sfxSlider;
	//public UISlider		bgSlider;
	public AudioClip 	interfaceSound;
	public AudioClip 	pickupSound;

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
		instance.audioSource.PlayOneShot (instance.interfaceSound, instance.sfxVolumeScale);
	}

	public static void PlayPickupSound(){
		instance.audioSource.PlayOneShot (instance.pickupSound, instance.sfxVolumeScale);
	}

	public static void SetSFXVolume(float volume){
		instance.sfxVolumeScale = volume;
	}


	public static void SetBGVolume(float volume){
		instance.bgVolumeScale = volume;
		instance.audioSource.volume = volume;
	}

	public static float SFXVolume {
		get { return instance.sfxVolumeScale; }
		set { instance.sfxVolumeScale = value; }
	}

	public static float BGVolume {
		get { return instance.bgVolumeScale; }
		set {
			instance.bgVolumeScale = value;
			instance.audioSource.volume = value;
		}
	}

	/*
	public void ChangeSFXVolumeUI(){
		if (instance.sfxSlider != null) {
			instance.sfxVolumeScale = instance.sfxSlider.value;
		}
	}


	public void ChangeBGVolumeUI(){
		if (instance.bgVolumeScale != null) {
			instance.bgVolumeScale = instance.bgSlider.value;
		}
	}
	*/
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISlider))]
public class AudioSetVolume : MonoBehaviour
{
	public bool changeSfxVolume = false;
	public bool changeBgVolume = false;

	private UISlider slider;

	// Use this for initialization
	void Start ()
	{
		this.slider = this.GetComponent<UISlider> ();

		if (this.changeBgVolume) {
			this.slider.value = AudioController.BGVolume;
		} else if (this.changeSfxVolume) {
			this.slider.value = AudioController.SFXVolume;
		}
	}

	public void ChangeValue(){
		if (this.changeBgVolume) {
			AudioController.BGVolume = this.slider.value;
		} else if (this.changeSfxVolume) {
			AudioController.SFXVolume = this.slider.value;
		}
	}
}

